using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using FastMember;
using ObjectiveHarmonyPatch;

namespace AtsEx.Extensions.PreTrainPatch
{
    /// <summary>
    /// 先行列車の走行位置を自由に変更できるようにするパッチを表します。
    /// </summary>
    public sealed class PreTrainPatch
    {
        private readonly SectionManager SectionManager;
        private readonly HarmonyPatch HarmonyPatch;

        internal PreTrainPatch(string name, FastMethod updatePreTrainLocationMethod, SectionManager sectionManager, Action overrideAction)
        {
            SectionManager = sectionManager;

            HarmonyPatch = HarmonyPatch.Patch(name, updatePreTrainLocationMethod.Source, PatchType.Prefix);
            HarmonyPatch.Invoked += Prefix;


            PatchInvokationResult Prefix(object sender, PatchInvokedEventArgs e)
            {
                if (e.Instance != SectionManager.Src) return PatchInvokationResult.DoNothing(e);

                overrideAction();

                return new PatchInvokationResult(SkipModes.SkipOriginal);
            }
        }

        internal PreTrainPatch(string name, FastMethod updatePreTrainLocationMethod, SectionManager sectionManager, IPreTrainLocationConverter converter)
            : this(name, updatePreTrainLocationMethod, sectionManager, () => UpdatePreTrainLocation(sectionManager, converter))
        {
        }

        private static void UpdatePreTrainLocation(SectionManager sectionManager, IPreTrainLocationConverter converter)
        {
            MapObjectList passObjects = sectionManager.PreTrainPassObjects;

            PreTrainLocation source;
            if (passObjects.Count == 0 || sectionManager.Sections.Count == 0)
            {
                source = new PreTrainLocation(-1, sectionManager.Sections.Count - 1);
            }
            else
            {
                int timeMilliseconds = sectionManager.TimeManager.TimeMilliseconds;

                while (passObjects.CurrentIndex >= 0 && ((ValueNode<int>)passObjects[passObjects.CurrentIndex]).Value > timeMilliseconds)
                {
                    passObjects.CurrentIndex--;
                }
                while (passObjects.CurrentIndex + 1 < passObjects.Count && ((ValueNode<int>)passObjects[passObjects.CurrentIndex + 1]).Value <= timeMilliseconds)
                {
                    passObjects.CurrentIndex++;
                }

                double newPreTrainLocation;
                if (passObjects.CurrentIndex < 0)
                {
                    newPreTrainLocation = passObjects[0].Location;
                }
                else if (passObjects.Count - 2 <= passObjects.CurrentIndex)
                {
                    newPreTrainLocation = passObjects[passObjects.Count - 1].Location;
                }
                else
                {
                    ValueNode<int> prev = (ValueNode<int>)passObjects[passObjects.CurrentIndex];
                    ValueNode<int> next = (ValueNode<int>)passObjects[passObjects.CurrentIndex + 1];

                    double timePassingRate = (double)(timeMilliseconds - prev.Value) / (next.Value - prev.Value);
                    newPreTrainLocation = (1.0 - timePassingRate) * prev.Location + timePassingRate * next.Location;
                }

                source = PreTrainLocation.FromLocation(newPreTrainLocation, sectionManager);
            }

            PreTrainLocation converted = converter.Convert(source);

            sectionManager.PreTrainLocation = converted.Location;
            if (sectionManager.PreTrainSectionIndex != converted.SectionIndex)
            {
                sectionManager.PreTrainSectionIndex = converted.SectionIndex;
                sectionManager.OnSectionChanged();
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            HarmonyPatch.Dispose();
        }
    }
}

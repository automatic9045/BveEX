using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.Extensions.ContextMenuHacker
{
    [Plugin(PluginType.Extension)]
    [ExtensionMainDisplayType(typeof(IContextMenuHacker))]
    internal sealed class ContextMenuHacker : AssemblyPluginBase, IContextMenuHacker
    {
        private readonly ToolStripItemCollection ContextMenuItems;
        private readonly int StartIndex;

        private readonly SortedList<int, List<ToolStripItem>> AddedItems = new SortedList<int, List<ToolStripItem>>();

        public ContextMenuHacker(PluginBuilder builder) : base(builder)
        {
            ContextMenuItems = BveHacker.MainForm.ContextMenu.Items;
            StartIndex = ContextMenuItems.IndexOfKey("toolStripMenuItem") + 1;

            foreach (ContextMenuItemType itemType in Enum.GetValues(typeof(ContextMenuItemType)))
            {
                AddedItems.Add((int)itemType, new List<ToolStripItem>());
            }

            _ = AddSeparator(ContextMenuItemType.CoreAndExtensions);
        }

        public override void Dispose()
        {
            foreach (ContextMenuItemType itemType in Enum.GetValues(typeof(ContextMenuItemType)))
            {
                Clear(itemType);
            }
        }

        public override void Tick(TimeSpan elapsed)
        {
        }

        private void Clear(ContextMenuItemType itemType)
        {
            List<ToolStripItem> targetItems = AddedItems[(int)itemType];
            targetItems.ForEach(item => ContextMenuItems.Remove(item));
        }

        public ToolStripItem AddItem(ToolStripItem item, ContextMenuItemType itemType)
        {
            int itemTypeNum = (int)itemType;

            int targetIndex = StartIndex;
            foreach (KeyValuePair<int, List<ToolStripItem>> x in AddedItems)
            {
                if (x.Key > itemTypeNum) break;

                List<ToolStripItem> disposedItems = x.Value.FindAll(y => y.IsDisposed);
                disposedItems.ForEach(disposedItem => x.Value.Remove(disposedItem));

                targetIndex += x.Value.Count;
            }

            ContextMenuItems.Insert(targetIndex, item);
            AddedItems[itemTypeNum].Add(item);

            return item;
        }

        public ToolStripMenuItem AddClickableMenuItem(string text, EventHandler click, ContextMenuItemType itemType)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(text);
            item.Click += click;
            _ = AddItem(item, itemType);

            return item;
        }

        public ToolStripMenuItem AddCheckableMenuItem(string text, EventHandler checkedChanged, ContextMenuItemType itemType)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(text)
            {
                CheckOnClick = true,
            };
            if (!(checkedChanged is null)) item.CheckedChanged += checkedChanged;
            _ = AddItem(item, itemType);

            return item;
        }

        public ToolStripMenuItem AddCheckableMenuItem(string text, ContextMenuItemType itemType) => AddCheckableMenuItem(text, null, itemType);

        public ToolStripSeparator AddSeparator(ContextMenuItemType itemType)
        {
            ToolStripSeparator item = new ToolStripSeparator();
            _ = AddItem(item, itemType);

            return item;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

namespace AtsEx.Extensions.PreTrainPatch
{
    /// <summary>
    /// 先行列車の走行位置を表します。
    /// </summary>
    public class PreTrainLocation
    {
        /// <summary>
        /// 距離程 [m] を取得します。
        /// </summary>
        public double Location { get; }

        /// <summary>
        /// 閉塞のインデックスを取得します。
        /// </summary>
        public int SectionIndex { get; }

        /// <summary>
        /// <see cref="PreTrainLocation"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="location">距離程 [m]。</param>
        /// <param name="sectionIndex">閉塞のインデックス。</param>
        public PreTrainLocation(double location, int sectionIndex)
        {
            Location = location;
            SectionIndex = sectionIndex;
        }

        /// <summary>
        /// 距離程から走行位置の閉塞を特定し、<see cref="PreTrainLocation"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="location">距離程 [m]。この値は走行位置の閉塞の特定に使用されます。</param>
        /// <param name="sectionManager">走行位置の閉塞を計算するのに用いる <see cref="SectionManager"/>。</param>
        /// <returns>生成した <see cref="PreTrainLocation"/>。</returns>
        public static PreTrainLocation FromLocation(double location, SectionManager sectionManager)
        {
            int sectionIndex = sectionManager.PreTrainSectionIndex;
            while (sectionIndex >= 0 && sectionManager.Sections[sectionIndex].Location > location) sectionIndex--;
            while (sectionIndex < sectionManager.Sections.Count - 1 && sectionManager.Sections[sectionIndex + 1].Location <= location) sectionIndex++;

            return new PreTrainLocation(location, sectionIndex);
        }
    }
}

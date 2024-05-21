using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveTypes
{
    /// <summary>
    /// <see cref="BveTypeSetFactory.DifferentVersionProfileLoaded"/> イベントの情報を提供します。
    /// </summary>
    public class DifferentVersionProfileLoadedEventArgs
    {
        /// <summary>
        /// BVE 本体のバージョンを取得します。
        /// </summary>
        public Version BveVersion { get; }

        /// <summary>
        /// 代用されたプロファイルのバージョンを取得します。
        /// </summary>
        public Version ProfileVersion { get; }

        /// <summary>
        /// <see cref="DifferentVersionProfileLoadedEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="bveVersion">BVE 本体のバージョン。</param>
        /// <param name="profileVersion">代用されたプロファイルのバージョン。</param>
        public DifferentVersionProfileLoadedEventArgs(Version bveVersion, Version profileVersion)
        {
            BveVersion = bveVersion;
            ProfileVersion = profileVersion;
        }
    }
}

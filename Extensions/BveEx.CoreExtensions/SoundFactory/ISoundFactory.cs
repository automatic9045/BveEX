using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.Extensions.SoundFactory
{
    /// <summary>
    /// BVE で利用可能な音声を簡単に読み込むための機能を提供します。
    /// </summary>
    public interface ISoundFactory : IExtension
    {
        /// <summary>
        /// <see cref="LoadFrom(string, double, Sound.SoundPosition, int)"/> メソッドが使用可能であるかどうかを取得します。
        /// </summary>
        /// <remarks>
        /// <see cref="AtsPlugin"/> オブジェクトの生存期間 (コンストラクタが実行されてから、<see cref="AtsPlugin.Dispose"/> メソッドが呼び出されるまで) に一致します。
        /// </remarks>
        bool IsAvailable { get; }

        /// <summary>
        /// 音声ファイルを読み込んで <see cref="Sound"/> を生成します。
        /// </summary>
        /// <param name="path">音声ファイルのパス。</param>
        /// <param name="minRadius">視点からの最小距離 [m]。この値は音量の計算に使用されます。</param>
        /// <param name="position">音源が地上にあるか車上にあるか。</param>
        /// <param name="bufferCount">この音声を同時に再生できる数。</param>
        /// <returns>音声ファイルにより生成された <see cref="Sound"/>。</returns>
        Sound LoadFrom(string path, double minRadius, Sound.SoundPosition position, int bufferCount = 1);
    }
}

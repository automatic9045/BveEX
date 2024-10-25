using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Sound.Native
{
    /// <summary>
    /// AtsEX プラグインから ATS サウンドを再生するための機能を提供します。
    /// </summary>
    public interface IAtsSoundSet
    {
        /// <summary>
        /// インデックスを指定して、AtsEX から使用する ATS サウンドを登録します。
        /// </summary>
        /// <param name="index">ATS サウンドのインデックス。</param>
        /// <returns>登録した ATS サウンドを AtsEX から再生するためのオブジェクト。</returns>
        IAtsSound Register(int index);
    }
}

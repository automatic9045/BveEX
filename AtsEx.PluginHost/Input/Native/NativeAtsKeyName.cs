using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Input.Native
{
    /// <summary>
    /// BVE が ATS プラグイン向けに提供する ATS キーの種類を指定します。
    /// </summary>
    public enum NativeAtsKeyName
    {
        /// <summary>ATS S キー (既定: Space)。</summary>
        S = 0,
        /// <summary>ATS A1 キー (既定: Insert)。</summary>
        A1 = 1,
        /// <summary>ATS A2 キー (既定: Delete)。</summary>
        A2 = 2,
        /// <summary>ATS B1 キー (既定: Home)。</summary>
        B1 = 3,
        /// <summary>ATS B2 キー (既定: End)。</summary>
        B2 = 4,
        /// <summary>ATS C1 キー (既定: PageUp)。</summary>
        C1 = 5,
        /// <summary>ATS C2 キー (既定: PageDown)。</summary>
        C2 = 6,
        /// <summary>ATS D キー (既定: D2)。</summary>
        D = 7,
        /// <summary>ATS E キー (既定: D3)。</summary>
        E = 8,
        /// <summary>ATS F キー (既定: D4)。</summary>
        F = 9,
        /// <summary>ATS G キー (既定: D5)。</summary>
        G = 10,
        /// <summary>ATS H キー (既定: D6)。</summary>
        H = 11,
        /// <summary>ATS I キー (既定: D7)。</summary>
        I = 12,
        /// <summary>ATS J キー (既定: D8)。</summary>
        J = 13,
        /// <summary>ATS K キー (既定: D9)。</summary>
        K = 14,
        /// <summary>ATS L キー (既定: D0)。</summary>
        L = 15,
    }
}

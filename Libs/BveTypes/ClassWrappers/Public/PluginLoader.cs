using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mackoy.Bvets;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 互換性のために残されている旧名のクラスです。<see cref="AtsPlugin"/> を使用してください。
    /// </summary>
    [Obsolete]
    public class PluginLoader : AtsPlugin
    {

        /// <summary>
        /// オリジナル オブジェクトから <see cref="PluginLoader"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected PluginLoader(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="PluginLoader"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new PluginLoader FromSource(object src) => src is null ? null : new PluginLoader(src);

        public PluginLoader(UserVehicleLocationManager locationManager, KeyProvider keyProvider, HandleSet _0, HandleSet _1, VehicleStateStore vehicleStateStore, SectionManager sectionManager, MapFunctionList beacons, DoorSet doors)
            : base(locationManager, keyProvider, _0, _1, vehicleStateStore, sectionManager, beacons, doors)
        {
        }
    }
}

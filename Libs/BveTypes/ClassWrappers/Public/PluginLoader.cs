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

        /// <summary>
        /// <see cref="PluginLoader"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="locationManager">自列車の位置に関する情報。</param>
        /// <param name="keyProvider">キー入力に関する情報。</param>
        /// <param name="handles">自列車のノッチ情報。</param>
        /// <param name="atsHandles">ATS による指示を適用した自列車のノッチ情報。</param>
        /// <param name="vehicleStateStore">自列車の状態に関する情報。</param>
        /// <param name="sectionManager">閉塞の制御に関する情報。</param>
        /// <param name="beacons">地上子の一覧。</param>
        /// <param name="doors">自列車のドアの一覧。</param>
        public PluginLoader(UserVehicleLocationManager locationManager, KeyProvider keyProvider, HandleSet handles, HandleSet atsHandles, VehicleStateStore vehicleStateStore, SectionManager sectionManager, MapFunctionList beacons, DoorSet doors)
            : base(locationManager, keyProvider, handles, atsHandles, vehicleStateStore, sectionManager, beacons, doors)
        {
        }
    }
}

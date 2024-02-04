using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Mackoy.Bvets;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// キー入力を管理します。
    /// </summary>
    public class KeyProvider : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<KeyProvider>();

            InputDevicesGetMethod = members.GetSourcePropertyGetterOf(nameof(InputDevices));

            LeverMovedEvent = members.GetSourceEventOf(nameof(LeverMoved));
            KeyDownEvent = members.GetSourceEventOf(nameof(KeyDown));
            KeyUpEvent = members.GetSourceEventOf(nameof(KeyUp));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="KeyProvider"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected KeyProvider(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="KeyProvider"/> クラスのインスタンス。</returns>
        public static KeyProvider FromSource(object src) => src is null ? null : new KeyProvider(src);

        private static FastMethod InputDevicesGetMethod;
        /// <summary>
        /// 読み込まれている入力デバイスプラグインを取得します。
        /// </summary>
        public Dictionary<string, IInputDevice> InputDevices => InputDevicesGetMethod.Invoke(Src, null);

        private static FastEvent LeverMovedEvent;
        /// <summary>
        /// 運転台レバーが操作されたときに発生します。
        /// </summary>
        public event InputEventHandler LeverMoved
        {
            add => LeverMovedEvent.Add(Src, value);
            remove => LeverMovedEvent.Remove(Src, value);
        }
        /// <summary>
        /// <see cref="LeverMoved"/> イベントを実行します。
        /// </summary>
        /// <param name="args">対象となるキーの情報。</param>
        public void LeverMoved_Invoke(InputEventArgs args) => LeverMovedEvent.Invoke(Src, new object[] { (object)Src, args });

        private static FastEvent KeyDownEvent;
        /// <summary>
        /// キーが押されたときに発生します。
        /// </summary>
        public event InputEventHandler KeyDown
        {
            add => KeyDownEvent.Add(Src, value);
            remove => KeyDownEvent.Remove(Src, value);
        }
        /// <summary>
        /// <see cref="KeyDown"/> イベントを実行します。
        /// </summary>
        /// <param name="args">対象となるキーの情報。</param>
        public void KeyDown_Invoke(InputEventArgs args) => KeyDownEvent.Invoke(Src, new object[] { (object)Src, args });

        private static FastEvent KeyUpEvent;
        /// <summary>
        /// 押されていたキーが離されたときに発生します。
        /// </summary>
        public event InputEventHandler KeyUp
        {
            add => KeyUpEvent.Add(Src, value);
            remove => KeyUpEvent.Remove(Src, value);
        }
        /// <summary>
        /// <see cref="KeyUp"/> イベントを実行します。
        /// </summary>
        /// <param name="args">対象となるキーの情報。</param>
        public void KeyUp_Invoke(InputEventArgs args) => KeyUpEvent.Invoke(Src, new object[] { Src, args });
    }
}

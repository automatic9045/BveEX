using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

using BveTypes.ClassWrappers.Extensions;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// シナリオファイルから読み込んだ情報にアクセスするための機能を提供します。
    /// </summary>
    /// <remarks>
    /// シナリオ本体の詳細へアクセスするには <see cref="Scenario"/> を使用してください。
    /// </remarks>
    /// <seealso cref="Scenario"/>
    public class ScenarioInfo : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<ScenarioInfo>();

            FromFileMethod = members.GetSourceMethodOf(nameof(FromFile));

            PathGetMethod = members.GetSourcePropertyGetterOf(nameof(Path));
            PathSetMethod = members.GetSourcePropertySetterOf(nameof(Path));

            FileNameGetMethod = members.GetSourcePropertyGetterOf(nameof(FileName));
            FileNameSetMethod = members.GetSourcePropertySetterOf(nameof(FileName));

            DirectoryNameGetMethod = members.GetSourcePropertyGetterOf(nameof(DirectoryName));
            DirectoryNameSetMethod = members.GetSourcePropertySetterOf(nameof(DirectoryName));

            TitleGetMethod = members.GetSourcePropertyGetterOf(nameof(Title));
            TitleSetMethod = members.GetSourcePropertySetterOf(nameof(Title));

            ImagePathGetMethod = members.GetSourcePropertyGetterOf(nameof(ImagePath));
            ImagePathSetMethod = members.GetSourcePropertySetterOf(nameof(ImagePath));

            AuthorGetMethod = members.GetSourcePropertyGetterOf(nameof(Author));
            AuthorSetMethod = members.GetSourcePropertySetterOf(nameof(Author));

            CommentGetMethod = members.GetSourcePropertyGetterOf(nameof(Comment));
            CommentSetMethod = members.GetSourcePropertySetterOf(nameof(Comment));

            RouteFilesGetMethod = members.GetSourcePropertyGetterOf(nameof(RouteFiles));
            RouteFilesSetMethod = members.GetSourcePropertySetterOf(nameof(RouteFiles));

            VehicleFilesGetMethod = members.GetSourcePropertyGetterOf(nameof(VehicleFiles));
            VehicleFilesSetMethod = members.GetSourcePropertySetterOf(nameof(VehicleFiles));

            RouteTitleGetMethod = members.GetSourcePropertyGetterOf(nameof(RouteTitle));
            RouteTitleSetMethod = members.GetSourcePropertySetterOf(nameof(RouteTitle));

            VehicleTitleGetMethod = members.GetSourcePropertyGetterOf(nameof(VehicleTitle));
            VehicleTitleSetMethod = members.GetSourcePropertySetterOf(nameof(VehicleTitle));

            ScenarioFileLoadErrorsGetMethod = members.GetSourcePropertyGetterOf(nameof(ScenarioFileLoadErrors));
            ScenarioFileLoadErrorsSetMethod = members.GetSourcePropertySetterOf(nameof(ScenarioFileLoadErrors));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="ScenarioInfo"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected ScenarioInfo(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="ScenarioInfo"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static ScenarioInfo FromSource(object src) => src is null ? null : new ScenarioInfo(src);

        private static FastMethod FromFileMethod;
        /// <summary>
        /// 読み込むシナリオファイルを指定して <see cref="ScenarioInfo"/> クラスのインスタンスを生成します。
        /// </summary>
        /// <param name="path">読み込むシナリオファイルのパス。</param>
        /// <returns>シナリオファイルから読み込んだ情報を表す <see cref="ScenarioInfo"/>。</returns>
        public static ScenarioInfo FromFile(string path)
        {
            object src = FromFileMethod.Invoke(null, new object[] { path });
            return new ScenarioInfo(src);
        }

        private static FastMethod PathGetMethod;
        private static FastMethod PathSetMethod;
        /// <summary>
        /// シナリオファイルのパスを取得・設定します。
        /// </summary>
        public string Path
        {
            get => PathGetMethod.Invoke(Src, null) as string;
            set => PathSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod FileNameGetMethod;
        private static FastMethod FileNameSetMethod;
        /// <summary>
        /// シナリオファイルのファイル名を取得・設定します。
        /// </summary>
        public string FileName
        {
            get => FileNameGetMethod.Invoke(Src, null) as string;
            set => FileNameSetMethod.Invoke(Src, new object[] { value } );
        }

        private static FastMethod DirectoryNameGetMethod;
        private static FastMethod DirectoryNameSetMethod;
        /// <summary>
        /// シナリオファイルのディレクトリ情報を取得・設定します。
        /// </summary>
        public string DirectoryName
        {
            get => DirectoryNameGetMethod.Invoke(Src, null) as string;
            set => DirectoryNameSetMethod.Invoke(Src, new object[] { value } );
        }

        private static FastMethod TitleGetMethod;
        private static FastMethod TitleSetMethod;
        /// <summary>
        /// シナリオのタイトルを取得・設定します。
        /// </summary>
        public string Title
        {
            get => TitleGetMethod.Invoke(Src, null) as string;
            set => TitleSetMethod.Invoke(Src, new object[] { value } );
        }

        private static FastMethod ImagePathGetMethod;
        private static FastMethod ImagePathSetMethod;
        /// <summary>
        /// シナリオのサムネイル画像のパスを取得・設定します。
        /// </summary>
        public string ImagePath
        {
            get => ImagePathGetMethod.Invoke(Src, null) as string;
            set => ImagePathSetMethod.Invoke(Src, new object[] { value } );
        }

        private static FastMethod AuthorGetMethod;
        private static FastMethod AuthorSetMethod;
        /// <summary>
        /// シナリオの作者を取得・設定します。
        /// </summary>
        public string Author
        {
            get => AuthorGetMethod.Invoke(Src, null) as string;
            set => AuthorSetMethod.Invoke(Src, new object[] { value } );
        }

        private static FastMethod CommentGetMethod;
        private static FastMethod CommentSetMethod;
        /// <summary>
        /// シナリオの説明を取得・設定します。
        /// </summary>
        public string Comment
        {
            get => CommentGetMethod.Invoke(Src, null) as string;
            set => CommentSetMethod.Invoke(Src, new object[] { value } );
        }

        private static FastMethod RouteFilesGetMethod;
        private static FastMethod RouteFilesSetMethod;
        /// <summary>
        /// 選択されうるマップファイルのリストを取得・設定します。
        /// </summary>
        public RandomFileList RouteFiles
        {
            get => RandomFileList.FromSource(RouteFilesGetMethod.Invoke(Src, null));
            set => RouteFilesSetMethod.Invoke(Src, new object[] { value?.Src } );
        }

        private static FastMethod VehicleFilesGetMethod;
        private static FastMethod VehicleFilesSetMethod;
        /// <summary>
        /// 選択されうる車両ファイルのリストを取得・設定します。
        /// </summary>
        public RandomFileList VehicleFiles
        {
            get => RandomFileList.FromSource(VehicleFilesGetMethod.Invoke(Src, null));
            set => VehicleFilesSetMethod.Invoke(Src, new object[] { value?.Src } );
        }

        private static FastMethod RouteTitleGetMethod;
        private static FastMethod RouteTitleSetMethod;
        /// <summary>
        /// 路線名を取得・設定します。
        /// </summary>
        public string RouteTitle
        {
            get => RouteTitleGetMethod.Invoke(Src, null) as string;
            set => RouteTitleSetMethod.Invoke(Src, new object[] { value } );
        }

        private static FastMethod VehicleTitleGetMethod;
        private static FastMethod VehicleTitleSetMethod;
        /// <summary>
        /// 車両名を取得・設定します。
        /// </summary>
        public string VehicleTitle
        {
            get => VehicleTitleGetMethod.Invoke(Src, null) as string;
            set => VehicleTitleSetMethod.Invoke(Src, new object[] { value } );
        }

        private static FastMethod ScenarioFileLoadErrorsGetMethod;
        private static FastMethod ScenarioFileLoadErrorsSetMethod;
        /// <summary>
        /// シナリオファイルの読込時に発生した、継続不能となる深刻なエラーを取得・設定します。
        /// </summary>
        public WrappedList<LoadError> ScenarioFileLoadErrors
        {
            get => WrappedList<LoadError>.FromSource(ScenarioFileLoadErrorsGetMethod.Invoke(Src, null) as IList);
            set => ScenarioFileLoadErrorsSetMethod.Invoke(Src, new object[] { value?.Src });
        }
    }
}

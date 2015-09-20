using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using PowerPoint = NetOffice.PowerPointApi;

namespace imageClipPaste.Models.Office
{
    /// <summary>
    /// NetOffice.PowerPointを操作するクラス
    /// </summary>
    class PowerPointModel
    {
        /// <summary>NLlog</summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// PowerPointのインストール状況を取得します。
        /// </summary>
        /// <returns></returns>
        public static bool IsInstalled()
        {
            Type officeType = Type.GetTypeFromProgID("PowerPoint.Application");
            return officeType != null;
        }

        /// <summary>
        /// PowerPointアプリケーションを取得します
        /// </summary>
        /// <returns>
        /// PowerPointプロセスが取得できる場合は、取得したPowerPointアプリケーションを、
        /// 取得できない場合は、新しくPowerPointプロセスを立ち上げ、そのPowerPointアプリケーションを返却します
        /// </returns>
        public static PowerPoint.Application GetApplication()
        {
            var app = PowerPoint.Application.GetActiveInstance();
            return (app == null) ? GetNewApplication() : app;
        }

        /// <summary>
        /// 新しいPowerPointアプリケーションを取得します
        /// </summary>
        /// <returns>
        /// 新しく起動したPowerPointプロセス
        /// </returns>
        public static PowerPoint.Application GetNewApplication()
        {
            return new PowerPoint.Application
            {
                Visible = NetOffice.OfficeApi.Enums.MsoTriState.msoTrue
            };
        }

        /// <summary>
        /// 引数に合致するPowerPointアプリケーションを取得します
        /// </summary>
        /// <param name="info">貼り付け先プロセス情報</param>
        /// <returns>
        /// 合致するPowerPointアプリケーションが取得できる場合は、取得したPowerPointアプリケーションを、
        /// 取得できない場合は、nullを返却します
        /// </returns>
        public static PowerPoint.Application FindApplication(Settings.PasteProcessInfo info)
        {
            var applications = PowerPoint.Application.GetActiveInstances();
            var findApp = applications
                .ToList()
                .First((app) => FindPresentation(app, info) != null);

            // 合致しなかったApplicationを破棄します
            applications
                .Where((app) => !ReferenceEquals(app, findApp))
                .ToList()
                .ForEach((app) => app.Dispose());

            return findApp;
        }

        /// <summary>
        /// 引数に合致するPowerPointプレゼンテーションを取得します
        /// </summary>
        /// <param name="app">PowerPointアプリケーション</param>
        /// <param name="name">プレゼンテーションの名前</param>
        /// <returns>
        /// 合致するプレゼンテーションが取得できる場合は、取得したプレゼンテーションを、
        /// 取得できない場合はnullを返却します
        /// </returns>
        public static PowerPoint.Presentation FindPresentation(PowerPoint.Application app, Settings.PasteProcessInfo info)
        {
            return app.Presentations
                .ToList().Cast<PowerPoint.Presentation>()
                .First((p) => p.FullName == info.FullName);

            // app.Presentationsで参照したPresentationはここで破棄しません
            // 呼び出し元でapp.Dispose()時に、子要素のDispose()が呼ばれるはずです
        }

        /// <summary>
        /// 開かれているPowerPointのスライド名をリストで取得します
        /// </summary>
        /// <returns></returns>
        public static List<Settings.PasteProcessInfo> GetPasteProcessList()
        {
            List<Settings.PasteProcessInfo> result = new List<Settings.PasteProcessInfo>();
            try
            {
                var apps = PowerPoint.Application.GetActiveInstances();
                apps.SelectMany(app => app.Presentations).Cast<PowerPoint.Presentation>()
                    .Where(p => p.ReadOnly == NetOffice.OfficeApi.Enums.MsoTriState.msoFalse)
                    .ToList()
                    .ForEach(p => result.Add(
                        new Settings.PasteProcessInfo
                        {
                            Name = p.Name,
                            FullName = p.FullName,
                            PasteType = Enums.PasteType.PowerPoint
                        }));

                apps.ToList()
                    .ForEach(app => app.Dispose());
            }
            catch (Exception ex)
            {
                _logger.Error("GetPasteProcessList: ", ex);
            }

            return result;
        }

        /// <summary>
        /// 画像ファイルをスライドに貼り付けます
        /// </summary>
        /// <param name="slide">貼り付け先のスライド</param>
        /// <param name="path">貼り付ける画像ファイルパス</param>
        /// <returns></returns>
        public static PowerPoint.Shape AddShapeFromImageFile(PowerPoint.Slide slide, string path)
        {
            // width, heightは、追加後にScaleを調整するので 0を指定します。
            float width = 0, height = 0;
            float left = 0, top = 0;

            var shape = slide.Shapes.AddPicture(
                path,
                NetOffice.OfficeApi.Enums.MsoTriState.msoFalse,
                NetOffice.OfficeApi.Enums.MsoTriState.msoTrue,
                left,
                top,
                width,
                height);

            // 貼り付けた画像の、拡大/縮小率を100%に設定します。
            shape.ScaleHeight(1, NetOffice.OfficeApi.Enums.MsoTriState.msoTrue);
            shape.ScaleWidth(1, NetOffice.OfficeApi.Enums.MsoTriState.msoTrue);

            return shape;
        }
    }
}

using imageClipPaste.Enums;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using Excel = NetOffice.ExcelApi;

namespace imageClipPaste.Models.Office
{
    /// <summary>
    /// NetOffice.Excelを操作するクラス
    /// </summary>
    public class ExcelModel
    {
        /// <summary>NLog</summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Excelのインストール状況を取得します
        /// </summary>
        /// <returns></returns>
        public static bool IsInstalledExcel()
        {
            Type officeType = Type.GetTypeFromProgID("Excel.Application");
            return officeType != null;
        }

        /// <summary>
        /// 新しいExcelアプリケーションを取得します
        /// </summary>
        /// <returns>
        /// Excelプロセスが取得できる場合は、取得したExcelアプリケーションを、
        /// 取得できない場合は、新しくExcelプロセスを立ち上げ、そのExcelアプリケーションを返却します
        /// </returns>
        public static Excel.Application GetExcelApplication()
        {
            var app = Excel.Application.GetActiveInstance();
            if (app == null)
            {
                // Excelプロセスを立ち上げた場合は
                // デフォルトで非表示となっているので、表示に切り替える
                app = new Excel.Application
                {
                    Visible = true
                };
            }
            return app;
        }

        /// <summary>
        /// 引数に合致するExcelアプリケーションを取得します
        /// </summary>
        /// <param name="info">貼り付け先プロセス情報</param>
        /// <returns>
        /// 合致するExcelアプリケーションが取得できる場合は、取得したExcelアプリケーションを、
        /// 取得できない場合は、nullを返却します
        /// </returns>
        public static Excel.Application FindExcelApplication(Settings.PasteProcessInfo info)
        {
            var applications = Excel.Application.GetActiveInstances();
            var findApp = applications
                .ToList()
                .First((app) => FindExcelWorkbook(app, info) != null);

            // 合致しなかったExcelApplicationを破棄します
            applications
                .Where((app) => !ReferenceEquals(app, findApp))
                .ToList()
                .ForEach((app) => app.Dispose());

            return findApp;
        }

        /// <summary>
        /// 引数に合致するExcelワークブックを取得します
        /// </summary>
        /// <param name="app">取得先のExcelアプリケーション</param>
        /// <param name="info">ワークブックを特定する情報</param>
        /// <returns>
        /// 合致するワークブックが取得できる場合は、取得したワークブックを、
        /// 取得できない場合はnullを返却します
        /// </returns>
        public static Excel.Workbook FindExcelWorkbook(Excel.Application app, Settings.PasteProcessInfo info)
        {
            return app.Workbooks
                .ToList()
                .First((b) => b.FullName == info.FullName);

            // app.Workbooksで参照したWorkbookはここで破棄しません
            // 呼び出し元でapp.Dispose()時に、子要素のDispose()が呼ばれるはず
        }

        /// <summary>
        /// 開かれているExcelのワークブック名をリストで取得します
        /// </summary>
        /// <returns></returns>
        public static List<Settings.PasteProcessInfo> GetPasteExcelProcessList()
        {
            List<Settings.PasteProcessInfo> result = new List<Settings.PasteProcessInfo>();
            Excel.Application[] applications = null;
            try
            {
                applications = Excel.Application.GetActiveInstances();
                applications
                    .SelectMany(app => app.Workbooks)
                    .Where(book => !book.ReadOnly)
                    .ToList()
                    .ForEach(book =>
                        result.Add(new Settings.PasteProcessInfo
                        {
                            Name = book.Name,
                            FullName = book.FullName,
                            PasteType = PasteType.Excel
                        }));

                applications
                    .ToList()
                    .ForEach(app => app.Dispose());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GetPasteExcelProcessList: ");
            }

            return result;
        }

        /// <summary>
        /// 画像ファイルをワークシートに貼り付けます
        /// </summary>
        /// <param name="sheet">貼り付け先のワークシート</param>
        /// <param name="path">貼り付ける画像ファイルパス</param>
        /// <returns></returns>
        public static Excel.Shape AddShapeFromImageFile(Excel.Worksheet sheet, string path)
        {
            return AddShapeFromImageFile(sheet, path, 0, 0);
        }

        /// <summary>
        /// 画像ファイルをワークシートの指定された位置に貼り付けます
        /// </summary>
        /// <param name="sheet">貼り付け先のワークシート</param>
        /// <param name="path">貼り付ける画像ファイルパス</param>
        /// <param name="top">Top座標</param>
        /// <param name="left">Left座標</param>
        /// <returns></returns>
        public static Excel.Shape AddShapeFromImageFile(Excel.Worksheet sheet, string path, float top, float left)
        {
            // width, heightは、追加後にScaleを調整するので 0を指定します。
            float width = 0, height = 0;

            var shape = sheet.Shapes.AddPicture(
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

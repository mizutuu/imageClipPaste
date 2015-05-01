using imageClipPaste.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = NetOffice.ExcelApi;

namespace imageClipPaste.Models.Office
{
    /// <summary>
    /// NetOffice.Excelを操作するクラス
    /// </summary>
    public class ExcelModel
    {
        /// <summary>
        /// 新しいExcelアプリケーションを取得します
        /// </summary>
        /// <returns>
        /// Excelプロセスが取得できる場合は、取得したExcelアプリケーションを、
        /// 取得できない場合は、新しくExcelプロセスを立ち上げ、そのExcelアプリケーションを返却します
        /// </returns>
        public static Excel.Application NewExcelApplication()
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

                // 新しいプロセスではワークブックが無いので追加します
                app.Workbooks.Add();
            }
            return app;
        }

        /// <summary>
        /// 引数に合致するExcelアプリケーションを取得します
        /// </summary>
        /// <param name="Hinstance">インスタンスハンドル</param>
        /// <param name="Hwnd">ウィンドウハンドル</param>
        /// <returns>
        /// 合致するExcelアプリケーションが取得できる場合は、取得したExcelアプリケーションを、
        /// 取得できない場合は、nullを返却します
        /// </returns>
        public static Excel.Application FindExcelApplication(int Hinstance, int Hwnd)
        {
            Excel.Application findApp = null;
            Excel.Application.GetActiveInstances()
                .ToList()
                .ForEach((app) =>
                {
                    if (app.Hinstance == Hinstance && app.Hwnd == Hwnd)
                    {
                        findApp = app;
                    }
                    else
                    {
                        app.Dispose();
                    }
                });
            return findApp;
        }

        /// <summary>
        /// 引数に合致するExcelワークブックを取得します
        /// </summary>
        /// <param name="app">取得先のExcelアプリケーション</param>
        /// <param name="name">ワークブックの名前</param>
        /// <returns>
        /// 合致するワークブックが取得できる場合は、取得したワークブックを、
        /// 取得できない場合はnullを返却します
        /// </returns>
        public static Excel.Workbook FindExcelWorkbook(Excel.Application app, string name)
        {
            if (app == null)
                return null;

            Excel.Workbook findBook = null;
            using (var books = app.Workbooks)
            {
                books
                    .ToList()
                    .ForEach((book) =>
                    {
                        if (book.Name == name)
                        {
                            findBook = book;
                        }
                        else
                        {
                            book.Dispose();
                        }
                    });
            }

            return findBook;
        }

        /// <summary>
        /// 開かれているExcelのワークブック名をリストで取得します
        /// </summary>
        /// <returns></returns>
        public static List<Settings.PasteProcessInfo> GetPasteExcelProcessList()
        {
            List<Settings.PasteProcessInfo> result = new List<Settings.PasteProcessInfo>();
            NetOffice.ExcelApi.Application[] applications = NetOffice.ExcelApi.Application.GetActiveInstances();
            applications
                .SelectMany(app => app.Workbooks)
                .ToList()
                .Where(book => !book.ReadOnly)
                .ToList()
                .ForEach(book =>
                    result.Add(new Settings.PasteProcessInfo
                    {
                        Name = book.Name,
                        Path = book.Path,
                        HInstance = book.Application.Hinstance,
                        HWnd = book.Application.Hwnd,
                        PasteType = PasteType.Excel
                    }));

            foreach (var app in applications)
                app.Dispose();

            return result;
        }
    }
}

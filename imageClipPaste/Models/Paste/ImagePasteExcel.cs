using imageClipPaste.Models.Office;
using NLog;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using Excel = NetOffice.ExcelApi;

namespace imageClipPaste.Models.Paste
{
    /// <summary>
    /// 画像をExcelに貼り付けます
    /// </summary>
    public class ImagePasteExcel : BaseImagePaste
    {
        /// <summary>
        /// Has Dispose already been called?
        /// </summary>
        bool disposed = false;

        /// <summary>
        /// 貼り付け可能状態
        /// </summary>
        public override bool IsPastable
        {
            get
            {
                return IsAlivePasteProcess();
            }
        }

        /// <summary>NLog</summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>貼り付け設定</summary>
        private Settings.PasteExcelSetting _setting;

        /// <summary>貼り付け先のプロセス情報</summary>
        private Settings.PasteProcessInfo _process;

        /// <summary>Excelアプリケーション</summary>
        private Excel.Application _xlsApplication;

        /// <summary>Excelアプリケーション配下のワークブック</summary>
        private Excel.Workbook _xlsWorkBook;

        /// <summary>Excel貼り付けのための一時保存先</summary>
        private string _tempImagePath;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="setting">貼り付け設定</param>
        public ImagePasteExcel(Settings.PasteProcessInfo process, Settings.PasteExcelSetting setting)
            : base()
        {
            _setting = setting;
            _process = process;
            _xlsApplication = GetExcelApplication(process);
            _xlsWorkBook = GetExcelWorkBook(_xlsApplication, process);
            _tempImagePath = Path.GetTempFileName();
        }

        /// <summary>
        /// 貼り付け先のプロセスとワークブックが生存しているか取得します
        /// </summary>
        private bool IsAlivePasteProcess()
        {
            if (_xlsApplication == null || _xlsWorkBook == null)
            {
                _logger.Debug("プロセス生存確認 Application: {0}, WorkBook: {1}",
                    _xlsApplication == null, _xlsWorkBook == null);
                return false;
            }

            // 見た限り、ExcelのApplicationになさそうだったので、
            // プロセスが無いときにRPCエラーとなることを利用しています。
            try
            {
                var dummy = _xlsApplication.Visible;
                dummy = _xlsWorkBook.ReadOnly;
            }
            catch (COMException ex)
            {
                _logger.Debug("プロセス生存確認", ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Excelアプリケーションを取得します
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        private Excel.Application GetExcelApplication(Settings.PasteProcessInfo process)
        {
            if (process.IsRequiredNew)
            {
                return ExcelModel.GetExcelApplication();
            }
            else
            {
                return ExcelModel.FindExcelApplication(process.HInstance, process.HWnd);
            }
        }

        /// <summary>
        /// Excelワークブックを取得します
        /// </summary>
        /// <param name="app"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        private Excel.Workbook GetExcelWorkBook(Excel.Application app, Settings.PasteProcessInfo process)
        {
            if (app == null)
                return null;

            if (process.IsRequiredNew)
            {
                return app.Workbooks.Add();
            }
            else
            {
                return ExcelModel.FindExcelWorkbook(app, process.Name);
            }
        }

        /// <summary>
        /// 画像をExcelに貼り付けます
        /// </summary>
        /// <param name="image">貼り付ける画像</param>
        public override void Paste(BitmapImage image)
        {
            if (!IsPastable)
                return;

            // Excel貼り付けのため、ファイルに保存します。
            SavePngFile(image, _tempImagePath);

            // 画像を貼り付けた後、アクティブセルを画像の下に移動するため、
            // 貼り付け先のワークシートをアクティブ化します。
            // このとき、シートの切り替わりを目立たなくさせるためにScreenUpdatingをオフにします。
            using (var activeSheetInApplication = _xlsApplication.ActiveSheet as Excel.Worksheet)
            using (var activeSheet = _xlsWorkBook.ActiveSheet as Excel.Worksheet)
            {
                var screenUpdating = _xlsApplication.ScreenUpdating;
                _xlsApplication.ScreenUpdating = false;

                // 貼り付け先のワークシートをアクティブ化してからアクティブセルを取得
                activeSheet.Activate();
                using (var activeCell = _xlsApplication.ActiveCell)
                using (var shape = ExcelModel.AddShapeFromImageFile(activeSheet, _tempImagePath,
                    Convert.ToSingle(activeCell.Top), Convert.ToSingle(activeCell.Left)))
                {
                    _logger.Trace("{0}.{1} に貼り付けました。File: {2}", _xlsWorkBook.Name, activeSheet.Name, _tempImagePath);

                    if (_setting.MoveActiveCellInImageBelow)
                    {
                        using (var bottomRightCell = shape.BottomRightCell)
                        using (var activeWindow = _xlsApplication.ActiveWindow)
                        using (var visibleRange = activeWindow.VisibleRange)
                        {
                            // 画像の下のセルをアクティブセルに指定します
                            int nextRow = bottomRightCell.Row + 2,
                                nextColumn = activeCell.Column;
                            if (nextRow <= activeSheet.Cells.Rows.Count)
                            {
                                activeSheet.Cells[nextRow, nextColumn].Select();

                                // セルの表示範囲を超える場合は、上で指定したアクティブセルまでスクロールします
                                var visibleRows = visibleRange.Count / visibleRange.Column;
                                if (visibleRows <= 0)
                                    visibleRows = nextRow; // 算出できない場合は、次の行番号とする
                                var visibleLastRow = visibleRange.Cells[visibleRows].Row;
                                if (nextRow > visibleLastRow)
                                    activeWindow.ScrollRow += (nextRow - visibleLastRow);
                            }
                        }
                    }
                }

                activeSheetInApplication.Activate();
                _xlsApplication.ScreenUpdating = screenUpdating;
            }
        }

        /// <summary>
        /// Png形式でBitmapImageをファイルに保存します
        /// </summary>
        /// <param name="image">保存するBitmapImage</param>
        /// <param name="path">保存先</param>
        protected static void SavePngFile(BitmapImage image, string path)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                var encode = new PngBitmapEncoder();
                encode.Frames.Add(BitmapFrame.Create(image));
                encode.Save(stream);
            }
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (_xlsWorkBook != null)
                {
                    _xlsWorkBook.Dispose();
                    _xlsWorkBook = null;
                }
                if (_xlsApplication != null)
                {
                    _xlsApplication.Dispose();
                    _xlsApplication = null;
                }
                if (File.Exists(_tempImagePath))
                {
                    File.Delete(_tempImagePath);
                }
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }
}

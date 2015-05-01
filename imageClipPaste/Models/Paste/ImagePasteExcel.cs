using imageClipPaste.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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

        /// <summary>貼り付け設定</summary>
        private Settings.PasteExcelSetting _setting;

        /// <summary>貼り付け先のプロセス情報</summary>
        private Settings.PasteProcessInfo _process;

        /// <summary>Excelアプリケーション</summary>
        private NetOffice.ExcelApi.Application _xlsApplication;

        /// <summary>Excelアプリケーション配下のワークブック</summary>
        private NetOffice.ExcelApi.Workbook _xlsWorkBook;

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
        }

        /// <summary>
        /// 貼り付け先のプロセスとワークブックが生存しているか取得します
        /// </summary>
        private bool IsAlivePasteProcess()
        {
            if (_xlsApplication == null || _xlsWorkBook == null)
                return false;

            // 見た限り、ExcelのApplicationになさそうだったので、
            // プロセスが無いときにRPCエラーとなることを利用しています。
            try
            {
                var dummy = _xlsApplication.Visible;
                dummy = _xlsWorkBook.ReadOnly;
            }
            catch (COMException)
            {
                return false;
            }

            return false;
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
                return Office.ExcelModel.NewExcelApplication();
            }
            else
            {
                return Office.ExcelModel.FindExcelApplication(process.HInstance, process.HWnd);
            }
        }

        /// <summary>
        /// Excelワークブックを取得します
        /// </summary>
        /// <param name="app"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        private Excel.Workbook GetExcelWorkBook(NetOffice.ExcelApi.Application app, Settings.PasteProcessInfo process)
        {
            if (app == null)
                return null;

            if (process.IsRequiredNew)
            {
                // 新しいワークブックの場合は、アクティブブックを返却する
                return app.ActiveWorkbook;
            }
            else
            {
                return Office.ExcelModel.FindExcelWorkbook(app, process.Name);
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

            using (var activeSheet = _xlsApplication.ActiveSheet as NetOffice.ExcelApi.Worksheet)
            {

                return;

                using (var activeCell = activeSheet.Cells)
                {
                    
                }

                float left = 0,
                      top = 0;
                // width, heightは、追加後にScaleを調整するので 0を指定します。
                using (var shape = activeSheet.Shapes.AddPicture(
                    ""/*image path*/,
                    NetOffice.OfficeApi.Enums.MsoTriState.msoFalse,
                    NetOffice.OfficeApi.Enums.MsoTriState.msoTrue,
                    left,
                    top,
                    0,  // width はゼロ指定
                    0)) // height はゼロ指定
                {
                    // 貼り付けた画像の、拡大/縮小率を100%に設定します。
                    shape.ScaleHeight(1, NetOffice.OfficeApi.Enums.MsoTriState.msoTrue);
                    shape.ScaleWidth(1, NetOffice.OfficeApi.Enums.MsoTriState.msoTrue);
                }
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
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }
}

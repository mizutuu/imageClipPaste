using imageClipPaste.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace imageClipPaste.Services.Paste
{
    /// <summary>
    /// 画像をExcelに貼り付けます
    /// </summary>
    public class ImagePasteExcel : IImagePaste, IDisposable
    {
        /// <summary>
        /// 貼り付け可能状態
        /// </summary>
        public bool IsPastable { get { return IsAlivePasteProcess(); } }

        /// <summary>Excelアプリケーション</summary>
        private NetOffice.ExcelApi.Application _xlsApplication;

        /// <summary>
        /// 貼り付け先のプロセスが生存しているか取得します
        /// </summary>
        private bool IsAlivePasteProcess()
        {
            if (_xlsApplication == null)
                return false;

            // 見た限り、ExcelのApplicationになさそうだったので、
            // プロセスが無いときにRPCエラーとなることを利用しています。
            try
            {
                var dummy = _xlsApplication.Visible;
            }
            catch (COMException)
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// 画像をExcelに貼り付けます
        /// </summary>
        /// <param name="image">貼り付ける画像</param>
        public void Paste(BitmapImage image)
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
        /// アンマネージ リソースを開放します。
        /// </summary>
        public void Dispose()
        {
            if (_xlsApplication != null)
            {
                _xlsApplication.Dispose();
                _xlsApplication = null;
            }
        }
    }
}

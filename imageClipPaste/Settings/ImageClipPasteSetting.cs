using imageClipPaste.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageClipPaste.Settings
{
    /// <summary>
    /// アプリケーション設定情報を管理する
    /// </summary>
    [Serializable]
    public class ImageClipPasteSetting
    {
        /// <summary>
        /// クリップボードを監視する間隔
        /// </summary>
        public TimeSpan ClipboardMonitorInterval { get; set; }

        /// <summary>
        /// 貼り付け先のプロセス情報
        /// </summary>
        public PasteProcessInfo CurrentPasteProcessInfo { get; set; }

        /// <summary>
        /// Excelに貼り付けるときの設定
        /// </summary>
        public PasteExcelSetting ExcelSetting { get; set; }

        /// <summary>
        /// PowerPointに貼り付けるときの設定
        /// </summary>
        public PastePowerPointSetting PowerPointSetting { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ImageClipPasteSetting()
        {
            ClipboardMonitorInterval = TimeSpan.FromMilliseconds(200);
            CurrentPasteProcessInfo = new PasteProcessInfo();
            ExcelSetting = new PasteExcelSetting();
            PowerPointSetting = new PastePowerPointSetting();
        }
    }
}

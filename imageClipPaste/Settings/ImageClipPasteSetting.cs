﻿using imageClipPaste.Enums;
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
        public int ClipboardMonitorIntervalMilliseconds { get; set; }

        /// <summary>
        /// クリップボードから画像をコピーするときに、自動変換可能な画像をコピーする
        /// </summary>
        public bool IsClipAutoConvertibleImage { get; set; }

        /// <summary>
        /// 貼り付け先のプロセス情報
        /// </summary>
        public PasteProcessInfo CurrentPasteProcessInfo { get; set; }

        /// <summary>
        /// クリップボード監視時のクリップボードリセット設定
        /// </summary>
        public bool IsResetClipboard { get; set; }

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
            ClipboardMonitorIntervalMilliseconds = 200;
            IsClipAutoConvertibleImage = true;
            CurrentPasteProcessInfo = new PasteProcessInfo();
            IsResetClipboard = true;
            ExcelSetting = new PasteExcelSetting();
            PowerPointSetting = new PastePowerPointSetting();
        }
    }
}

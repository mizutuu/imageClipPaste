using GalaSoft.MvvmLight.Command;
using System;

namespace imageClipPaste.ViewModel
{
    /// <summary>
    /// アプリケーション設定ウィンドウのViewModelです。
    /// </summary>
    public class ApplicationSettingViewModel : ViewModelBase
    {
        #region Properties
        /// <summary>
        /// クリップボードの監視間隔
        /// </summary>
        private int intervalMilliseconds;
        public int IntervalMilliseconds
        {
            get { return intervalMilliseconds; }
            set { Set(ref intervalMilliseconds, value); }
        }

        /// <summary>
        /// クリップボードから画像をコピーするときに、自動変換可能な画像をコピーする
        /// </summary>
        private bool isClipAutoConvertibleImage;
        public bool IsClipAutoConvertibleImage
        {
            get { return isClipAutoConvertibleImage; }
            set { Set(ref isClipAutoConvertibleImage, value); }
        }

        /// <summary>
        /// Excel貼り付け設定
        /// </summary>
        private Settings.PasteExcelSetting excelSetting;
        public Settings.PasteExcelSetting ExcelSetting
        {
            get { return excelSetting; }
            set { Set(ref excelSetting, value); }
        }
        #endregion

        #region Commands
        /// <summary>
        /// OKボタンコマンド
        /// </summary>
        private RelayCommand onOkCommand;
        public RelayCommand OnOkCommand
        {
            get
            {
                return onOkCommand = onOkCommand ?? new RelayCommand(() =>
                {
                    // アプリケーション設定を、Settingsオブジェクトに設定します
                    Properties.Settings.Default.Setting.ClipboardMonitorIntervalMilliseconds = 
                        IntervalMilliseconds;
                    Properties.Settings.Default.Setting.IsClipAutoConvertibleImage = IsClipAutoConvertibleImage;
                    Properties.Settings.Default.Setting.ExcelSetting = ExcelSetting;

                    Properties.Settings.Default.Save();
                });
            }
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ApplicationSettingViewModel()
        {
            // アプリケーションの設定を読み込む
            IntervalMilliseconds = 
                Properties.Settings.Default.Setting.ClipboardMonitorIntervalMilliseconds;
            IsClipAutoConvertibleImage = Properties.Settings.Default.Setting.IsClipAutoConvertibleImage;
            ExcelSetting =
                Properties.Settings.Default.Setting.ExcelSetting;
        }

        /// <summary>
        /// MVVM Light ToolkitのCleanup
        /// </summary>
        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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

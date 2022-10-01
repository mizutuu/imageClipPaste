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
        /// 画像を貼り付けたあとで、アクティブなセルを画像の下に移動するか
        /// </summary>
        private bool moveActiveCellInImageBelow;
        public bool MoveActiveCellInImageBelow
        {
            get { return moveActiveCellInImageBelow; }
            set { Set(ref moveActiveCellInImageBelow, value); }
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
                    Properties.Settings.Default.Setting.ExcelSetting.MoveActiveCellInImageBelow = MoveActiveCellInImageBelow;

                    Properties.Settings.Default.Save();
                });
            }
        }

        /// <summary>
        /// OKボタンコマンド
        /// </summary>
        private RelayCommand onWindowClosing;
        public RelayCommand OnWindowClosing
        {
            get
            {
                return onWindowClosing = onWindowClosing ?? new RelayCommand(() =>
                {
                    // ウィンドウを閉じるときにViewModalをリセットする
                    LoadApplicationSettings();
                });
            }
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ApplicationSettingViewModel()
        {
            LoadApplicationSettings();
        }

        /// <summary>
        /// アプリケーションの設定を読み込む
        /// </summary>
        private void LoadApplicationSettings()
        {
            IntervalMilliseconds =
                Properties.Settings.Default.Setting.ClipboardMonitorIntervalMilliseconds;
            IsClipAutoConvertibleImage = Properties.Settings.Default.Setting.IsClipAutoConvertibleImage;
            MoveActiveCellInImageBelow =
                Properties.Settings.Default.Setting.ExcelSetting.MoveActiveCellInImageBelow;
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

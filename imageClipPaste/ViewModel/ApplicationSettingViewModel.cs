using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using imageClipPaste.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        #endregion

        #region Commands
        /// <summary>
        /// OKボタンコマンド
        /// </summary>
        private RelayCommand<Window> onOkCommand;
        public RelayCommand<Window> OnOkCommand
        {
            get
            {
                return onOkCommand = onOkCommand ?? new RelayCommand<Window>((window) =>
                {
                    Save();
                    window.Close();
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
            IntervalMilliseconds = Properties.Settings.Default.Setting.ClipboardMonitorInterval.Milliseconds;
        }

        /// <summary>
        /// アプリケーション設定を、Settingsオブジェクトに設定します
        /// </summary>
        public void Save()
        {
            Properties.Settings.Default.Setting.ClipboardMonitorInterval = TimeSpan.FromMilliseconds(IntervalMilliseconds);
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

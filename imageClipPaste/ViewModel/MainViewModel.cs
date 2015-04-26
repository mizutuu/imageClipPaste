using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using imageClipPaste.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace imageClipPaste.ViewModel
{
    /// <summary>
    /// メインウィンドウのViewModel
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Properties
        /// <summary>
        /// クリップボード監視状態
        /// </summary>
        private bool isEnableMonitor;
        public bool IsEnableMonitor
        {
            get { return isEnableMonitor; }
            set { Set(ref isEnableMonitor, value); }
        }

        private BitmapImage capturedImage;
        public BitmapImage CapturedImage
        {
            get { return capturedImage; }
            set { Set(ref capturedImage, value); }
        }
        #endregion

        #region Commands
        /// <summary>
        /// クリップボード監視切り替えコマンド
        /// </summary>
        private RelayCommand onMonitorClipboardCommand;
        public RelayCommand OnMonitorClipboardCommand
        {
            get { return onMonitorClipboardCommand = onMonitorClipboardCommand ?? new RelayCommand(onMonitorClipboard); }
        }
        #endregion

        private IImageWatcher imageWatcher;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainViewModel(IImageWatcher service)
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            imageWatcher = service;
            imageWatcher.Interval = TimeSpan.FromMilliseconds(1000);
            imageWatcher.CapturedNewerImage += imageWatcher_CapturedNewerImage;
        }

        /// <summary>
        /// MVVM Light ToolkitのCleanup
        /// </summary>
        public override void Cleanup()
        {
            // 
            imageWatcher.Stop();

            base.Cleanup();
        }

        /// <summary>
        /// クリップボード監視切り替えを行います
        /// </summary>
        private void onMonitorClipboard()
        {
            if (imageWatcher.IsEnabled)
            {
                imageWatcher.Stop();
            }
            else
            {
                imageWatcher.Start();
            }
            IsEnableMonitor = imageWatcher.IsEnabled;
        }

        /// <summary>
        /// 新しい画像通知イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void imageWatcher_CapturedNewerImage(object sender, CapturedNewerImageEventArgs e)
        {
            CapturedImage = e.newImage;
        }
    }
}
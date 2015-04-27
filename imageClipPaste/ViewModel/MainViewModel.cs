using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using imageClipPaste.Interfaces;
using imageClipPaste.Views.Dialog;
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

        /// <summary>クリップボードの画像を監視するサービス</summary>
        private IImageWatcher _imageWatcher;

        /// <summary>ウィンドウを表示するサービス</summary>
        private IWindowService _windowService;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainViewModel(IImageWatcher service, IWindowService windowService)
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            _imageWatcher = service;
            _imageWatcher.Interval = TimeSpan.FromMilliseconds(1000);
            _imageWatcher.CapturedNewerImage += imageWatcher_CapturedNewerImage;

            _windowService = windowService;
        }

        /// <summary>
        /// MVVM Light ToolkitのCleanup
        /// </summary>
        public override void Cleanup()
        {
            // 
            _imageWatcher.Stop();

            base.Cleanup();
        }

        /// <summary>
        /// クリップボード監視切り替えを行います
        /// </summary>
        private void onMonitorClipboard()
        {
            if (_imageWatcher.IsEnabled)
            {
                _imageWatcher.Stop();
            }
            else
            {
                // 貼り付け不可能の場合は、プロセス選択ダイアログを開き、callbackで処理を開始する。
                // 貼り付け可能の場合は、処理を開始する。
                _windowService.ShowDialog<Views.PasteProcessSelectWindow>(() =>
                {
                    return; 
                    //_imageWatcher.Start();
                });
            }
            IsEnableMonitor = _imageWatcher.IsEnabled;
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
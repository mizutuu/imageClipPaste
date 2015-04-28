using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using imageClipPaste.Enums;
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

        /// <summary>
        /// 貼り付け先種別
        /// </summary>
        private PasteType pasteType;
        public PasteType PasteType
        {
            get { return pasteType; }
            set { Set(ref pasteType, value); }
        }

        /// <summary>
        /// クリップボードから取得した画像
        /// </summary>
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
        private RelayCommand onSwitchClipboardMonitorCommand;
        public RelayCommand OnSwitchClipboardMonitorCommand
        {
            get { return onSwitchClipboardMonitorCommand = onSwitchClipboardMonitorCommand ?? new RelayCommand(onSwitchClipboardMonitor); }
        }

        /// <summary>
        /// アプリケーション設定ウィンドウを開くコマンド
        /// </summary>
        private RelayCommand onOpenSettingCommand;
        public RelayCommand OnOpenSettingCommand
        {
            get
            {
                return onOpenSettingCommand = onOpenSettingCommand ?? new RelayCommand(() =>
                {
                    // アプリケーション設定ウィンドウをモーダルで表示する
                    _windowService.ShowDialog<Views.ApplicationSettingWindow>();

                    // 設定値を画面に反映する
                    PasteType = Properties.Settings.Default.Setting.PasteType;
                }, () => { return !IsEnableMonitor; });
            }
        }
        #endregion

        /// <summary>クリップボードの画像を監視するサービス</summary>
        private IImageMonitor _clipboardMonitorService;

        /// <summary>ウィンドウを表示するサービス</summary>
        private IWindowService _windowService;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainViewModel(IImageMonitor clipboardMonitor, IWindowService windowService)
        {
            // クリップボード監視サービスを初期化する
            _clipboardMonitorService = clipboardMonitor;
            _clipboardMonitorService.Interval = TimeSpan.FromMilliseconds(1000);
            _clipboardMonitorService.CapturedNewerImage += imageWatcher_CapturedNewerImage;

            _windowService = windowService;

            PasteType = Properties.Settings.Default.Setting.PasteType;
        }

        /// <summary>
        /// MVVM Light ToolkitのCleanup
        /// </summary>
        public override void Cleanup()
        {
            // 
            _clipboardMonitorService.Stop();

            base.Cleanup();
        }

        /// <summary>
        /// クリップボード監視切り替えを行います
        /// </summary>
        private void onSwitchClipboardMonitor()
        {
            if (_clipboardMonitorService.IsEnabled)
            {
                _clipboardMonitorService.Stop();
            }
            else
            {
                // 貼り付け実行クラスのインスタンスを取得
                // 
                // 貼り付け不可能？
                // →貼り付け先を選んでもらう
                //
                // 監視を開始する
                

                // 貼り付け不可能の場合は、プロセス選択ダイアログを開き、callbackで処理を開始する。
                // 貼り付け可能の場合は、処理を開始する。
                if (false)
                {
                    _windowService.ShowDialog<Views.PasteProcessSelectWindow>(() =>
                    {

                        return;
                        //_imageWatcher.Start();
                    });
                }

                _clipboardMonitorService.Start();
            }
            IsEnableMonitor = _clipboardMonitorService.IsEnabled;
        }

        /// <summary>
        /// 新しい画像通知イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void imageWatcher_CapturedNewerImage(object sender, CapturedNewerImageEventArgs e)
        {
            CapturedImage = e.newImage;

            // TODO: 貼り付けターゲットに貼り付ける。
        }
    }
}
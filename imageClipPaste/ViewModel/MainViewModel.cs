using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using imageClipPaste.Enums;
using imageClipPaste.Interfaces;
using imageClipPaste.Services;
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
            get
            {
                return onSwitchClipboardMonitorCommand = onSwitchClipboardMonitorCommand ?? new RelayCommand(() =>
                {
                    if (_clipboardMonitorService.IsEnabled)
                    {
                        _clipboardMonitorService.Stop();
                        CleanupImagePaste();
                        PasteType = Enums.PasteType.NoSelect;
                    }
                    else
                    {
                        // 画像の貼り付け先を選択し、選択結果をアプリケーション設定に保持します
                        var dialogResult = 
                            _windowService.ShowDialog<Views.PasteProcessSelectWindow>(() => {
                                ViewModelLocator.CleanupPasteProcessSelect();
                            }).Value;
                        if (!dialogResult)
                            return;

                        PasteType = Properties.Settings.Default.Setting.CurrentPasteProcessInfo.PasteType;

                        // 貼り付け処理を実行するインスタンスを取得し、
                        // クリップボードの監視を開始します
                        _imagePaste = ImagePasteFactory.Create(Properties.Settings.Default.Setting);
                        _clipboardMonitorService.Start();
                    }
                    IsEnableMonitor = _clipboardMonitorService.IsEnabled;
                });
            }
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

                    // 設定値を反映する
                    _clipboardMonitorService.Interval = Properties.Settings.Default.Setting.ClipboardMonitorInterval;
                }, () => { return !IsEnableMonitor; });
            }
        }
        #endregion

        /// <summary>クリップボードの画像を監視するサービス</summary>
        private IImageMonitor _clipboardMonitorService;

        /// <summary>ウィンドウを表示するサービス</summary>
        private IWindowService _windowService;

        /// <summary>画像を貼り付けを行う</summary>
        private IImagePaste _imagePaste;

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
            _imagePaste = null;

            PasteType = Enums.PasteType.NoSelect;
        }

        /// <summary>
        /// MVVM Light ToolkitのCleanup
        /// </summary>
        public override void Cleanup()
        {
            _clipboardMonitorService.Stop();
            _clipboardMonitorService.CapturedNewerImage -= imageWatcher_CapturedNewerImage;
            _clipboardMonitorService.Dispose();
            CleanupImagePaste();
            base.Cleanup();
        }

        /// <summary>
        /// 画像を貼り付けリソースを開放します
        /// </summary>
        public void CleanupImagePaste()
        {
            if (_imagePaste != null)
                _imagePaste.Dispose();
            _imagePaste = null;
        }

        /// <summary>
        /// 新しい画像通知イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void imageWatcher_CapturedNewerImage(object sender, CapturedNewerImageEventArgs e)
        {
            CapturedImage = e.newImage;
            _imagePaste.Paste(e.newImage);

            // 画像が貼り付けられないときは、監視を停止します
            // 貼り付け先のプロセスが無いとき。
            if (!_imagePaste.IsPastable)
                _clipboardMonitorService.Stop();
        }
    }
}
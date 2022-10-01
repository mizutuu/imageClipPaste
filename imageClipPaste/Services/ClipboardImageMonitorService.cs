using imageClipPaste.Interfaces;
using imageClipPaste.Models.Clipboard;
using imageClipPaste.Models.Paste;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace imageClipPaste.Services
{
    /// <summary>
    /// クリップボードを監視して、新しい画像がある場合に通知するサービス
    /// </summary>
    public class ClipboardImageMonitorService : BaseImageMonitorService
    {
        /// <summary>NLog</summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Has Dispose already been called?
        /// </summary>
        bool disposed = false;

        /// <summary>新しく取得した画像を通知するイベントハンドラ</summary>
        public override event EventHandler<CapturedNewerImageEventArgs> CapturedNewerImage;

        /// <summary>
        /// 監視実行タスク
        /// </summary>
        private Thread MonitorExecutor;

        /// <summary>
        /// 監視停止用トークンソース
        /// </summary>
        private CancellationTokenSource MonitorCancelToken;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ClipboardImageMonitorService()
            : base()
        {
            MonitorCancelToken = new CancellationTokenSource();
            MonitorCancelToken.Cancel();
        }

        /// <summary>
        /// 画像の監視を開始します
        /// </summary>
        public override void Start()
        {
            // すでに監視中の場合は何もしない
            if (!MonitorCancelToken.IsCancellationRequested)
            {
                logger.Debug("監視中に監視が開始されました。");
                return;
            }

            logger.Debug("監視タスクを起動します。");
            MonitorCancelToken = new CancellationTokenSource();
            MonitorExecutor = execution(MonitorCancelToken.Token);
            IsEnabled = true;
        }

        /// <summary>
        /// 画像の監視を停止します。
        /// </summary>
        public override void Stop()
        {
            // すでに監視が停止されている場合は何もしない
            if (MonitorCancelToken.IsCancellationRequested)
            {
                logger.Debug("監視停止状態で監視が停止されました。");
                return;
            }

            logger.Debug("監視タスクにキャンセル要求を発行します。");
            using (var token = MonitorCancelToken)
            {
                token.Cancel();

                // タスクの停止を待機する
                MonitorExecutor.Join();
                IsEnabled = false;
            }
            logger.Debug("監視タスクを停止しました。");
        }

        /// <summary>
        /// クリップボードを監視します
        /// </summary>
        /// <param name="cancelToken">監視停止用トークン</param>
        /// <returns>クリップボード監視スレッド</returns>
        private Thread execution(CancellationToken cancelToken)
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    var clipImageManager = new ClipboardImageManager {
                        FilterSameImage = this.FilterSameImage,
                        IsClipAutoConvertibleImage = this.IsClipAutoConvertibleImage
                    };
                    while (!cancelToken.IsCancellationRequested)
                    {
                        logger.Trace("監視周期" + " Total Memory = {0} KB", GC.GetTotalMemory(true) / 1024);

                        BitmapImage image = clipImageManager.GetNewClipboardImage();
                        if (image != null)
                        {
                            logger.Trace("新しい画像をクリップボードから取得しました。");
                            EventHandler<CapturedNewerImageEventArgs> handler = CapturedNewerImage;
                            if (handler != null)
                                handler(this, new CapturedNewerImageEventArgs { newImage = image });
                        }

                        // 指定時間待つ
                        Thread.Sleep(Interval);
                    }

                    logger.Debug("監視タスクが正常に終了しました。");
                }
                catch (PasteProcessNotFoundException ex)
                {
                    logger.Debug(ex);
                    using (var token = MonitorCancelToken)
                    {
                        token.Cancel();
                        IsEnabled = false;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "監視タスク中に例外が発生しました。");
                    throw;
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return thread;
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                MonitorCancelToken.Dispose();
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }
}

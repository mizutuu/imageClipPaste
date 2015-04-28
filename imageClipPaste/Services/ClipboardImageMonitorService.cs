﻿using imageClipPaste.Interfaces;
using imageClipPaste.Services.Clipboard;
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
    public class ClipboardImageMonitorService : IImageMonitor
    {
        /// <summary>NLog</summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>監視状態</summary>
        public bool IsEnabled { get; set; }

        /// <summary>監視間隔</summary>
        public TimeSpan Interval { get; set; }

        /// <summary>クリップボードから取得した画像が前回と同じだった場合にフィルタするかを設定します</summary>
        public bool FilterSameImage { get; set; }

        /// <summary>新しく取得した画像を通知するイベントハンドラ</summary>
        public event EventHandler<CapturedNewerImageEventArgs> CapturedNewerImage;

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
        {
            IsEnabled = false;
            FilterSameImage = true;
            Interval = TimeSpan.FromMilliseconds(1000);
            MonitorCancelToken = new CancellationTokenSource();
            MonitorCancelToken.Cancel();
        }

        /// <summary>
        /// 画像の監視を開始します
        /// </summary>
        public void Start()
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
        public void Stop()
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
                    var clipImageManager = new ClipboardImageManager { FilterSameImage = this.FilterSameImage };
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
                catch (Exception ex)
                {
                    logger.Error("監視タスク中に例外が発生しました。", ex);
                    throw ex;
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return thread;
        }
    }
}
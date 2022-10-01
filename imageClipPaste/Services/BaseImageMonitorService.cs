using imageClipPaste.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageClipPaste.Services
{
    /// <summary>
    /// 画像を監視するベースクラス
    /// </summary>
    public abstract class BaseImageMonitorService : IImageMonitor
    {
        /// <summary>
        /// Has Dispose already been called?
        /// </summary>
        bool disposed = false;

        /// <summary>監視状態</summary>
        public bool IsEnabled { get; set; }

        /// <summary>監視間隔</summary>
        public TimeSpan Interval { get; set; }

        /// <summary>
        /// クリップボードから画像をコピーするときに、自動変換可能な画像をコピーするか
        /// </summary>
        public bool IsClipAutoConvertibleImage { get; set; }

        /// <summary>クリップボードから取得した画像が前回と同じだった場合にフィルタするかを設定します</summary>
        public bool FilterSameImage { get; set; }

        /// <summary>新しく取得した画像を通知するイベントハンドラ</summary>
        public abstract event EventHandler<CapturedNewerImageEventArgs> CapturedNewerImage;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BaseImageMonitorService()
        {
            IsEnabled = false;
            FilterSameImage = true;
            Interval = TimeSpan.FromMilliseconds(1000);
            IsClipAutoConvertibleImage = true;
        }

        /// <summary>
        /// 監視を監視します
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// 監視を停止します
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Public implementation of Dispose pattern callable by consumers.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }
}

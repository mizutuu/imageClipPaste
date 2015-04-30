using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace imageClipPaste.Interfaces
{
    /// <summary>
    /// 定期的に画像を取得し、新しい画像がある場合に、取得した画像を通知するインタフェース
    /// </summary>
    public interface IImageMonitor : IDisposable
    {
        /// <summary>
        /// 監視状態
        ///   true:監視中, false:停止中
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>監視間隔</summary>
        TimeSpan Interval { get; set; }

        /// <summary>画像の監視を開始します</summary>
        void Start();

        /// <summary>画像の監視を停止します。</summary>
        void Stop();

        /// <summary>新しく取得した画像を通知するイベントハンドラ</summary>
        event EventHandler<CapturedNewerImageEventArgs> CapturedNewerImage;
    }

    /// <summary>
    /// 画像を通知するイベントハンドラ引数
    /// </summary>
    public class CapturedNewerImageEventArgs : EventArgs
    {
        /// <summary>新しく取得した画像</summary>
        public BitmapImage newImage { get; set; }
    }
}

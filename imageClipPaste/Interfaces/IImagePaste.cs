using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace imageClipPaste.Interfaces
{
    /// <summary>
    /// 画像を特定のプロセスに貼り付けるインタフェース
    /// </summary>
    public interface IImagePaste
    {
        /// <summary>
        /// 貼り付け可能状態
        /// </summary>
        bool IsPastable { get; }

        /// <summary>
        /// 画像をプロセスに貼り付けます
        /// </summary>
        /// <param name="image">貼り付ける画像</param>
        void Paste(BitmapImage image);
    }
}

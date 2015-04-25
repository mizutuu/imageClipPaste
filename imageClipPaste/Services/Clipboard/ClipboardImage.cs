using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace imageClipPaste.Services.Clipboard
{
    /// <summary>
    /// 
    /// </summary>
    public class ClipboardImage
    {
        /// <summary>画像のピクセル幅</summary>
        private int PixelWidth;

        /// <summary>画像のピクセル高さ</summary>
        private int PixelHeight;

        /// <summary>画像をピクセル配列に変換したもの</summary>
        public byte[] Pixels { set; private get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ClipboardImage()
        {
            PixelWidth = 0;
            PixelHeight = 0;
            Pixels = new byte[0];
        }

        /// <summary>
        /// ピクセル幅と高さを基準にして、同一の画像であるかをチェックします
        /// </summary>
        /// <param name="image">比較対象の画像</param>
        /// <returns>同一の場合はtrueを、異なる場合はfalseを返します</returns>
        public bool EqualsPixelWidthAndHeight(BitmapSource image)
        {
            return ((PixelWidth == image.PixelWidth)
                && (PixelHeight == image.PixelHeight));
        }

        /// <summary>
        /// バイト配列を基準にして、同一の画像であるかをチェックします
        /// </summary>
        /// <param name="imagePixels">比較対象のバイト配列</param>
        /// <returns>同一の場合はtrueを、異なる場合はfalseを返します</returns>
        public bool EqualsPixelBytes(byte[] imagePixels)
        {
            return Pixels.SequenceEqual(imagePixels);
        }

        /// <summary>
        /// 引数の画像を、比較元の画像として設定します
        /// </summary>
        /// <param name="pixelWidth">ピクセル幅</param>
        /// <param name="pixelHeight">ピクセル高さ</param>
        /// <param name="imagePixels">画像のバイト配列</param>
        public void SetImage(int pixelWidth, int pixelHeight, byte[] imagePixels)
        {
            PixelWidth = pixelWidth;
            PixelHeight = pixelHeight;
            Pixels = imagePixels;
        }
    }
}

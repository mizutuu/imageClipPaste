using imageClipPaste.Models.Clipboard;
using NLog;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace imageClipPaste.Models.Clipboard
{
    /// <summary>
    /// クリップボードから取得した画像を管理するクラス
    /// </summary>
    public class ClipboardImageManager
    {
        /// <summary>一つ前に取得した画像の情報</summary>
        private ClipboardImage previouseImage = new ClipboardImage();

        /// <summary>クリップボードから取得した画像が前回と同じだった場合にフィルタするかを設定します</summary>
        public bool FilterSameImage { get; set; }

        /// <summary>
        /// クリップボードから新しい画像を取得します
        /// </summary>
        /// <returns>
        /// クリップボードから新しい画像が取得できない場合はnullを、
        /// 新しい画像が取得できた場合はBitmapImageを返します。
        /// </returns>
        public BitmapImage GetNewClipboardImage()
        {
            if (!System.Windows.Clipboard.ContainsImage())
                return null;

            // クリップボードのデータを取得する
            BitmapSource clipboardImage;
            try
            {
                clipboardImage = System.Windows.Clipboard.GetImage();
            }
            catch (COMException)
            {
                // クリップボードから画像を取得する際に、時々失敗することがある。
                // その時は、次回処理に任せる。
                return null;
            }

            // クリップボードから取得した画像のアルファ値を調整する。
            var clipImageSource = AdjustAGBRBitmapSource(clipboardImage);

            // BitmapSourceからBitmapImageに変換する。
            var clipBitmapImage = ConvertBitmapImage(clipImageSource);

            // 前回と今回のイメージのPixelを比較して、同一の場合はスキップする。
            var clipPixels = ConvertPixels(clipBitmapImage);
            if (FilterSameImage && previouseImage.EqualsPixelBytes(clipPixels))
                return null;

            // 同一画像と比較する用途で保管しておく。
            if (FilterSameImage)
                previouseImage.SetImage(
                    clipboardImage.PixelWidth,
                    clipboardImage.PixelHeight,
                    clipPixels);

            return clipBitmapImage;
        }

        /// <summary>
        /// BitmapImageをバイト配列に変換します。
        /// </summary>
        /// <param name="image">変換対象のBitmapImage</param>
        /// <returns>バイト配列</returns>
        public static byte[] ConvertPixels(BitmapImage image)
        {
            int width = image.PixelWidth;
            int height = image.PixelHeight;
            int stride = width * ((image.Format.BitsPerPixel + 7) / 8);
            byte[] pixels = new byte[height * stride];
            image.CopyPixels(pixels, stride, 0);
            return pixels;
        }

        /// <summary>
        /// Clipboardから取得したBitmapSourceのアルファ値を調整します。
        /// </summary>
        /// <param name="imageSource">Clipboardから取得したBitmapSource</param>
        /// <returns>アルファ値を調整したBitmapSource</returns>
        private static BitmapSource AdjustAGBRBitmapSource(BitmapSource imageSource)
        {
            // WpfのGetImageから取得した画像データは、アルファ値が全て 0に設定されている。
            // Encode/Decodeをかけて正常な画像に変換します。
            var encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageSource));
            using (var stream = new System.IO.MemoryStream())
            {
                encoder.Save(stream);
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                var decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                return decoder.Frames[0];
            }
        }

        /// <summary>
        /// BitmapSourceをBitmapImageに変換します
        /// </summary>
        /// <param name="imageSource">変換元のBitmapSource</param>
        /// <returns>BitmapImage</returns>
        public static BitmapImage ConvertBitmapImage(BitmapSource imageSource)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(imageSource));
                pngEncoder.Save(stream);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.CreateOptions = BitmapCreateOptions.None;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }
    }
}

using imageClipPaste.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace imageClipPaste.Services.Paste
{
    /// <summary>
    /// 画像をPowerPointに貼り付けます
    /// </summary>
    public class ImagePastePowerPoint : IImagePaste, IDisposable
    {   
        /// <summary>
        /// 貼り付け可能状態
        /// </summary>
        public bool IsPastable { get { return IsAlivePasteProcess(); } }

        /// <summary>貼り付け設定</summary>
        private Settings.PastePowerPointSetting _setting;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="setting">貼り付け設定</param>
        public ImagePastePowerPoint(Settings.PastePowerPointSetting setting)
        {
            _setting = setting;
        }

        /// <summary>
        /// 貼り付け先のプロセスが生存しているか取得します
        /// </summary>
        private bool IsAlivePasteProcess()
        {
            // TODO:
            return false;
        }

        /// <summary>
        /// 画像をPowerPointに貼り付けます
        /// </summary>
        /// <param name="image">貼り付ける画像</param>
        public void Paste(BitmapImage image)
        {
            // TODO:
        }

        /// <summary>
        /// アンマネージ リソースを開放します。
        /// </summary>
        public void Dispose()
        {
        }
    }
}

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
    public class ImagePastePowerPoint : BaseImagePaste
    {
        /// <summary>
        /// Has Dispose already been called?
        /// </summary>
        bool disposed = false;

        /// <summary>
        /// 貼り付け可能状態
        /// </summary>
        public override bool IsPastable
        {
            get
            {
                return IsAlivePasteProcess();
            }
        }


        /// <summary>貼り付け設定</summary>
        private Settings.PastePowerPointSetting _setting;

        /// <summary>貼り付け先のプロセス情報</summary>
        private Settings.PasteProcessInfo _process;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="setting">貼り付け設定</param>
        public ImagePastePowerPoint(Settings.PasteProcessInfo process, Settings.PastePowerPointSetting setting)
        {
            _setting = setting;
            _process = process;
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
        public override void Paste(BitmapImage image)
        {
            // TODO:
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
                // TODO:
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }
}

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
    /// 画像を貼り付けを行うベースクラスです
    /// </summary>
    public abstract class BaseImagePaste : IImagePaste, IDisposable
    {
        /// <summary>
        /// Has Dispose already been called?
        /// </summary>
        bool disposed = false;

        /// <summary>
        /// 
        /// </summary>
        public abstract bool IsPastable
        {
            get;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BaseImagePaste()
        {

        }

        /// <summary>
        /// 画像を貼り付ける
        /// </summary>
        /// <param name="image">画像</param>
        public abstract void Paste(BitmapImage image);

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

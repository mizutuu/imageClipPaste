using imageClipPaste.Interfaces;
using imageClipPaste.Models.Office;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using PowerPoint = NetOffice.PowerPointApi;

namespace imageClipPaste.Models.Paste
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

        /// <summary>NLog</summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>貼り付け設定</summary>
        private Settings.PastePowerPointSetting _setting;

        /// <summary>貼り付け先のプロセス情報</summary>
        private Settings.PasteProcessInfo _process;

        /// <summary>PowerPointアプリケーション</summary>
        private PowerPoint.Application _pptApplication;

        /// <summary>PowerPointアプリケーション配下のプレゼンテーション</summary>
        private PowerPoint.Presentation _pptPresentation;

        /// <summary>Excel貼り付けのための一時保存先</summary>
        private string _tempImagePath;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="setting">貼り付け設定</param>
        public ImagePastePowerPoint(Settings.PasteProcessInfo process, Settings.PastePowerPointSetting setting)
        {
            _setting = setting;
            _process = process;
            _pptApplication = GetApplication(process);
            _pptPresentation = GetPresentation(_pptApplication, process);
            _tempImagePath = Path.GetTempFileName();
        }

        /// <summary>
        /// 貼り付け先のプロセスが生存しているか取得します
        /// </summary>
        private bool IsAlivePasteProcess()
        {
            if (_pptApplication == null || _pptPresentation == null)
            {
                _logger.Debug("プロセス生存確認 Application: {0}, Presentation: {1}",
                    _pptApplication == null, _pptPresentation == null);
                return false;
            }

            // 見た限り、Applicationになさそうだったので、
            // プロセスが無いときにRPCエラーとなることを利用しています。
            try
            {
                var dummy = _pptApplication.Visible;
                dummy = _pptPresentation.ReadOnly;
            }
            catch (COMException ex)
            {
                _logger.Debug("プロセス生存確認", ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// PowerPointアプリケーションを取得します
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        private PowerPoint.Application GetApplication(Settings.PasteProcessInfo process)
        {
            if (process.IsRequiredNew)
            {
                return PowerPointModel.GetApplication();
            }
            else
            {
                return PowerPointModel.FindApplication(process);
            }
        }

        /// <summary>
        /// PowerPointプレゼンテーションを取得します
        /// </summary>
        /// <param name="app"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        private PowerPoint.Presentation GetPresentation(PowerPoint.Application app, Settings.PasteProcessInfo process)
        {
            if (process.IsRequiredNew)
            {
                // プロセスを新規立ち上げしたときには、貼り付け先のプレゼンテーションが無いので作成しておく
                return app.Presentations.Add();
            }
            else
            {
                return PowerPointModel.FindPresentation(app, process);
            }
        }

        /// <summary>
        /// 画像をPowerPointに貼り付けます
        /// </summary>
        /// <param name="image">貼り付ける画像</param>
        public override void Paste(BitmapImage image)
        {
            if (!IsPastable)
                throw new PasteProcessNotFoundException(_process.ToString());

            // 貼り付け準備
            SavePngFile(image, _tempImagePath);

            /*
             * TODO: 追加するスライドのビュータイプを、設定ウィンドウから設定できるようにするとなお便利。
             *       →OfficeのVersionに依存するところがあるので難しいかも。
             */
            // 貼り付け先のスライドを追加してから、画像を貼り付ける
            using (var slides = _pptPresentation.Slides)
            using (var slide = slides.Add(slides.Count + 1, PowerPoint.Enums.PpSlideLayout.ppLayoutBlank))
            {
                // アクティブなスライド番号と、スライドのサイズを取得する
                float slideWidth, slideHeight;
                using (var master = slide.Master)
                {
                    slideWidth = master.Width;
                    slideHeight = master.Height;
                }

                // 画像の貼り付け＆センタリング
                using (var shape = PowerPointModel.AddShapeFromImageFile(slide, _tempImagePath))
                {
                    _logger.Trace("{0}.{1} に貼り付けました。File: {2}", _pptPresentation.Name, slide.Name, _tempImagePath);

                    // 貼り付けた画像が中央に表示されるよう調整
                    shape.Left = (slideWidth - shape.Width) / 2;
                    shape.Top = (slideHeight - shape.Height) / 2;
                }

                // 画像を貼り付けたスライドをアクティブ化する
                slide.Select();


                slides
                    .ToList().Cast<PowerPoint.Slide>()
                    .ToList()
                    .ForEach((s) => s.Dispose());
            }
        }

        /// <summary>
        /// Png形式でBitmapImageをファイルに保存します
        /// </summary>
        /// <param name="image">保存するBitmapImage</param>
        /// <param name="path">保存先</param>
        protected static void SavePngFile(BitmapImage image, string path)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                var encode = new PngBitmapEncoder();
                encode.Frames.Add(BitmapFrame.Create(image));
                encode.Save(stream);
            }
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
                if (_pptPresentation != null)
                {
                    _pptPresentation.Dispose();
                    _pptPresentation = null;
                }
                if (_pptApplication != null)
                {
                    _pptApplication.Dispose();
                    _pptApplication = null;
                }
                if (File.Exists(_tempImagePath))
                {
                    File.Delete(_tempImagePath);
                }
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }
}

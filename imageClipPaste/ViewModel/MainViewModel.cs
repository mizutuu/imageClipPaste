using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using imageClipPaste.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace imageClipPaste.ViewModel
{
    /// <summary>
    /// ���C���E�B���h�E��ViewModel
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Properties
        /// <summary>
        /// �N���b�v�{�[�h�Ď����
        /// </summary>
        private bool isEnableMonitor;
        public bool IsEnableMonitor
        {
            get { return isEnableMonitor; }
            set { Set(ref isEnableMonitor, value); }
        }

        private BitmapImage capturedImage;
        public BitmapImage CapturedImage
        {
            get { return capturedImage; }
            set { Set(ref capturedImage, value); }
        }
        #endregion

        #region Commands
        /// <summary>
        /// �N���b�v�{�[�h�Ď��؂�ւ��R�}���h
        /// </summary>
        private RelayCommand onMonitorClipboardCommand;
        public RelayCommand OnMonitorClipboardCommand
        {
            get { return onMonitorClipboardCommand = onMonitorClipboardCommand ?? new RelayCommand(onMonitorClipboard); }
        }
        #endregion

        private IImageWatcher imageWatcher;

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public MainViewModel(IImageWatcher service)
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            imageWatcher = service;
            imageWatcher.Interval = TimeSpan.FromMilliseconds(1000);
            imageWatcher.CapturedNewerImage += imageWatcher_CapturedNewerImage;
        }

        /// <summary>
        /// MVVM Light Toolkit��Cleanup
        /// </summary>
        public override void Cleanup()
        {
            // 
            imageWatcher.Stop();

            base.Cleanup();
        }

        /// <summary>
        /// �N���b�v�{�[�h�Ď��؂�ւ����s���܂�
        /// </summary>
        private void onMonitorClipboard()
        {
            if (imageWatcher.IsEnabled)
            {
                imageWatcher.Stop();
            }
            else
            {
                imageWatcher.Start();
            }
            IsEnableMonitor = imageWatcher.IsEnabled;
        }

        /// <summary>
        /// �V�����摜�ʒm�C�x���g�n���h��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void imageWatcher_CapturedNewerImage(object sender, CapturedNewerImageEventArgs e)
        {
            CapturedImage = e.newImage;
        }
    }
}
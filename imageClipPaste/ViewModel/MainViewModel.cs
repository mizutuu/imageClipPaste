using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using imageClipPaste.Interfaces;
using imageClipPaste.Views.Dialog;
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

        /// <summary>�N���b�v�{�[�h�̉摜���Ď�����T�[�r�X</summary>
        private IImageWatcher _imageWatcher;

        /// <summary>�E�B���h�E��\������T�[�r�X</summary>
        private IWindowService _windowService;

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public MainViewModel(IImageWatcher service, IWindowService windowService)
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            _imageWatcher = service;
            _imageWatcher.Interval = TimeSpan.FromMilliseconds(1000);
            _imageWatcher.CapturedNewerImage += imageWatcher_CapturedNewerImage;

            _windowService = windowService;
        }

        /// <summary>
        /// MVVM Light Toolkit��Cleanup
        /// </summary>
        public override void Cleanup()
        {
            // 
            _imageWatcher.Stop();

            base.Cleanup();
        }

        /// <summary>
        /// �N���b�v�{�[�h�Ď��؂�ւ����s���܂�
        /// </summary>
        private void onMonitorClipboard()
        {
            if (_imageWatcher.IsEnabled)
            {
                _imageWatcher.Stop();
            }
            else
            {
                // �\��t���s�\�̏ꍇ�́A�v���Z�X�I���_�C�A���O���J���Acallback�ŏ������J�n����B
                // �\��t���\�̏ꍇ�́A�������J�n����B
                _windowService.ShowDialog<Views.PasteProcessSelectWindow>(() =>
                {
                    return; 
                    //_imageWatcher.Start();
                });
            }
            IsEnableMonitor = _imageWatcher.IsEnabled;
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
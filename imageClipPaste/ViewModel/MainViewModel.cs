using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using imageClipPaste.Enums;
using imageClipPaste.Interfaces;
using imageClipPaste.Services;
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

        /// <summary>
        /// �\��t������
        /// </summary>
        private PasteType pasteType;
        public PasteType PasteType
        {
            get { return pasteType; }
            set { Set(ref pasteType, value); }
        }

        /// <summary>
        /// �N���b�v�{�[�h����擾�����摜
        /// </summary>
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
        private RelayCommand onSwitchClipboardMonitorCommand;
        public RelayCommand OnSwitchClipboardMonitorCommand
        {
            get
            {
                return onSwitchClipboardMonitorCommand = onSwitchClipboardMonitorCommand ?? new RelayCommand(() =>
                {
                    if (_clipboardMonitorService.IsEnabled)
                    {
                        _clipboardMonitorService.Stop();
                        CleanupImagePaste();
                        PasteType = Enums.PasteType.NoSelect;
                    }
                    else
                    {
                        // �摜�̓\��t�����I�����A�I�����ʂ��A�v���P�[�V�����ݒ�ɕێ����܂�
                        var dialogResult = 
                            _windowService.ShowDialog<Views.PasteProcessSelectWindow>(() => {
                                ViewModelLocator.CleanupPasteProcessSelect();
                            }).Value;
                        if (!dialogResult)
                            return;

                        PasteType = Properties.Settings.Default.Setting.CurrentPasteProcessInfo.PasteType;

                        // �\��t�����������s����C���X�^���X���擾���A
                        // �N���b�v�{�[�h�̊Ď����J�n���܂�
                        _imagePaste = ImagePasteFactory.Create(Properties.Settings.Default.Setting);
                        _clipboardMonitorService.Start();
                    }
                    IsEnableMonitor = _clipboardMonitorService.IsEnabled;
                });
            }
        }

        /// <summary>
        /// �A�v���P�[�V�����ݒ�E�B���h�E���J���R�}���h
        /// </summary>
        private RelayCommand onOpenSettingCommand;
        public RelayCommand OnOpenSettingCommand
        {
            get
            {
                return onOpenSettingCommand = onOpenSettingCommand ?? new RelayCommand(() =>
                {
                    // �A�v���P�[�V�����ݒ�E�B���h�E�����[�_���ŕ\������
                    _windowService.ShowDialog<Views.ApplicationSettingWindow>();

                    // �ݒ�l�𔽉f����
                    _clipboardMonitorService.Interval = Properties.Settings.Default.Setting.ClipboardMonitorInterval;
                }, () => { return !IsEnableMonitor; });
            }
        }
        #endregion

        /// <summary>�N���b�v�{�[�h�̉摜���Ď�����T�[�r�X</summary>
        private IImageMonitor _clipboardMonitorService;

        /// <summary>�E�B���h�E��\������T�[�r�X</summary>
        private IWindowService _windowService;

        /// <summary>�摜��\��t�����s��</summary>
        private IImagePaste _imagePaste;

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public MainViewModel(IImageMonitor clipboardMonitor, IWindowService windowService)
        {
            // �N���b�v�{�[�h�Ď��T�[�r�X������������
            _clipboardMonitorService = clipboardMonitor;
            _clipboardMonitorService.Interval = TimeSpan.FromMilliseconds(1000);
            _clipboardMonitorService.CapturedNewerImage += imageWatcher_CapturedNewerImage;

            _windowService = windowService;
            _imagePaste = null;

            PasteType = Enums.PasteType.NoSelect;
        }

        /// <summary>
        /// MVVM Light Toolkit��Cleanup
        /// </summary>
        public override void Cleanup()
        {
            _clipboardMonitorService.Stop();
            _clipboardMonitorService.CapturedNewerImage -= imageWatcher_CapturedNewerImage;
            _clipboardMonitorService.Dispose();
            CleanupImagePaste();
            base.Cleanup();
        }

        /// <summary>
        /// �摜��\��t�����\�[�X���J�����܂�
        /// </summary>
        public void CleanupImagePaste()
        {
            if (_imagePaste != null)
                _imagePaste.Dispose();
            _imagePaste = null;
        }

        /// <summary>
        /// �V�����摜�ʒm�C�x���g�n���h��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void imageWatcher_CapturedNewerImage(object sender, CapturedNewerImageEventArgs e)
        {
            CapturedImage = e.newImage;
            _imagePaste.Paste(e.newImage);

            // �摜���\��t�����Ȃ��Ƃ��́A�Ď����~���܂�
            // �\��t����̃v���Z�X�������Ƃ��B
            if (!_imagePaste.IsPastable)
                _clipboardMonitorService.Stop();
        }
    }
}
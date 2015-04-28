using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using imageClipPaste.Enums;
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
            get { return onSwitchClipboardMonitorCommand = onSwitchClipboardMonitorCommand ?? new RelayCommand(onSwitchClipboardMonitor); }
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

                    // �ݒ�l����ʂɔ��f����
                    PasteType = Properties.Settings.Default.Setting.PasteType;
                }, () => { return !IsEnableMonitor; });
            }
        }
        #endregion

        /// <summary>�N���b�v�{�[�h�̉摜���Ď�����T�[�r�X</summary>
        private IImageMonitor _clipboardMonitorService;

        /// <summary>�E�B���h�E��\������T�[�r�X</summary>
        private IWindowService _windowService;

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

            PasteType = Properties.Settings.Default.Setting.PasteType;
        }

        /// <summary>
        /// MVVM Light Toolkit��Cleanup
        /// </summary>
        public override void Cleanup()
        {
            // 
            _clipboardMonitorService.Stop();

            base.Cleanup();
        }

        /// <summary>
        /// �N���b�v�{�[�h�Ď��؂�ւ����s���܂�
        /// </summary>
        private void onSwitchClipboardMonitor()
        {
            if (_clipboardMonitorService.IsEnabled)
            {
                _clipboardMonitorService.Stop();
            }
            else
            {
                // �\��t�����s�N���X�̃C���X�^���X���擾
                // 
                // �\��t���s�\�H
                // ���\��t�����I��ł��炤
                //
                // �Ď����J�n����
                

                // �\��t���s�\�̏ꍇ�́A�v���Z�X�I���_�C�A���O���J���Acallback�ŏ������J�n����B
                // �\��t���\�̏ꍇ�́A�������J�n����B
                if (false)
                {
                    _windowService.ShowDialog<Views.PasteProcessSelectWindow>(() =>
                    {

                        return;
                        //_imageWatcher.Start();
                    });
                }

                _clipboardMonitorService.Start();
            }
            IsEnableMonitor = _clipboardMonitorService.IsEnabled;
        }

        /// <summary>
        /// �V�����摜�ʒm�C�x���g�n���h��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void imageWatcher_CapturedNewerImage(object sender, CapturedNewerImageEventArgs e)
        {
            CapturedImage = e.newImage;

            // TODO: �\��t���^�[�Q�b�g�ɓ\��t����B
        }
    }
}
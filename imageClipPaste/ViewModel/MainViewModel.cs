using GalaSoft.MvvmLight.Command;
using imageClipPaste.Enums;
using imageClipPaste.Interfaces;
using imageClipPaste.Models.Office;
using imageClipPaste.Services;
using imageClipPaste.Views.Dialog;
using System;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

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
                return onSwitchClipboardMonitorCommand = onSwitchClipboardMonitorCommand ?? new RelayCommand(
                    SwitchClipboardMonitor,
                    () => ExcelModel.IsInstalledExcel());
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
                    var interval = TimeSpan.FromMilliseconds(
                        Properties.Settings.Default.Setting.ClipboardMonitorIntervalMilliseconds);
                    _clipboardMonitorService.Interval = interval;
                    _processMonitorTimer.Interval = interval;
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

        /// <summary>�\��t����v���Z�X�̐������m�F����^�C�}�[</summary>
        private DispatcherTimer _processMonitorTimer;

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public MainViewModel(IImageMonitor clipboardMonitor, IWindowService windowService)
        {
            var interval = TimeSpan.FromMilliseconds(
                Properties.Settings.Default.Setting.ClipboardMonitorIntervalMilliseconds);

            // �N���b�v�{�[�h�Ď��T�[�r�X������������
            _clipboardMonitorService = clipboardMonitor;
            _clipboardMonitorService.Interval = interval;
            _clipboardMonitorService.CapturedNewerImage += imageWatcher_CapturedNewerImage;

            // �\��t����v���Z�X�̐����m�F�^�C�}�[������������
            _processMonitorTimer = new DispatcherTimer();
            _processMonitorTimer.Interval = interval;
            _processMonitorTimer.Tick += _processMonitorTimer_Tick;

            _windowService = windowService;
            _imagePaste = null;

            PasteType = PasteType.NoSelect;
        }

        /// <summary>
        /// MVVM Light Toolkit��Cleanup
        /// </summary>
        public override void Cleanup()
        {
            _processMonitorTimer.Stop();
            _processMonitorTimer.Tick -= _processMonitorTimer_Tick;

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
        }

        /// <summary>
        /// �\��t����v���Z�X�����m�F�^�C�}�[�C�x���g
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _processMonitorTimer_Tick(object sender, EventArgs e)
        {
            // �摜���\��t���s�̂Ƃ��́A�Ď����~���܂�
            if (!_imagePaste.IsPastable)
                SwitchClipboardMonitor();
        }

        /// <summary>
        /// �N���b�v�{�[�h�̊Ď���؂�ւ��܂�
        /// </summary>
        private void SwitchClipboardMonitor()
        {
            if (_clipboardMonitorService.IsEnabled)
            {
                _processMonitorTimer.Stop();
                _clipboardMonitorService.Stop();
                CleanupImagePaste();
                PasteType = PasteType.NoSelect;
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
                _processMonitorTimer.Start();
                _clipboardMonitorService.Start();
            }
            IsEnableMonitor = _clipboardMonitorService.IsEnabled;
        }
    }
}
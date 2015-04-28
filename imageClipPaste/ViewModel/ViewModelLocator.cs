/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:imageClipPaste"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using imageClipPaste.Interfaces;
using imageClipPaste.Services;
using imageClipPaste.Views.Dialog;
using Microsoft.Practices.ServiceLocation;

namespace imageClipPaste.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            // �N���b�v�{�[�h�Ď��p�T�[�r�X��o�^
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // TODO: �e�X�g�p�X�^�u�T�[�r�X�̓o�^
            }
            else
            {
                SimpleIoc.Default.Register<IImageWatcher, ClipboardImageWatchService>();
            }

            // �_�C�A���O�\���T�[�r�X��o�^
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // TODO: �e�X�g�p�X�^�u�T�[�r�X�̓o�^
            }
            else
            {
                SimpleIoc.Default.Register<IWindowService, ModalDialogService>();
            }
        }

        /// <summary>
        /// ���C���E�B���h�E��ViewModel�̃C���X�^���X���擾���܂�
        /// </summary>
        public MainViewModel Main
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<MainViewModel>())
                    SimpleIoc.Default.Register<MainViewModel>();

                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// �摜�\��t����v���Z�X�I���E�B���h�E��ViewModel�̃C���X�^���X���擾���܂�
        /// </summary>
        public PasteProcessSelectViewModel PasteProcessSelect
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PasteProcessSelectViewModel>())
                    SimpleIoc.Default.Register<PasteProcessSelectViewModel>();

                return ServiceLocator.Current.GetInstance<PasteProcessSelectViewModel>();
            }
        }

        /// <summary>
        /// �A�v���P�[�V�����ݒ�E�B���h�E��ViewModel�̃C���X�^���X���擾���܂�
        /// </summary>
        public ApplicationSettingViewModel ApplicationSetting
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<ApplicationSettingViewModel>())
                    SimpleIoc.Default.Register<ApplicationSettingViewModel>();

                return ServiceLocator.Current.GetInstance<ApplicationSettingViewModel>();
            }
        }
        
        /// <summary>
        /// �A�v���P�[�V�����ɕR�Â�ViewModel��j�����܂�
        /// </summary>
        public static void Cleanup()
        {

            // �T�[�r�X�̓o�^���������܂��B
            SimpleIoc.Default.Unregister<ClipboardImageWatchService>();
            SimpleIoc.Default.Unregister<ModalDialogService>();

            // ViewModel�̓o�^���������܂�
            CleanupPasteProcessSelect();
            CleanupApplicationSetting();

            SimpleIoc.Default.GetInstance<MainViewModel>().Cleanup();
            SimpleIoc.Default.Unregister<MainViewModel>();
        }

        /// <summary>
        /// �摜�\��t����v���Z�X�I���E�B���h�E��ViewModel�̃C���X�^���X��o�^�������܂�
        /// </summary>
        public static void CleanupPasteProcessSelect()
        {
            if (SimpleIoc.Default.IsRegistered<PasteProcessSelectViewModel>())
            {
                SimpleIoc.Default.GetInstance<PasteProcessSelectViewModel>().Cleanup();
                SimpleIoc.Default.Unregister<PasteProcessSelectViewModel>();
            }
        }

        /// <summary>
        /// �A�v���P�[�V�����ݒ�E�B���h�E��ViewModel�̃C���X�^���X��o�^�������܂�
        /// </summary>
        public static void CleanupApplicationSetting()
        {
            if (SimpleIoc.Default.IsRegistered<ApplicationSettingViewModel>())
            {
                SimpleIoc.Default.GetInstance<ApplicationSettingViewModel>().Cleanup();
                SimpleIoc.Default.Unregister<ApplicationSettingViewModel>();
            }
        }
    }
}
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
using CommonServiceLocator;

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

            // クリップボード監視用サービスを登録
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // design time view services
            }
            else
            {
                SimpleIoc.Default.Register<IImageMonitor, ClipboardImageMonitorService>();
            }

            // ダイアログ表示サービスを登録
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // design time view services
            }
            else
            {
                SimpleIoc.Default.Register<IWindowService, ModalDialogService>();
            }
        }

        /// <summary>
        /// メインウィンドウのViewModelのインスタンスを取得します
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
        /// 画像貼り付け先プロセス選択ウィンドウのViewModelのインスタンスを取得します
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
        /// アプリケーション設定ウィンドウのViewModelのインスタンスを取得します
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
        /// アプリケーションに紐づくViewModelを破棄します
        /// </summary>
        public static void Cleanup()
        {
            // サービスの登録を解除します。
            SimpleIoc.Default.Unregister<ClipboardImageMonitorService>();
            SimpleIoc.Default.Unregister<ModalDialogService>();

            // ViewModelの登録を解除します
            CleanupPasteProcessSelect();
            CleanupApplicationSetting();

            SimpleIoc.Default.GetInstance<MainViewModel>().Cleanup();
            SimpleIoc.Default.Unregister<MainViewModel>();
        }

        /// <summary>
        /// 画像貼り付け先プロセス選択ウィンドウのViewModelのインスタンスを登録解除します
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
        /// アプリケーション設定ウィンドウのViewModelのインスタンスを登録解除します
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
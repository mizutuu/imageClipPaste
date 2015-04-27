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

            // クリップボード監視用サービスを登録
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // TODO: テスト用スタブサービスの登録
            }
            else
            {
                SimpleIoc.Default.Register<IImageWatcher, ClipboardImageWatchService>();
            }

            // ダイアログ表示サービスを登録
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // TODO: テスト用スタブサービスの登録
            }
            else
            {
                SimpleIoc.Default.Register<IWindowService, ProcessSelectDialogService>();
            }

        }

        public MainViewModel Main
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<MainViewModel>())
                    SimpleIoc.Default.Register<MainViewModel>();

                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public PasteProcessSelectViewModel PasteProcessSelect
        {
            get
            {
                if (!SimpleIoc.Default.ContainsCreated<PasteProcessSelectViewModel>())
                    SimpleIoc.Default.Register<PasteProcessSelectViewModel>();

                return ServiceLocator.Current.GetInstance<PasteProcessSelectViewModel>();
            }
        }
        
        public static void Cleanup()
        {

            // サービスの登録を解除します。
            SimpleIoc.Default.Unregister<ClipboardImageWatchService>();
            SimpleIoc.Default.Unregister<ProcessSelectDialogService>();

            // TODO: Clear the ViewModels
        }
    }
}
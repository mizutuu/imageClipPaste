using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace imageClipPaste
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        /// <summary>NLog</summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 未処理例外を捕捉する、イベントハンドラを登録します。
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // アプリケーション全体に紐づくViewModelを登録解除します
            ViewModel.ViewModelLocator.Cleanup();

            // 未処理例外イベントハンドラを解除します。
            this.DispatcherUnhandledException -= App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// WPF UIスレッド上で、未処理例外が発生したときに通知されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            ReportException(e.Exception);
        }

        /// <summary>
        /// WPF UIスレッド以外で、未処理例外が発生したときに通知されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ReportException(e.ExceptionObject as Exception);
        }

        /// <summary>
        /// 未処理例外が発生したときに、通知します。
        /// </summary>
        /// <param name="e"></param>
        private void ReportException(Exception e)
        {
            // ログに出力します。
            logger.Error(e);

            // ユーザにエラーを通知します。
            MessageBox.Show("致命的な問題が発生しました。" + Environment.NewLine +
                "（" + e.Message + "）", AppDomain.CurrentDomain.FriendlyName,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}

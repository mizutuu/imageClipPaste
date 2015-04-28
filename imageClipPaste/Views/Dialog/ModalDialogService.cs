using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace imageClipPaste.Views.Dialog
{
    /// <summary>
    /// 貼り付け先の選択ダイアログサービス
    /// </summary>
    public class ModalDialogService : IWindowService
    {
        /// <summary>
        /// ダイアログを表示する
        /// </summary>
        /// <typeparam name="T">表示するウィンドウクラス</typeparam>
        /// <param name="afterHideCallback">非表示とした後に実行するAction</param>
        public bool? ShowDialog<T>(Action afterHideCallback)
            where T : Window, new()
        {
            bool? result;
            var w = GetCurrentWindow();
            result = new T
            {
                Owner = w
            }.ShowDialog();

            if (afterHideCallback != null)
                afterHideCallback.Invoke();

            return result;
        }

        /// <summary>
        /// ダイアログを表示する
        /// </summary>
        /// <typeparam name="T">表示するウィンドウクラス</typeparam>
        public bool? ShowDialog<T>()
            where T : Window, new()
        {
            return this.ShowDialog<T>(null);
        }

        /// <summary>
        /// 現在表示状態となっているWindowを取得します（ここで取得していいのか…。。。）
        /// </summary>
        /// <returns></returns>
        private Window GetCurrentWindow()
        {
            // 現在表示されているWindowは、Current.Windowsの一番下にあるはず。
            return App.Current.Windows.Cast<Window>().Last();
        }
    }
}

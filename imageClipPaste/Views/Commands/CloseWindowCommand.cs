using System;
using System.Windows;

namespace imageClipPaste.Views.Commands
{
    /// <summary>
    /// ウィンドウを閉じるコマンド
    /// </summary>
    public class CloseWindowCommand : CommandExtension
    {
        /// <summary>
        /// コマンド実行時に呼び出されます。
        /// </summary>
        /// <param name="parameter">Windowオブジェクト</param>
        public override void Execute(object parameter)
        {
            Window w = parameter as Window;
            if (w == null)
                throw new ArgumentNullException(
                    "parameter",
                    "XAMLのCommandParameterに、Windowインスタンスを指定して下さい。");

            // コマンド発行元ウィンドウのクローズを実行します。
            // window.Close() -> Closing -> Closedとイベントが流れていきます。
            w.Close();
        }
    }
}

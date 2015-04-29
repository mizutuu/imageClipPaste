using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageClipPaste.Views.Commands
{
    /// <summary>
    /// キャンセルボタンを伴ったウィンドウを閉じるコマンド
    /// </summary>
    public class CloseCancelWindowCommand : CloseWindowCommand
    {
        /// <summary>
        /// コマンド実行時に呼び出されます。
        /// </summary>
        /// <param name="parameter">Windowオブジェクト</param>
        public override void Execute(object parameter)
        {
            CloseWindow(parameter, false);
        }
    }
}

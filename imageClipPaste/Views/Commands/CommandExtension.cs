using System;
using System.Windows.Input;

namespace imageClipPaste.Views.Commands
{
    /// <summary>
    /// XAMLから直接実行するコマンドの基盤クラスです。
    /// </summary>
    public abstract class CommandExtension : ICommand
    {
        /// <summary>
        /// 現在の状態で、このコマンドが実行できるかを判定します。
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// RaiseCanExecuteChanged が呼び出されたときに生成されます。
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// 現在のコマンドターゲットに対して、コマンドを実行します。
        /// </summary>
        /// <param name="parameter"></param>
        public abstract void Execute(object parameter);
    }
}

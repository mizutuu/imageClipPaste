using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace imageClipPaste.Views.Dialog
{
    public interface IWindowService
    {
        void ShowDialog<T>(Action afterHideCallback)
            where T : Window, new();

        void ShowDialog<T>()
            where T : Window, new();
    }
}

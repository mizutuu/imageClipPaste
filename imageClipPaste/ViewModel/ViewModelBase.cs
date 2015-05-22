using GalaSoft.MvvmLight;
using System.Runtime.CompilerServices;

namespace imageClipPaste.ViewModel
{
    /// <summary>
    /// MvvmLightのラッパークラスです。
    /// .NET Framework4で必要ですが、4.5.1以降では CallerMemberName がサポートされるので不要です。
    /// </summary>
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase, ICleanup
    {
        protected bool Set<T>(ref T field, T newValue = default(T), bool broadcast = false, [CallerMemberName] string propertyName = null)
        {
            return base.Set(propertyName, ref field, newValue, broadcast);
        }
    }
}

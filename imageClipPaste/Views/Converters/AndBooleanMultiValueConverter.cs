using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace imageClipPaste.Views.Converters
{
    /// <summary>
    /// 複数のBooleanのANDをとるコンバータ
    /// </summary>
    public class AndBooleanMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = true;
            foreach (object o in values)
            {
                bool? value = o as bool?;
                if (!value.HasValue)
                    throw new InvalidOperationException("bool型のみ許容されます。");

                result &= value.Value;
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

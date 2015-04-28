using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace imageClipPaste.Views.Converters
{
    /// <summary>
    /// EnumのDescriptionを表示値に変換するコンバータ
    /// </summary>
    public class EnumDescriptionConverter : IValueConverter
    {
        /// <summary>
        /// EnumのDescriptionアノテーション値を取得します。
        /// </summary>
        /// <param name="enumObj"></param>
        /// <returns></returns>
        private string GetEnumDescription(Enum enumObj)
        {
            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());
            object[] attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)
            {
                return enumObj.ToString();
            }
            else
            {
                DescriptionAttribute attrib = attribArray[0] as DescriptionAttribute;
                return attrib.Description;
            }
        }

        /// <summary>
        /// 値を変換します。
        /// </summary>
        /// <param name="value">バインディング ソースによって生成された値。</param>
        /// <param name="targetType">バインディング ターゲット プロパティの型。</param>
        /// <param name="parameter">使用するコンバーター パラメーター。</param>
        /// <param name="culture">コンバーターで使用するカルチャ。</param>
        /// <returns>変換された値。</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic)
                return "dummy";

            Enum e = (Enum)value;
            return GetEnumDescription(e);
        }

        /// <summary>
        /// 値を変換します。
        /// バインディング ターゲットの値をバインディング ソースに反映するときにこのメソッドを呼び出します。
        /// </summary>
        /// <param name="value">バインディング ターゲットによって生成される値。</param>
        /// <param name="targetType">変換後の型。</param>
        /// <param name="parameter">使用するコンバーター パラメーター。</param>
        /// <param name="culture">コンバーターで使用するカルチャ。</param>
        /// <returns>変換された値。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Empty;
        }
    }
}

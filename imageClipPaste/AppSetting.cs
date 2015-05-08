using System;
using System.ComponentModel;
using System.Configuration;

namespace imageClipPaste
{
    /// <summary>
    /// App.configファイルの、
    /// <appSettings></appSettings>セクションに記載された設定値を取得します
    /// </summary>
    public static class AppSetting
    {
        public static bool IsEnableAnimation
        {
            get
            {
                var value = ConfigurationManager.AppSettings["IsEnableAnimation"];
                if (String.IsNullOrWhiteSpace(value))
                    return true;

                return Convert.ToBoolean(value);
            }
        }
    }
}

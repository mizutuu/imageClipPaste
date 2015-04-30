using imageClipPaste.Enums;
using imageClipPaste.Interfaces;
using imageClipPaste.Services.Paste;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageClipPaste.Services
{
    /// <summary>
    /// 画像を貼り付けるインスタンスを作るFactoryクラス
    /// </summary>
    public class ImagePasteFactory
    {
        /// <summary>
        /// 画像を貼り付けるインスタンスを作成します
        /// </summary>
        /// <param name="pasteType">貼り付け対象の種別</param>
        /// <returns></returns>
        public static IImagePaste Create(Settings.ImageClipPasteSetting setting)
        {
            switch (setting.CurrentPasteProcessInfo.PasteType)
            {
                case PasteType.Excel:
                    return new ImagePasteExcel(setting.CurrentPasteProcessInfo, setting.ExcelSetting);
                case PasteType.PowerPoint:
                    return new ImagePastePowerPoint(setting.CurrentPasteProcessInfo, setting.PowerPointSetting);
                default:
                    throw new NotImplementedException("not supported.");
            }
        }
    }
}

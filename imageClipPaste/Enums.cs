using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageClipPaste.Enums
{
    /// <summary>
    /// 貼り付け先の種別
    /// </summary>
    public enum PasteType
    {
        [Description("Excel")]
        Excel,

        [Description("PowerPoint")]
        PowerPoint
    }
}

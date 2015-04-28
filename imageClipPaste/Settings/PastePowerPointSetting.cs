using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageClipPaste.Settings
{
    /// <summary>
    /// PowerPointに貼り付けるときの設定です
    /// </summary>
    [Serializable]
    public class PastePowerPointSetting
    {
        /// <summary>貼り付け先情報</summary>
        public PasteProcessInfo ProcessInfo { get; set; }
    }
}

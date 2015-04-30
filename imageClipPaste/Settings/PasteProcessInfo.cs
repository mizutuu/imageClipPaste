using imageClipPaste.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageClipPaste.Settings
{
    /// <summary>
    /// 貼り付け先のプロセス情報
    /// </summary>
    [Serializable]
    public class PasteProcessInfo
    {
        /// <summary>
        /// Excelの場合はワークブック名
        /// PowerPointの場合は
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Excelの場合はブックのパス、未保存の場合はnull
        /// PowerPointの場合は
        /// </summary>
        public String Path { get; set; }

        /// <summary>
        /// インスタンスハンドル
        /// </summary>
        public int HInstance { get; set; }

        /// <summary>
        /// ウィンドウハンドル
        /// </summary>
        public int HWnd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PasteType PasteType { get; set; }

        /// <summary>
        /// 新しいワークブック又は
        /// </summary>
        public bool IsRequiredNew { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PasteProcessInfo()
        {
            IsRequiredNew = false;
        }
    }
}

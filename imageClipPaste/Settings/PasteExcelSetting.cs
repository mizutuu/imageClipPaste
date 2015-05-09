using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageClipPaste.Settings
{
    /// <summary>
    /// Excelに貼り付けるときの設定です
    /// </summary>
    [Serializable]
    public class PasteExcelSetting
    {
        /// <summary>
        /// 画像を貼り付けたあとで、アクティブなセルを画像の下に移動する
        /// </summary>
        public bool MoveActiveCellInImageBelow { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PasteExcelSetting()
        {
            MoveActiveCellInImageBelow = true;
        }
    }
}

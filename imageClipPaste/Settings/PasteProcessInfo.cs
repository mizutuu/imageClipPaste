using imageClipPaste.Enums;
using System;

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
        public string Name { get; set; }

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

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}", Name, HInstance, HWnd, PasteType, IsRequiredNew);
        }
    }
}

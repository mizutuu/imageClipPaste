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
        /// PowerPointの場合はプレゼンテーション名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ワークブック名、プレゼンテーション名を一意に区別できる名前
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 貼り付けタイプ
        /// </summary>
        public PasteType PasteType { get; set; }

        /// <summary>
        /// 新しいワークブック又は新しいプレゼンテーションフラグ
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
            return string.Format("{0}, {1}, {2}, {3}", Name, FullName, PasteType, IsRequiredNew);
        }
    }
}

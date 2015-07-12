using System;

namespace imageClipPaste.Models.Paste
{
    /// <summary>
    /// 貼り付け先プロセスが見つからないときのException
    /// </summary>
    public class PasteProcessNotFoundException : Exception
    {
        public PasteProcessNotFoundException()
        {
        }

        public PasteProcessNotFoundException(string message)
            : base(message)
        {
        }

        public PasteProcessNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

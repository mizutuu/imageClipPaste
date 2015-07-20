using GalaSoft.MvvmLight.Command;
using imageClipPaste.Enums;
using imageClipPaste.Models.Office;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageClipPaste.ViewModel
{
    /// <summary>
    /// 画像の貼り付け先を選択するダイアログのViewModel
    /// </summary>
    public class PasteProcessSelectViewModel : ViewModelBase
    {
        #region Properties
        private ObservableCollection<Settings.PasteProcessInfo> processSource;
        public ObservableCollection<Settings.PasteProcessInfo> ProcessSource
        {
            get { return processSource; }
            set { Set(ref processSource, value); }
        }

        /// <summary>
        /// クリップボードリセット設定
        /// </summary>
        private bool isResetClipboard;
        public bool IsResetClipboard
        {
            get { return isResetClipboard; }
            set {
                Set(ref isResetClipboard, value);
                Properties.Settings.Default.Setting.IsResetClipboard = value;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        #region Commands
        private RelayCommand<Settings.PasteProcessInfo> onSelectProcess;
        public RelayCommand<Settings.PasteProcessInfo> OnSelectProcess
        {
            get
            {
                return onSelectProcess = onSelectProcess ?? new RelayCommand<Settings.PasteProcessInfo>((selected) =>
                {
                    Properties.Settings.Default.Setting.CurrentPasteProcessInfo = selected;
                });
            }
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PasteProcessSelectViewModel()
        {
            var list = new List<Settings.PasteProcessInfo>();
            list.Add(GetNew(PasteType.Excel));
            //list.Add(GetNew(PasteType.PowerPoint)); // TODO:
            list.AddRange(GetPasteProcessList(PasteType.Excel));
            ProcessSource = new ObservableCollection<Settings.PasteProcessInfo>(list);
            IsResetClipboard = Properties.Settings.Default.Setting.IsResetClipboard;
        }

        /// <summary>
        /// MVVM Light ToolkitのCleanup
        /// </summary>
        public override void Cleanup()
        {

            base.Cleanup();
        }

        public Settings.PasteProcessInfo GetNew(PasteType pasteType)
        {
            switch (pasteType)
            {
                case PasteType.Excel:
                    return new Settings.PasteProcessInfo
                    {
                        IsRequiredNew = true,
                        Name = "新しいワークブック",
                        PasteType = PasteType.Excel
                    };
                case PasteType.PowerPoint:
                    return new Settings.PasteProcessInfo
                    {
                        IsRequiredNew = true,
                        Name = "新しいプレゼンテーション",
                        PasteType = PasteType.PowerPoint
                    };
                default:
                    throw new NotImplementedException("not supported.");
            }
        }

        public List<Settings.PasteProcessInfo> GetPasteProcessList(PasteType pasteType)
        {
            switch (pasteType)
            {
                case PasteType.Excel:
                    return ExcelModel.GetPasteExcelProcessList();
                case PasteType.PowerPoint:
                    return GetPastePowerPointList();
                default:
                    throw new NotImplementedException("not supported.");
            }
        }

        public List<Settings.PasteProcessInfo> GetPastePowerPointList()
        {
            // TODO:
            throw new NotImplementedException();
        }
    }
}

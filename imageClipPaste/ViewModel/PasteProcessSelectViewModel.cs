using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using imageClipPaste.Enums;
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
                    return GetPasteExcelProcessList();
                case PasteType.PowerPoint:
                    return GetPastePowerPointList();
                default:
                    throw new NotImplementedException("not supported.");
            }
        }

        public List<Settings.PasteProcessInfo> GetPasteExcelProcessList()
        {
            List<Settings.PasteProcessInfo> result = new List<Settings.PasteProcessInfo>();
            NetOffice.ExcelApi.Application[] applications = NetOffice.ExcelApi.Application.GetActiveInstances();
            applications
                .SelectMany(app => app.Workbooks)
                .ToList()
                .ForEach(book => 
                    result.Add(new Settings.PasteProcessInfo {
                        Name = book.Name,
                        Path = book.Path,
                        HInstance = book.Application.Hinstance,
                        HWnd = book.Application.Hwnd,
                        PasteType = PasteType.Excel
                    }));

            foreach (var app in applications)
                app.Dispose();

            return result;
        }

        public List<Settings.PasteProcessInfo> GetPastePowerPointList()
        {
            // TODO:
            throw new NotImplementedException();
        }
    }
}

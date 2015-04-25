using imageClipPaste.Interfaces;
using imageClipPaste.Services;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace imageClipPaste
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image.png");
        public MainWindow()
        {
            InitializeComponent();

            watcher.Interval = TimeSpan.FromMilliseconds(1000);
            watcher.CapturedNewerImage += watcher_CapturedNewerImage;
        }

        void watcher_CapturedNewerImage(object sender, CapturedNewerImageEventArgs e)
        {
            //putLog("event Fire!!!");
            /*
            image.Dispatcher.Invoke(() =>
            {
                image.Source = e.newImage;
            });
            */
        }

        private ClipboardImageWatchService watcher = new ClipboardImageWatchService();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (watcher.IsEnabled)
            {
                watcher.Stop();
            }
            else
            {
                watcher.Start();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            /*
            NetOffice.ExcelApi.Application[] applications = NetOffice.ExcelApi.Application.GetActiveInstances();
            applications
                .SelectMany(app => app.Workbooks)
                .ToList()
                .ForEach(book => putLog("find excel book: " + book.Name));

            foreach (var app in applications)
                app.Dispose();
            */

            using (var app = NetOffice.ExcelApi.Application.GetActiveInstance())
            {
                using (var activeSheet = app.ActiveSheet as NetOffice.ExcelApi.Worksheet)
                {
                    using (var activeCell = activeSheet.Cells)
                    {
                        //putLog("left:" + activeCell.Left + ", top:" + activeCell.Top);
                    }
                    
                    float left = 0,
                          top = 0;
                    // width, heightは、追加後にScaleを調整するので 0を指定します。
                    using (var shape = activeSheet.Shapes.AddPicture(
                        path,
                        NetOffice.OfficeApi.Enums.MsoTriState.msoFalse,
                        NetOffice.OfficeApi.Enums.MsoTriState.msoTrue,
                        left,
                        top,
                        0,  // width
                        0)) // height 
                    {
                        // 貼り付けた画像の、拡大/縮小率を100%に設定します。
                        shape.ScaleHeight(1, NetOffice.OfficeApi.Enums.MsoTriState.msoTrue);
                        shape.ScaleWidth(1, NetOffice.OfficeApi.Enums.MsoTriState.msoTrue);
                    }
                }
            }
        }
    }
}

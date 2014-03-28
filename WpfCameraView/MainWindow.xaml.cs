using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
//using System.Timers;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfCameraView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CameraCV CameraDevice = new CameraCV();
        private DispatcherTimer timer;
        private System.UInt16 durationInterval;
        private ushort frameinsec;

        public MainWindow()
        {

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(UpgradeImageWindow);

            FrameInSecond = 25;
            timer.Interval = new TimeSpan(0, 0, 0, 0, milliseconds: durationInterval);
            timer.Start();

            InitializeComponent();
        }

        public ushort FrameInSecond 
        {
            set
            {
                frameinsec = value;
                durationInterval = (ushort) (1000/frameinsec);
            }
            get { return frameinsec; }
        }

        private void btnTestPhoto_Click(object sender, RoutedEventArgs e)
        {
        }

        private void UpgradeImageWindow(object sender, EventArgs e)
        {
            Bitmap bmp = CameraDevice.GetFrame();

            BitmapImage bi = new BitmapImage();
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            bi.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.StreamSource = ms;
            bi.EndInit();

            ViewWinDS.Source = bi;
        }

    }
}

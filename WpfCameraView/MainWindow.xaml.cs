using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
//using System.Timers;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using DragEventArgs = System.Windows.DragEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;


namespace WpfCameraView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    //public interface IMainWindow
    //{

    //}
    public partial class MainWindow : Window//, IMainWindow
    {
        CameraCV CameraDevice = new CameraCV();
        private DispatcherTimer _timer;
        private System.UInt16 durationInterval;
        private ushort frameinsec;

        

        public MainWindow()
        {
            InitializeComponent();
            InitBegin();
            //ViewWinDS.mouse
        }

        private void InitBegin()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(UpgradeImageWindow);

            FrameInSecond = 25;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, milliseconds: durationInterval);
            _timer.Start();

            ViewWinDS.Drop += ViewWinDsOnDrop;
            ViewWinDS.DragEnter += ViewWinDsOnDragEnter;
            ViewWinDS.DragLeave += ViewWinDsOnDragLeave;
            ViewWinDS.DragOver += ViewWinDsOnDragOver;
            ViewWinDS.Stretch = Stretch.Uniform;
        }



        #region event handler

        private void ViewWinDsOnDragOver(object sender, DragEventArgs dragEventArgs)
        {
            TempTextBox.Text += " dragov ";
        }

        private void ViewWinDsOnDragLeave(object sender, DragEventArgs dragEventArgs)
        {
            TempTextBox.Text += " dragle ";
        }

        private void ViewWinDsOnDragEnter(object sender, DragEventArgs dragEventArgs)
        {
            TempTextBox.Text += " dragen ";
        }

        private void ViewWinDsOnDrop(object sender, DragEventArgs dragEventArgs)
        {
            TempTextBox.Text += " drop ";
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

        private void ViewWinDS_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (ViewWinDS.IsMouseOver && (Keyboard.IsKeyDown(key: Key.LeftCtrl) || Keyboard.IsKeyDown(key: Key.RightCtrl)))
            {
                if (e.Delta > 0) TempTextBox.Text += " up ";
                else TempTextBox.Text += " down ";
            }
            if ((Mouse.LeftButton == MouseButtonState.Pressed) && ViewWinDS.IsMouseOver )
            {
                if (e.Delta > 0) TempTextBox.Text += " вверх ";
                else TempTextBox.Text += " вниз ";
            }
        }
        #endregion

        public ushort FrameInSecond
        {
            set
            {
                frameinsec = value;
                durationInterval = (ushort)(1000 / frameinsec);
            }
            get { return frameinsec; }
        }

        //private void MenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    _timer.Stop();
        //}

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Close();
        }

        


        private void DoOpenCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ImageDocuments imageDocument = new ImageDocuments();
            if (imageDocument.Open())
            {
                _timer.Stop();
                BitmapImage bmImageScreen = new BitmapImage();
                bmImageScreen.BeginInit();
                bmImageScreen.UriSource = new Uri(imageDocument
                    .PhysicalLocation, UriKind.Relative);
                bmImageScreen.CacheOption = BitmapCacheOption.OnLoad;
                bmImageScreen.EndInit();
                ViewWinDS.Source = bmImageScreen;
            }
            
        }
    }
}

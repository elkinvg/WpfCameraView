using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing.Imaging;
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
        
        public MainWindow()
        {
            InitializeComponent();
            InitBegin();
            //ViewWinDS.mouse

            InitializeScaleTransform();
            //InitializeRotateTransform();
            InitializeTransformGroup();
        }

        #region TESTREGION
        private ScaleTransform scaleTransform = new ScaleTransform();
        //private RotateTransform rotateTransform = new RotateTransform();


        //private void MenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    _timer.Stop();
        //}

        private void btnTestPhoto_Click(object sender, RoutedEventArgs e)
        {
            CameraDevice.SaveFrameAs("fh.jpg");
            CameraDevice.SaveFrameAs("fh.png");
            CameraDevice.SaveFrameAs("fh.bmp");
        }


        private void InitializeScaleTransform()
        {
            scaleTransform.ScaleX = 1.0;
            scaleTransform.ScaleY = 1.0;

            scaleTransform.CenterX = 50.0;
            scaleTransform.CenterY = 50.0;
        }

        //private void InitializeRotateTransform()
        //{
        //    rotateTransform.Angle = 0.0;
        //    rotateTransform.CenterX = 150.0;
        //    rotateTransform.CenterY = 50.0;
        //}

        private void InitializeTransformGroup()
        {
            TransformGroup transformGroup = new TransformGroup();
            //transformGroup.Children.Add(rotateTransform);
            transformGroup.Children.Add(scaleTransform);
            //myEllipse.RenderTransform = transformGroup;
            ViewWinDS.RenderTransform = transformGroup;
        }


        #endregion

        #region public Properties

        public ushort FrameInSecond
        {
            set
            {
                _frameinsec = value;
                _durationInterval = (ushort)(1000 / _frameinsec);
            }
            get { return _frameinsec; }
        }

        #endregion

        #region private Methods

        #region Methods

        private void InitBegin()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += UpgradeImageWindow;

            FrameInSecond = 25;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, milliseconds: _durationInterval);

            if (!CameraDevice.IsCameraExist) MenuItemVideoCheck.IsEnabled = false;
            else
            {
                MenuItemVideoCheck.IsChecked = true;
                CheckCameraItem.IsEnabled = false;
            }

            ViewWinDS.Stretch = Stretch.Uniform;
            _isImageDocumentInit = false;
            SaveFile.IsEnabled = false;

        }

        #endregion

        #region event handler

        private void MenuItemVideoCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (MenuItemVideoCheck.IsChecked)
            {
                _timer.Start();
            }
        }

        private void MenuItemVideoCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!MenuItemVideoCheck.IsChecked)
            {
                _timer.Stop();
            }
        }

        private void CheckCameraItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (CameraDevice.CheckCamera())
            {
                MenuItemVideoCheck.IsEnabled = true;
                MenuItemVideoCheck.IsChecked = true;
                CheckCameraItem.IsEnabled = false;
            }
        }

        private void UpgradeImageWindow(object sender, EventArgs e)
        {
            using (var ms = new MemoryStream())
            {
                Bitmap bmp = CameraDevice.GetFrame();
                if (bmp == null)
                {
                    MenuItemVideoCheck.IsEnabled = false;
                    MenuItemVideoCheck.IsChecked = false;
                    CheckCameraItem.IsEnabled = true;
                }

                BitmapImage bi = new BitmapImage();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                bi.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = ms;
                bi.EndInit();

                ViewWinDS.Source = bi;
            }
        }

        private void ViewWinDS_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (ViewWinDS.IsMouseOver && (Keyboard.IsKeyDown(key: Key.LeftCtrl) || Keyboard.IsKeyDown(key: Key.RightCtrl)))
            {
                if (e.Delta > 0)
                {
                    TempTextBox.Text += " up ";
                    ViewWinDS.Stretch = Stretch.None;
                    //scaleTransform.ScaleX *= .97;
                    //scaleTransform.ScaleY *= .97;
                }
                else
                {
                    TempTextBox.Text += " down ";
                    ViewWinDS.Stretch = Stretch.Uniform;
                }
            }
            if ((Mouse.LeftButton == MouseButtonState.Pressed) && ViewWinDS.IsMouseOver)
            {
                if (e.Delta > 0) TempTextBox.Text += " вверх ";
                else TempTextBox.Text += " вниз ";
            }

        }

        private void DoOpenCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (!_isImageDocumentInit)
            {
                _imageDocument = new ImageDocuments();
                _isImageDocumentInit = true;
            }

            if (_imageDocument.Open())
            {
                if (_imageDocument.LoadedBitmapImage != null)
                {
                    if (_timer.IsEnabled) _timer.Stop();
                    ViewWinDS.Source = _imageDocument.LoadedBitmapImage;

                    TempTextBox.Text += "Loaded Image" + _imageDocument.OpenImFormat;
                    if (!SaveFile.IsEnabled) SaveFile.IsEnabled = true;
                    
                }
                else
                {
                    if (!_timer.IsEnabled) _timer.Start();
                    TempTextBox.Text += "It isn't Image ";
                }
            }
        }

        private void DoSaveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (!_isImageDocumentInit)
            {
                _imageDocument = new ImageDocuments();
                _isImageDocumentInit = true;
            }

            if (_imageDocument.Save())
            {
                if (_imageDocument.LoadedBitmapImage != null)
                {

                    TempTextBox.Text += "Save Image";
                }
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Close();
        }

        #endregion

        #endregion

        #region private fields

        private DispatcherTimer _timer;
        private System.UInt16 _durationInterval;
        private ushort _frameinsec;
        private ImageDocuments _imageDocument;
        private bool _isImageDocumentInit;

        #endregion
    }
}

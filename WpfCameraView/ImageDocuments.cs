using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using wf = System.Windows.Forms;

namespace WpfCameraView
{
    internal class ImageDocuments
    {

        #region TESTREGION
        #endregion

        #region public Properties

        public string PhysicalLocation
        {
            get { return _physicalLocation; }
            set { _physicalLocation = value; }
        }

        public BitmapImage LoadedBitmapImage
        {
            get { return _loadedBitmapImage; }
        }

        public string OpenImFormat
        {
            get { return _openImFormat; }
        }

        #endregion

        #region public Methods

        public bool Open()
        {
            wf.OpenFileDialog opndlg = new wf.OpenFileDialog();
            opndlg.Filter = "Image Documents (*.jpg;*.png;*.jpeg;*.tiff)|*.jpg;*.png;*.jpeg;*.tiff";

            opndlg.InitialDirectory = !string.IsNullOrEmpty(_physicalLocation) ? _physicalLocation.Substring(0, _physicalLocation.LastIndexOf('\\')) : AppDomain.CurrentDomain.BaseDirectory;

            opndlg.Multiselect = false;
            opndlg.CheckFileExists = true;

            if (opndlg.ShowDialog() == wf.DialogResult.OK)
            {
                _physicalLocation = opndlg.FileName;
                //_initialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                return false;
            }


            if (_physicalLocation.Length > 0)
            {
                LoadImageDocument();
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool Save()
        {

            using (var stream = new FileStream(_physicalLocation, FileMode.Create))
            {

                BitmapEncoder bi;
                switch (_openImFormat)
                {
                    case "jpeg":
                        bi = new JpegBitmapEncoder();
                        JustSaveToStream(bi, stream);
                        break;
                    case "png":
                        bi = new PngBitmapEncoder();
                        JustSaveToStream(bi, stream);
                        break;
                    case "tiff":
                        bi = new TiffBitmapEncoder();
                        JustSaveToStream(bi, stream);
                        break;
                    case "bmp":
                        bi = new BmpBitmapEncoder();
                        JustSaveToStream(bi, stream);
                        break;
                }
                
                //bi.Frames.Add(BitmapFrame.Create(_loadedBitmapImage));
                ////bi.
                ////bi.Metadata = _pictMetadata.Clone();
                ////bi.QualityLevel = 95;
                //bi.Save(stream);
            }
            return true;
        }

        #endregion

        #region private Methods

        private static string GetImageFormat(Stream stream)
        {
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };    // PNG
            var tiff = new byte[] { 73, 73, 42 };         // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

            var buffer = new byte[4];
            stream.Read(buffer, 0, buffer.Length);
            stream.Seek(0,SeekOrigin.Begin);

            if (bmp.SequenceEqual(buffer.Take(bmp.Length)))
                return "bmp";

            if (gif.SequenceEqual(buffer.Take(gif.Length)))
                return "gif";

            if (png.SequenceEqual(buffer.Take(png.Length)))
                return "png";

            if (tiff.SequenceEqual(buffer.Take(tiff.Length)))
                return "tiff";

            if (tiff2.SequenceEqual(buffer.Take(tiff2.Length)))
                return "tiff";

            if (jpeg.SequenceEqual(buffer.Take(jpeg.Length)))
                return "jpeg";

            if (jpeg2.SequenceEqual(buffer.Take(jpeg2.Length)))
                return "jpeg";

            return "unknown";
        }

        private void JustSaveToStream(BitmapEncoder encoder, Stream stream)
        {
            encoder.Frames.Add(BitmapFrame.Create(_loadedBitmapImage
                ));
            encoder.Save(stream);
        }

        void LoadImageDocument()
        {
            try
            {
                BitmapImage bmImageScreen = new BitmapImage();

                _loadedBitmapImage = bmImageScreen;
                using (var stream = new FileStream(_physicalLocation, FileMode.Open))
                {
                    //JpegBitmapDecoder decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    bmImageScreen.BeginInit();
                    bmImageScreen.StreamSource = stream;
                    _openImFormat = GetImageFormat(stream);
                    bmImageScreen.CacheOption = BitmapCacheOption.OnLoad;
                    bmImageScreen.EndInit();
                }


                return;
            }
            catch (NotSupportedException nse)
            {
                //_loadedBitmapImage = null;
            }
        }

        #endregion

        #region private fields

        private BitmapImage _loadedBitmapImage = null;
        private string _physicalLocation = "";
        private string _openImFormat;

        #endregion
    }


}

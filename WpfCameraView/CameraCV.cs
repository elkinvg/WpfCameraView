using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace WpfCameraView
{
    class CameraCV
    {
        private Capture _cameraCapture;
        private bool _isCameraExist;


        private Image<Bgr, Byte> _frameImage;

        public CameraCV()
        {
            CheckCamera();
        }

        public bool IsCameraExist
        {
            get { return _isCameraExist; }
        }

        ~CameraCV()
        {
            //_cameraCapture.Stop();
        }

        public bool CheckCamera()
        {
            if (_cameraCapture != null) _cameraCapture.Dispose();
            try
            {
                //Set up capture device
                _cameraCapture = new Capture();
                //_cameraCapture.ImageGrabbed += checkCamera;
                _frameImage = _cameraCapture.QueryFrame();
                _isCameraExist = _frameImage != null;

            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
                _isCameraExist = false;

            }
            return _isCameraExist;
        }

        //private void checkC

        public Bitmap GetFrame()
        {
            try
            {
                _frameImage = _cameraCapture.QueryFrame();
                return _frameImage.ToBitmap();
            }
                catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
                if(_isCameraExist) _isCameraExist = false;
                return null;
            }
        }

        public void SaveFrameAs(System.String filename)
        {
            _frameImage = _cameraCapture.QueryFrame();
            _frameImage.Save(filename);
            _cameraCapture.Stop();

        }
        //private void 

        public Bitmap sobb()
        {
            //_cameraCapture.Start();
            _frameImage = _cameraCapture.QueryFrame();
            _frameImage.Save("ddd.jpg");
            Bitmap asBitmap = _frameImage.ToBitmap(_frameImage.Width, _frameImage.Height);
            //_cameraCapture.Pause();
            return asBitmap;
            //asBitmap.Save("ddd2.jpg");

        }
    }
}

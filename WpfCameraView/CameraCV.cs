using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace WpfCameraView
{
    class CameraCV
    {
        private readonly Capture cameraCapture;
        
        private Image<Bgr, Byte> frameImage;

        public CameraCV()
        {
            cameraCapture = new Capture();
        }

        ~CameraCV()
        {
            //cameraCapture.Stop();
        }

        private void GetVideo(object sender, EventArgs e)
        {
            //frameImage = cameraCapture.QueryFrame();
            //frameImage.Save("ddd.jpg");
        }

        public Bitmap GetFrame()
        {
            frameImage = cameraCapture.QueryFrame();
            return frameImage.ToBitmap();
        }

        public void SaveFrameAs(System.String filename)
        {
            frameImage = cameraCapture.QueryFrame();
            frameImage.Save(filename);
        }
        //private void 

        public Bitmap sobb()
        {
            //cameraCapture.Start();
            frameImage = cameraCapture.QueryFrame();
            frameImage.Save("ddd.jpg");
            Bitmap asBitmap = frameImage.ToBitmap(frameImage.Width, frameImage.Height);
            //cameraCapture.Pause();

            return asBitmap;
            //asBitmap.Save("ddd2.jpg");

        }
    }
}

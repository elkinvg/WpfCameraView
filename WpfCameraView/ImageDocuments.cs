using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wf = System.Windows.Forms;

namespace WpfCameraView
{
    class ImageDocuments
    {

        private string _physicalLocation = "";

        public string PhysicalLocation
        {
            get { return _physicalLocation; }
            set { _physicalLocation = value; }
        }

        public bool Open()
        {
            wf.OpenFileDialog opndlg = new wf.OpenFileDialog();
            opndlg.Filter = "Image Documents (*.jpg;*.png;*.jpeg;*.tiff)|*.jpg;*.png;*.jpeg;*.tiff";
            //opndlg.Filter = "Text Documents (*.txt)|*.txt|All Files|*.*";
            if (!string.IsNullOrEmpty(_physicalLocation))
            {
                opndlg.InitialDirectory = _physicalLocation.Substring(0, _physicalLocation.LastIndexOf('\\'));
            }
            else
            {
                opndlg.InitialDirectory = Environment.GetEnvironmentVariable("SystemDrive") + "\\";
            }
            opndlg.Multiselect = false;
            opndlg.CheckFileExists = true;

            if (opndlg.ShowDialog() == wf.DialogResult.OK)
            {
                _physicalLocation = opndlg.FileName;
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}

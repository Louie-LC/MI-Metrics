using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public enum DialogType
    {
        Image = 0,
        CSV = 1
    }

    class DialogManager
    {
        public static SelectFileResult ShowDialog(DialogType type)
        {

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            switch (type)
            {
                case DialogType.Image:
                    SetImageProperties(dlg);
                    break;
                case DialogType.CSV:
                    SetCSVProperties(dlg);
                    break;
            }
            
            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();
            string fileName = "";
            bool fileSelected = false;
            if (result == true)
            {
                fileName = dlg.FileName;
                fileSelected = true;
            }

            return new SelectFileResult(fileName, fileSelected);
 
        }

        private static void SetImageProperties(Microsoft.Win32.OpenFileDialog dlg)
        {
            dlg.Title = "Select Image File";
            dlg.FileName = "Image"; // Default file name
            dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "PNG|*.png" +
                "|JPG|*.jpg" +
                "|GIF|*.gif" +
                "|All Files|*.*"; // Filter files by extensions

            dlg.Multiselect = false;
            dlg.CheckFileExists = true;
        }

        private static void SetCSVProperties(Microsoft.Win32.OpenFileDialog dlg)
        {
            dlg.Title = "Select CSV File";
            dlg.FileName = "TestData"; // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "CSV|*.csv"; // Filter files by extensions

            dlg.Multiselect = false;
            dlg.CheckFileExists = true;
        }


    }
}

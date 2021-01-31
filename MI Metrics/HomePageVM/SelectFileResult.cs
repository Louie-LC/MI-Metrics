using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public class SelectFileResult
    {
        public bool validFileSelected;
        public string filePath;

        public SelectFileResult(string filePath, bool validFileSelected)
        {
            this.filePath = filePath;
            this.validFileSelected = validFileSelected;
        }
    }
}

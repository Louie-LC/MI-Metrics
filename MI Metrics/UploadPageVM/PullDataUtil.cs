using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public class PullDataUtil
    {
        public List<IntermediateTestResult> PullDataFromCSV(Product product, string filePath)
        {
            List<ControlFile> controlFiles = MainWindow.database.controlFileManager.RetrieveControlFileList(product);
            BuildTestStepLists(controlFiles);

            ScannerDataImporter importer = new ScannerDataImporter();
            return importer.ImportCSVData(filePath, controlFiles, product.Tests);
        }

        private void BuildTestStepLists(List<ControlFile> controlFiles)
        {
            foreach (ControlFile controlFile in controlFiles)
            {
                controlFile.BuildTestStepList();
            }
        }
    }
}

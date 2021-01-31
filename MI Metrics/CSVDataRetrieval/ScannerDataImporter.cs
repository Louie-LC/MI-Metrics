using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public class ScannerDataImporter
    {
        private FileParser fileParser;
        private List<IntermediateTestResult> testData;
        
        public List<IntermediateTestResult> ImportCSVData(string fileName, List<ControlFile> controlFiles, List<Test> tests)
        {
            fileParser = new CSVParser(fileName);
            ((CSVParser)fileParser).AddControlFiles(controlFiles);
            ((CSVParser)fileParser).CreateTestDictionary(tests);
            testData = fileParser.RetrieveContentsFromFile();
            return testData;
        }        
    }
}

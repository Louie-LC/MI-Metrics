using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace MI_Metrics
{

    public class CSVParser : FileParser
    {
        private TextFieldParser csvParser;

        // These are the column names found in the CSV file
        public const string CSVHeaderSystemID = "DHR ident";
        public const string CSVHeaderTestName = "MT ident";
        public const string CSVHeaderTestResult = "Entry";
        public const string CSVHeaderTestDate = "Date";
        public const string CSVHeaderSubMT = "Sub-MT No";
        public const string CSVHeaderControlFile = "Control file ident";

        //There are the indexes of the columns that need to be tracked
        private int indexSystemID;
        private int indexTestName;
        private int indexTestResult;
        private int indexTestDate;
        private int indexSubMT;
        private int indexControlFile;

        private Dictionary<string, List<TestStep>> controlFileDictionary;
        private Dictionary<int, string> testDictionary;

        public string FilePath { get => _filePath; set => _filePath = value; }
        string _filePath;

        public CSVParser(string file)
        {
            FilePath = @file;
            indexSystemID = -1;
            indexTestName = -1;
            indexTestResult = -1;
            indexTestDate = -1;
            indexSubMT = -1;
            controlFileDictionary = new Dictionary<string, List<TestStep>>();
            testDictionary = new Dictionary<int, string>();
        }

        // This method should be called after the constructor in order to retrieve data from the set file
        public override List<IntermediateTestResult> RetrieveContentsFromFile()
        {
            List<string[]> fileContentList = RetrieveCSVContents();
            ProcessHeaderContent(fileContentList);
            return CreateTestData(fileContentList);
        }

        private List<string[]> RetrieveCSVContents()
        {
            List<string[]> fileContent = new List<string[]>();
            using (csvParser = new TextFieldParser(FilePath))
            {
                InitializeParserProperties();

                // Skip the header row that contains the column names
                // csvParser.ReadLine();
                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    fileContent.Add(csvParser.ReadFields());
                }
                csvParser.Close();
            }

            return fileContent;
        }

        private void InitializeParserProperties()
        {
            // Setup how the parser reads data from the CSV file
            csvParser.SetDelimiters(new string[] { "," });
            csvParser.HasFieldsEnclosedInQuotes = false;
            csvParser.TrimWhiteSpace = true;
        }

        private void ProcessHeaderContent(List<string[]> fileContentList)
        {
            /*
             * The CSV file has a header which has the name of the columns.
             * This methods uses the header to find the desired test columns, and then discards the header. 
             */
            string[] contentHeader = fileContentList[0];
            SetHeaderIndex(contentHeader);
            fileContentList.RemoveAt(0);
        }

        private void SetHeaderIndex(string[] header)
        {
            // A method for finding which CSV columns contain the desired test data
            for (int i = 0; i < header.Length; i++)
            {
                if (header[i].Equals(CSVHeaderSystemID))
                {
                    indexSystemID = i;
                }
                else if (header[i].Equals(CSVHeaderTestName))
                {
                    indexTestName = i;
                }
                else if (header[i].Equals(CSVHeaderTestResult))
                {
                    indexTestResult = i;
                }
                else if (header[i].Equals(CSVHeaderTestDate))
                {
                    indexTestDate = i;
                }
                else if (header[i].Equals(CSVHeaderSubMT))
                {
                    indexSubMT = i;
                }
                else if (header[i].Equals(CSVHeaderControlFile))
                {
                    indexControlFile = i;
                }
            }
        }

        public void AddControlFile(ControlFile controlFile)
        {
            controlFileDictionary.Add(controlFile.FullDescription, controlFile.TestSteps);
        }

        public void AddControlFiles(List<ControlFile> controlFiles)
        {
            foreach(ControlFile controlFile in controlFiles)
            {
                AddControlFile(controlFile);
            }
        }

        public void CreateTestDictionary(List<Test> tests)
        {
            foreach (Test test in tests)
            {
                testDictionary.Add(test.TestID, test.Name);
            }
        }

        private List<IntermediateTestResult> CreateTestData(List<string[]> fileContent)
        {
            List<IntermediateTestResult> testData;
            testData = GenerateData(fileContent);
            return testData;
        }
        private List<IntermediateTestResult> GenerateData(List<string[]> fileContents)
        {
            List<IntermediateTestResult> allTestData = new List<IntermediateTestResult>();
            foreach (string[] csvRow in fileContents)
            {
                // Only rows that have preset control file associated with them should have their data searched.
                // This will skip the current row if the row's control file hasn't been defined in the program.
                List<TestStep> testSteps = null;
                string controlFileIdent = csvRow[indexControlFile].TrimStart('0');
                if (controlFileDictionary.ContainsKey(controlFileIdent))
                {
                    testSteps = controlFileDictionary[controlFileIdent];
                }
                else
                {
                    continue;
                }
                
                // This pulls just the step number from the CSV description field
                // If the description is blank, this iteration of the loop is skipped with a continue
                string csvTestDescription = csvRow[indexTestName];
                string csvSubStep = csvRow[indexSubMT];
                if (csvTestDescription.Length == 0) continue;


                int firstSpace = csvTestDescription.IndexOf(" ");
                string csvStep = csvTestDescription.Substring(0, firstSpace);

                foreach (TestStep testStep in testSteps)
                {
                    if (csvStep.Equals(testStep.Step) && csvTestDescription.Contains(" M" + testStep.MStep + " ") && csvSubStep.Equals(testStep.SubStep))
                    {
                        // Now that an IEDTest is found that matches the row that's being evaluated, add the row's information into the set of tests to be returned.
                        IntermediateTestResult result = new IntermediateTestResult();
                        result.TestID = testStep.TestID;
                        result.SystemDescription = TrimDescription(csvRow[indexSystemID]);
                        result.Value = csvRow[indexTestResult];
                        result.Date = csvRow[indexTestDate];
                        result.TestName = testDictionary[testStep.TestID];
                        result.ControlFile = controlFileIdent;

                        allTestData.Add(result);
                    }
                }
            }
            return allTestData;
        }

        private string TrimDescription(string description)
        {
            // The front of partnumber and serial number both need leading zeroes trimmed.
            // These fields are stored later on as numerics and comparisons will not work
            // if the zeroes are kept.
            string[] information = description.Split('_');
            if (information.Length != 5)
                return description;
            string partNumber = information[0].TrimStart('0');
            string serialNumber = information[4].TrimStart('0');

            return String.Format("{0}_{1}_{2}_{3}_{4}", partNumber, information[1], information[2], information[3], serialNumber);
        }
    }


}

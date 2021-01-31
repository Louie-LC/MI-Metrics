using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public class ManageControlFileUtil
    {
        TemplatePageViewModel viewModel;


        public ManageControlFileUtil(TemplatePageViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public ControlFile AddControlFile(string numberText, string type, string part, string version, Product product)
        {
            long number;
            if (!Int64.TryParse(numberText, out number)) return null;
            if(product == null || !DatabaseHelper.ValidateCharacters(type) || !DatabaseHelper.ValidateCharacters(part) || !DatabaseHelper.ValidateCharacters(version))
            {
                return null;
            }

            ControlFile controlFile = MainWindow.database.controlFileManager.AddControlFile(number, type, part, version, product);
            return controlFile;
        }

        public void EditControlFile(ControlFile controlFile, long number, string type, string part, string version)
        {
            bool differentNumber = !(controlFile.Number == number);
            bool differentType = !controlFile.Type.Equals(type);
            bool differentPart = !controlFile.Part.Equals(part);
            bool differentVersion = !controlFile.Version.Equals(version);

            if (differentNumber || differentType || differentPart || differentVersion)
            {
                MainWindow.database.controlFileManager.EditControlFile(controlFile, number, type, part, version);
            }
        }

        public void EditTestSteps(TemplateChange changes)
        {
            foreach(TestStep testStep in changes.deletedTestSteps)
            {
                MainWindow.database.testStepManager.DeleteTestStep(testStep);
            }
            foreach (TestStep testStep in changes.editedTestSteps)
            {
                MainWindow.database.testStepManager.EditTestStep(testStep);
            }
            foreach (TestStep testStep in changes.addedTestSteps)
            {
                MainWindow.database.testStepManager.AddTestStep(testStep);
            }
        }
    }
}

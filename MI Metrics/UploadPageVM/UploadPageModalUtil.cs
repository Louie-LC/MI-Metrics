using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MI_Metrics
{
    public class UploadPageModalUtil
    {
        public static void ShowUploadSuccessModal(int totalTest, Window window)
        {
            string label = "Upload Succesful";
            string message;
            if (totalTest == 1)
            {
                message = String.Format("{0} test result was successfully uploaded to the database.", totalTest);
            }
            else
            {
                message = String.Format("{0} test results were successfully uploaded to the database.", totalTest);
            }
            MessageModal messageModal = new MessageModal(label, message, ModalType.Success)
            {
                Owner = window,
                Height = 200,
                Width = 360
            };
            messageModal.ShowDialog();
        }

        public static void ShowUploadErrorModal(int totalTest, int errorCount, Window window)
        {
            int successCount = totalTest - errorCount;

            string label = "Errors During Upload";
            string message;
            if (successCount == 1)
            {
                message = String.Format("{0} test result was successfully uploaded to the database.", successCount);
            }
            else
            {
                message = String.Format("{0} test results were successfully uploaded to the database.", successCount);
            }
            if (errorCount == 1)
            {
                message += String.Format("\n{0} test result encountered errors and was unable to be uploaded.", errorCount);
            }
            else
            {
                message += String.Format("\n{0} test results encountered errors and were unable to be uploaded.", errorCount);
            }

            MessageModal messageModal = new MessageModal(label, message, ModalType.Error)
            {
                Owner = window,
                Height = 200,
                Width = 360
            };
            messageModal.ShowDialog();
        }

        public static void ShowNoTestResultsModal(Window window)
        {
            string label = "No Test Results Present";
            string message = "There are no test results in the Test Data list. Test results first need to be pulled from a .csv file before they can be uploaded.";
            MessageModal messageModal = new MessageModal(label, message, ModalType.Warning)
            {
                Owner = window,
                Height = 200,
                Width = 360
            };
            messageModal.ShowDialog();
        }

        public static void ShowErrorModal(int errorCount, Window window)
        {
            string label = "Errors Found";
            string message;
            if (errorCount == 1)
            {
                message = String.Format("There is {0} test result with errors that need to be resolved before the data can be uploaded.", errorCount);
            }
            else
            {
                message = String.Format("There are {0} test results with errors that need to be resolved before the data can be uploaded.", errorCount);
            }

            MessageModal messageModal = new MessageModal(label, message, ModalType.Error)
            {
                Owner = window,
                Height = 200,
                Width = 360
            };
            messageModal.ShowDialog();
        }

        public static bool DisplayConfirmationWindow(int addedTestCount, int warningCount, Window window)
        {
            string label = "Upload Data";
            string message;
            if (warningCount > 0)
            {
                if (addedTestCount == 1)
                {
                    message = String.Format("{0} new test result will be added.", addedTestCount);
                }
                else
                {
                    message = String.Format("{0} new test results will be added.", addedTestCount);
                }

                if (warningCount == 1)
                {
                    message += String.Format("\n{0} existing test result will be updated.", warningCount);
                }
                else
                {
                    message += String.Format("\n{0} existing test results will be updated.", warningCount);
                }
            }
            else
            {
                if (addedTestCount == 1)
                {
                    message = String.Format("{0} new test result will be added.", addedTestCount);
                }
                else
                {
                    message = String.Format("{0} new test results will be added.", addedTestCount);
                }
            }


            ConfirmationModal resultsModal = new ConfirmationModal(label, message)
            {
                Owner = window,
                Height = 200,
                Width = 360
            };
            resultsModal.ShowDialog();

            return resultsModal.SubmitButtonPressed;
        }
    }
}

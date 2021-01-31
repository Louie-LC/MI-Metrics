using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public enum ResultError
    {
        CSVConflict,
        IncorrectSystemID,
        IncorrectValue,
        IncorrectDate
    }

    public enum ResultWarning
    {
        DatabaseConflict
    }



    public class IntermediateTestResult : INotifyPropertyChanged
    {
        private static readonly string[] ErrorMessages =
        {
            "This entry conflicts with another test entry",
            "System ID is in an incorrect format",
            "Test Result should be a valid number",
            "Test Date should be a valid date"
        };
        private static readonly string[] WarningMessages =
        {
            "A test entry for this system already exissts in the database"
        };

        private string errorMessage;
        private string warningMessage;

        private List<ResultError> errors;
        private List<ResultWarning> warnings;

        private bool errorFound;
        private bool warningFound;

        private int testID;
        private string systemDescription;
        private string value;
        private string date;
        private string testName;
        private string controlFile;




        public int TestID { get => testID; set => testID = value; }
        public string SystemDescription { get => systemDescription; set => systemDescription = value; }
        public string Value { get => value; set => this.value = value; }
        public string Date { get => date; set => date = value; }
        public string TestName { get => testName; set => testName = value; }
        public string ControlFile { get => controlFile; set => controlFile = value; }
        public bool ErrorFound
        {
            get => errorFound;
            set
            {
                errorFound = value;
                TestResult_PropertyChanged();
            }
        }
        public bool WarningFound
        {
            get => warningFound;
            set
            {
                warningFound = value;
                TestResult_PropertyChanged();
            }
        } 
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                TestResult_PropertyChanged();
            }
        }
        public string WarningMessage
        {
            get => warningMessage;
            set
            {
                warningMessage = value;
                TestResult_PropertyChanged();
            }
        }

        public IntermediateTestResult()
        {
            ErrorFound = false;
            WarningFound = false;
            ErrorMessage = "";
            WarningMessage = "";
            errors = new List<ResultError>();
            warnings = new List<ResultWarning>();
        }

        public void AddErrorMesssage(ResultError error)
        {
            errors.Add(error);
            AssignErrorMessage();
            ErrorFound = true;
        }

        public void AddWarningMessage(ResultWarning warning)
        {
            warnings.Add(warning);
            AssignWarningMessage();
            WarningFound = true;
        }




        public void RemoveErrorMessage(ResultError error)
        {
            errors.Remove(error);
            if (errors.Count == 0)
            {
                ErrorFound = false;
            }
            AssignErrorMessage();
        }
        
        public void RemoveWarningMessage(ResultWarning warning)
        {
            warnings.Remove(warning);
            if (warnings.Count == 0)
            {
                WarningFound = false;
            }
            AssignWarningMessage();
        }

        private void AssignErrorMessage()
        {
            if(errors.Count > 0)
            {
                if (errors.Contains(ResultError.CSVConflict))
                {
                    ErrorMessage = ErrorMessages[(int)ResultError.CSVConflict];
                }
                else
                {
                    ErrorMessage = ErrorMessages[(int)errors[0]];
                }
            }
            else
            {
                ErrorMessage = "";
            }
        }

        private void AssignWarningMessage()
        {
            if (warnings.Count > 0)
            {
                WarningMessage = WarningMessages[(int)warnings[0]];
            }
            else
            {
                WarningMessage = "";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void TestResult_PropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


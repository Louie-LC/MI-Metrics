using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public class TestStep : INotifyPropertyChanged
    {
        private int testStepID;
        private string step;
        private string mStep;
        private string subStep; // Can be null
        private double? lowerLimit; // Can be null
        private double? upperLimit; // Can be null
        private int controlFileID;
        private int testID;

        public int TestStepID
        {
            get => testStepID;
            set
            {
                testStepID = value;
                Test_PropertyChagned();
            }
        }
        public string Step
        {
            get => step;
            set
            {
                step = value;
                Test_PropertyChagned();
            }
        }
        public string MStep
        {
            get => mStep;
            set
            {
                mStep = value;
                Test_PropertyChagned();
            }
        }
        public string SubStep
        {
            get => subStep;
            set
            {
                subStep = value;
                Test_PropertyChagned();
            }
        }
        public double? LowerLimit
        {
            get => lowerLimit;
            set
            {
                lowerLimit = value;
                Test_PropertyChagned();
            }
        }
        public double? UpperLimit
        {
            get => upperLimit;
            set
            {
                upperLimit = value;
                Test_PropertyChagned();
            }
        }
        public int ControlFileID
        {
            get => controlFileID;
            set
            {
                controlFileID = value;
                Test_PropertyChagned();
            }
        }
        public int TestID
        {
            get => testID;
            set
            {
                testID = value;
                Test_PropertyChagned();
            }
        }


        public TestStep(string step, string mStep, string subStep, double? lowerLimit, double? upperLimit, int testID)
        {
            Step = step;
            MStep = mStep;
            SubStep = subStep;
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
            TestID = testID;
        }

        public TestStep(int testStepID, string step, string mStep, string subStep, double? lowerLimit, double? upperLimit, int controlFileID, int testID)
        {
            TestStepID = testStepID;
            Step = step;
            MStep = mStep;
            SubStep = subStep;
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
            ControlFileID = controlFileID;
            TestID = testID;
        }

        public override String ToString()
        {
            string returnString = "";
            returnString += step + " - M" + mStep;
            if (!String.IsNullOrEmpty(subStep))
            {
                returnString += " - " + subStep;
            }
            return returnString;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Test_PropertyChagned([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public class ControlFile : INotifyPropertyChanged
    {
        private int controlFileID;
        private long number;
        private string type;
        private string part;
        private string version;
        private int productID;
        private string fullDescription;
        private List<TestStep> testSteps;

        public int ControlFileID
        {
            get => controlFileID;
            set
            {
                controlFileID = value;
                ControlFile_PropertyChanged();
            }
        }
        public long Number
        {
            get => number;
            set
            {
                number = value;
                ControlFile_PropertyChanged();
            }
        }
        public string Type
        {
            get => type;
            set
            {
                type = value;
                ControlFile_PropertyChanged();
            }
        }
        public string Part
        {
            get => part;
            set
            {
                part = value;
                ControlFile_PropertyChanged();
            }
        }
        public string Version
        {
            get => version;
            set
            {
                version = value;
                ControlFile_PropertyChanged();
            }
        }
        public int ProductID
        {
            get => productID;
            set
            {
                productID = value;
                ControlFile_PropertyChanged();
            }
        }
        public List<TestStep> TestSteps { get => testSteps; set => testSteps = value; }

        public string FullDescription
        {
            get => fullDescription;
            set
            {
                fullDescription = value;
                ControlFile_PropertyChanged();
            }
        }
        public ControlFile(int controlFileID, long number, string type, string part, string version, int productID)
        {
            ControlFileID = controlFileID;
            Number = number;
            Type = type;
            Part = part;
            Version = version;
            ProductID = productID;
            UpdateFullDescription();
            TestSteps = new List<TestStep>();
        }

        public void UpdateFullDescription()
        {
            FullDescription = Number + "_" + Type + "_" + Part + "_" + Version;
        }

        public ControlFile Copy()
        {
            ControlFile controlFile = new ControlFile(this.ControlFileID, this.number, this.Type, this.Part, this.Version, this.ProductID);
            return controlFile;
        }

        public void BuildTestStepList()
        {
            if(TestSteps == null || TestSteps.Count == 0) TestSteps = MainWindow.database.testStepManager.GetTestStepList(this);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void ControlFile_PropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

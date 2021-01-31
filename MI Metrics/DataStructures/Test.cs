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
    public class Test : INotifyPropertyChanged
    {
        public int TestID
        {
            get => testID;
            set
            {
                testID = value;
                Test_PropertyChagned();
            }
        }
        private int testID;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                Test_PropertyChagned();
            }
        }
        private string name;

        public int ProductID
        {
            get => productID;
            set
            {
                productID = value;
                Test_PropertyChagned();
            }
        }
        private int productID;

        public ObservableCollection<TestStep> TestStepView { get => testStepView; set => testStepView = value; }
        private ObservableCollection<TestStep> testStepView;

        public Test(int testID, string name, int productID)
        {
            TestID = testID;
            Name = name;
            ProductID = productID;
            TestStepView = new ObservableCollection<TestStep>();
        }
        

        public Test Copy()
        {
            // Used for copying tests during control file management.
            return new Test(this.testID, this.Name, this.ProductID);
        }

        
        public event PropertyChangedEventHandler PropertyChanged;
        private void Test_PropertyChagned([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MI_Metrics
{
    public class Product : INotifyPropertyChanged
    {
        private int productID;
        private string name;
        private string imagePath;
        private List<Test> tests;

        public int ProductID { get => productID; set => productID = value; }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                Product_PropertyChanged();
            }
        }
        public string ImagePath { get => imagePath; set => imagePath = value; }

        public List<Test> Tests
        {
            get => tests;
            set
            {
                tests = value;
            }
        }
        
        public Product()
        {
            Tests = new List<Test>();
        }
        public Product(int productID, string name, string imagePath)
        {
            ProductID = productID;
            Name = name;
            ImagePath = @imagePath;
            Tests = new List<Test>();
        }

        public void CreateTestList()
        {
            this.Tests = MainWindow.database.testManager.RetrieveTests(this);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void Product_PropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}

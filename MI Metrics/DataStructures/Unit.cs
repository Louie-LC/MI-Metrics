using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace MI_Metrics
{
    public class Unit : INotifyPropertyChanged
    {
        private int unitID;
        private int productID;
        private long partNumber;
        private string description;
        
        public int UnitID { get => unitID; set => unitID = value; }
        public int ProductID
        {
            get => productID;
            set
            {
                productID = value;
                Unit_PropertyChagned();
            }
        }
        public long PartNumber
        {
            get => partNumber;
            set
            {
                partNumber = value;
                Unit_PropertyChagned();
            }
        }
        public string Description
        {
            get => description;
            set
            {
                description = value;
                Unit_PropertyChagned();
            }
        }

        public Unit(int unitID, long partNumber, string description, int productID)
        {
            UnitID = unitID;
            ProductID = productID;
            PartNumber = partNumber;
            Description = description;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void Unit_PropertyChagned([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

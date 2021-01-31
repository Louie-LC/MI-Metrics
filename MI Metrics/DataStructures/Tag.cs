using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public class Tag : INotifyPropertyChanged
    {
        private int tagID;
        private string description;
        private int productID;
        private List<Unit> applicableUnits;
        private bool applicableToUnit;

        public int TagID { get => tagID; set => tagID = value; }
        public string Description
        {
            get => description;
            set
            {
                description = value;
                Tag_PropertyChanged();
            }
        }
        public int ProductID { get => productID; set => productID = value; }
        public List<Unit> ApplicableUnits { get => applicableUnits; set => applicableUnits = value; }
        public bool ApplicableToUnit
        {
            get => applicableToUnit;
            set
            {
                applicableToUnit = value;
                Tag_PropertyChanged();
            }
        }

        public Tag(int tagID, string description, int productID)
        {
            TagID = tagID;
            Description = description;
            ProductID = productID;
            applicableToUnit = false;
        }

        public Tag(string description, int productID)
        {
            Description = description;
            ProductID = productID;
            applicableToUnit = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Tag_PropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

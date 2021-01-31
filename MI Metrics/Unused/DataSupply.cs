using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace MI_Metrics
{
    // A class for demonstrating how the INotifyPropertyChanged interface can be implimented.

    public class DataSupply : INotifyPropertyChanged
    {
        string displayText = "Here's this text";
        public string DisplayText
        {
            get
            {

                return displayText;
                
            }
            set
            {               
                displayText = value;
                Console.WriteLine("Value changed: " + displayText);
                NotifyPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

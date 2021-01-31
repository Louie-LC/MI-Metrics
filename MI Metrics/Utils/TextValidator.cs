using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MI_Metrics
{
    public class TextValidator
    {
        public static bool ValidatePastedText(DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!DatabaseHelper.ValidateCharacters(text))
                {
                    e.CancelCommand();
                    return false;
                }
            }
            else
            {
                e.CancelCommand();
                return false;
            }
            return true;
        }

        public static bool ValidatePastedLong(DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                long dummy;
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!Int64.TryParse(text, out dummy))
                {
                    e.CancelCommand();
                    return false;
                }
            }
            else
            {
                e.CancelCommand();
                return false;
            }
            return true;
        }

        public static bool ValidatePastedDouble(DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                double dummy;
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!Double.TryParse(text, out dummy))
                {
                    e.CancelCommand();
                    return false;
                }
            }
            else
            {
                e.CancelCommand();
                return false;
            }
            return true;
        }

        public static bool ValidatePastedInt(DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                int dummy;
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!Int32.TryParse(text, out dummy))
                {
                    e.CancelCommand();
                    return false;
                }
            }
            else
            {
                e.CancelCommand();
                return false;
            }
            return true;
        }
    }
}

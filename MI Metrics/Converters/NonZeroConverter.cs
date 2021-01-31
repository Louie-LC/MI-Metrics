using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MI_Metrics
{
    // https://stackoverflow.com/questions/4886988/wpf-trigger-that-would-work-if-the-value-is-equal-or-greater
    public class NonZeroConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new NonZeroConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intValue = (int)value;
            return intValue != 0;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
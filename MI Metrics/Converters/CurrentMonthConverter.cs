using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MI_Metrics
{
    public class CurrentMonthConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new CurrentMonthConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateTime = DateTime.Parse((string)value);
            return DateTime.Today.Month == dateTime.Month;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

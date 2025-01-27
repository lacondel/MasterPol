using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MasterPol.AppData.DbMethods
{
    internal class PhoneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string phone && !string.IsNullOrEmpty(phone))
            {
                return $"+7({phone.Substring(0, 3)}){phone.Substring(3, 3)}-{phone.Substring(6,2)}-{phone.Substring(8,2)}";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

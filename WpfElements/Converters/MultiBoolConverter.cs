using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WpfElements
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class MultiBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || !values.Any() || values.Any(b => b is not Boolean))
                return DependencyProperty.UnsetValue;

            if (parameter == null)
                return DependencyProperty.UnsetValue;

            bool result = false;
            
            switch (parameter.ToString())
            {
                case "XOR":
                    foreach (var value in values)
                    {
                        result |= (bool)value;
                    }
                    return !result;
            }

            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

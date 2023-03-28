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
    [ValueConversion(typeof(Enum), typeof(bool))]
    public class EnumEqualsParameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {




            //if (value == null || //!! это отдебажить
            //    !(parameter is string str) ||
            //    !Enum.IsDefined(value.GetType(), value))
            //{
            //    // make warning
            //    return DependencyProperty.UnsetValue; 
            //}
            //else 
            //    return (object)Enum.Parse(value.GetType(), str).Equals(value);


            //if (value == null || Enum.is)
            bool result = false;
            object outEnum;
            try
            {
                if (Enum.TryParse(value.GetType(), parameter.ToString(), out outEnum))
                    result = value.Equals(outEnum);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

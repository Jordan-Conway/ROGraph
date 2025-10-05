using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ROGraph.Models
{
    public class CoordinateConverter: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(values.Length != 2)
            {
                throw new ArgumentException("CoordinateConverter requires exactly two parameters");
            }

            return (((Guid, Guid))values[0], ((Guid, Guid))values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            object[] objects = new object[2];

            Tuple<object, object> valueTuple = (Tuple<object, object>)value;

            objects[0] = valueTuple.Item1;
            objects[1] = valueTuple.Item2;

            return objects;
        }
    }
}

using System;
using System.Windows.Data;

namespace TsuyobahaCounter
{
    public class CalcQuotientConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double answer = 0;
            if (values[0] is int firstValue)
            {
                if (values[1] is int secondValue)
                {
                    // ゼロ除算対策
                    if (secondValue != 0)
                    {
                        answer = (double)firstValue / secondValue;
                        answer *= 100;
                    }
                }
            }


            return answer.ToString("F3");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString().Split(' ');
        }
    }
}

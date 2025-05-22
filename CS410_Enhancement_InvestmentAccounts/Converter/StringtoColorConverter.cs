using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CS410_Enhancement_InvestmentAccounts.Converter
{
    /// <summary>
    /// StringtoColorConverter is a value converter that converts a string to a SolidColorBrush. This is used to
    /// give the validation message red or green depending on success.
    /// </summary>
    internal class StringtoColorConverter : IValueConverter
    {
        public SolidColorBrush RedBrush { get; set; }
        public SolidColorBrush GreenBrush { get; set; }

        public StringtoColorConverter()
        {
            RedBrush = new SolidColorBrush(Colors.Red);
            GreenBrush = new SolidColorBrush(Colors.Green);
        }

        /// <summary>
        /// Converts a string to a SolidColorBrush. This is used to give the validation message red or green depending on success.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>SolidcolorBrush</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return RedBrush;
            }
            if (value is string)
            {
                var s = value as string;
                if (s.Equals("Validation successful"))
                {
                    return GreenBrush;
                }
               
            }

            return RedBrush;
            
        }
        /// <summary>
        /// Unneeded but part of interface, only used for two way conversion binding.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

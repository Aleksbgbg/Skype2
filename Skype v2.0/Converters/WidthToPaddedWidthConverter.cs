namespace Skype2.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(double), typeof(double))]
    internal class WidthToPaddedWidthConverter : IValueConverter
    {
        public static WidthToPaddedWidthConverter Default { get; } = new WidthToPaddedWidthConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value - 60.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
            return (double)value + (double)parameter;
        }
    }
}
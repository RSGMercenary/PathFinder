using System;
using System.Globalization;
using System.Windows.Data;

namespace PathFinder.Converters
{
	public class WidthConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)values[0] ? (double)values[1] : double.NaN;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
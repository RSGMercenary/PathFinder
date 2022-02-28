using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PathFinder.Converters
{
	public class BoolWrapConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)value ? TextWrapping.Wrap : TextWrapping.NoWrap;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
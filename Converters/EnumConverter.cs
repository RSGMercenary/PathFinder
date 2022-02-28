using PathFinder.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PathFinder.Converters
{
	public class EnumConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value as Enum)?.GetDescriptionOrToString() ?? value?.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
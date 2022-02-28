using PathFinder.Processors.Abstracts;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PathFinder.Converters
{
	public class ProcessorVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var parameters = ((string)parameter).Split('|');
			var type = Type.GetType(parameters[0]);
			bool self = parameters.Length > 1 && bool.Parse(parameters[1]);
			return (self ? type.IsInstanceOfType(value) : (value as IProcessor).Implements(type)) ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PathFinder.Converters
{
	public class VisibilityMarginConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (Visibility)value == Visibility.Visible
				? new Thickness(0, 0, 0, SystemParameters.HorizontalScrollBarButtonWidth)
				: new Thickness(0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
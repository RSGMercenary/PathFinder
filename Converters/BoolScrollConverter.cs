﻿using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace PathFinder.Converters
{
	public class BoolScrollConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)value ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Auto;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
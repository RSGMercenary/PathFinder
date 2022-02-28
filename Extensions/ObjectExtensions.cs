using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace PathFinder.Extensions
{
	public static class ObjectExtensions
	{
		public static string GetDescriptionOrToString(this object value)
		{
			return value.GetType().GetField(value.ToString()).GetAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
		}

		public static T GetAttribute<T>(this MemberInfo value) where T : Attribute
		{
			return value.GetCustomAttributes(false).OfType<T>().SingleOrDefault();
		}
	}
}
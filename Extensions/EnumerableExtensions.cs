using System;
using System.Collections.Generic;

namespace PathFinder.Extensions
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> Enumerate<T>(this T item)
		{
			yield return item;
		}

		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> items, Action<T> method)
		{
			foreach(var item in items)
				method.Invoke(item);
			return items;
		}

		public static T Pop<T>(this IList<T> items)
		{
			var item = items[items.Count - 1];
			items.RemoveAt(items.Count - 1);
			return item;
		}
	}
}
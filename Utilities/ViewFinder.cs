using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace PathFinder.Utilities
{
	public static class ViewFinder
	{
		public static T GetAncestor<T>(DependencyObject item, string name = null)
			where T : DependencyObject
		{
			do
			{
				item = VisualTreeHelper.GetParent(item);
				if(IsObject<T>(item, name))
					return item as T;
			}
			while(item != null);
			return null;
		}

		public static T GetDescendant<T>(DependencyObject item, string name = null)
			where T : DependencyObject
		{
			int childCount = VisualTreeHelper.GetChildrenCount(item);
			for(int index = 0; index < childCount; ++index)
			{
				var descendant = VisualTreeHelper.GetChild(item, index);
				if(IsObject<T>(descendant, name))
					return descendant as T;
				descendant = GetDescendant<T>(descendant, name);
				if(descendant != null)
					return descendant as T;
			}
			return null;
		}

		public static T GetSelfOrDescendant<T>(DependencyObject item, string name = null)
			where T : DependencyObject
		{
			if(IsObject<T>(item, name))
				return item as T;
			return GetDescendant<T>(item, name);
		}

		public static IEnumerable<T> GetDescendants<T>(DependencyObject item, string name = null)
			where T : DependencyObject
		{
			int childCount = VisualTreeHelper.GetChildrenCount(item);
			for(int index = 0; index < childCount; ++index)
			{
				var child = VisualTreeHelper.GetChild(item, index);
				if(IsObject<T>(child, name))
					yield return child as T;
				foreach(var descendant in GetDescendants<T>(child, name))
					yield return descendant;
			}
		}

		public static D GetAncestorDescendant<A, D>(DependencyObject item, string nameA = null, string nameD = null)
			where A : DependencyObject
			where D : DependencyObject
		{
			var ancestor = GetAncestor<A>(item, nameA);
			return ancestor != null ? GetDescendant<D>(ancestor, nameD) : null;
		}

		public static bool IsObject<T>(DependencyObject item, string name = null)
			where T : DependencyObject
		{
			if(item is T)
			{
				if(!string.IsNullOrWhiteSpace(name))
				{
					if(item is FrameworkElement element && element.Name == name)
						return true;
				}
				else
					return true;
			}
			return false;
		}
	}
}
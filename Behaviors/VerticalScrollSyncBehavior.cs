using PathFinder.Utilities;
using System.Windows;
using System.Windows.Controls;

namespace PathFinder.Behaviors
{
	public static class VerticalScrollSyncBehavior
	{
		public static readonly DependencyProperty VerticalScrollSyncProperty =
			DependencyProperty.RegisterAttached(
				"VerticalScrollSync",
				typeof(Control),
				typeof(VerticalScrollSyncBehavior),
				new PropertyMetadata(null, VerticalScrollSyncChanged)
				);

		private static void VerticalScrollSyncChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var control = obj as Control;

			if(args.NewValue != null)
			{
				control.Loaded += Loaded;
				if(control.IsLoaded)
					Loaded(control);
			}
		}

		private static void Loaded(object obj, RoutedEventArgs args = null)
		{
			var element = obj as FrameworkElement;
			element.Loaded -= Loaded;

			var scrollViewer = ViewFinder.GetSelfOrDescendant<ScrollViewer>(element);
			if(scrollViewer == null)
				return;

			scrollViewer.Unloaded += Unloaded;
			scrollViewer.ScrollChanged += ScrollChanged;
		}

		private static void Unloaded(object obj, RoutedEventArgs args)
		{
			var scroll = obj as ScrollViewer;
			scroll.Unloaded -= Unloaded;
			scroll.ScrollChanged -= ScrollChanged;
		}

		private static void ScrollChanged(object obj, ScrollChangedEventArgs e)
		{
			var scroll1 = obj as ScrollViewer;
			var scroll2 = ViewFinder.GetSelfOrDescendant<ScrollViewer>(GetSelfOrParentProperty(scroll1));
			scroll2.ScrollToVerticalOffset(scroll1.VerticalOffset);
		}

		private static DependencyObject GetSelfOrParentProperty(FrameworkElement element)
		{
			return (element.GetValue(VerticalScrollSyncProperty) ??
				element.TemplatedParent.GetValue(VerticalScrollSyncProperty)) as DependencyObject;
		}

		public static void SetVerticalScrollSync(UIElement element, bool value) => element.SetValue(VerticalScrollSyncProperty, value);

		public static bool GetVerticalScrollSync(UIElement element) => (bool)element.GetValue(VerticalScrollSyncProperty);
	}
}
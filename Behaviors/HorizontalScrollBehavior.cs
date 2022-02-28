using PathFinder.Utilities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PathFinder.Behaviors
{
	public static class HorizontalScrollBehavior
	{
		public static readonly DependencyProperty HorizontalScrollProperty =
			DependencyProperty.RegisterAttached(
				"HorizontalScroll",
				typeof(bool),
				typeof(HorizontalScrollBehavior),
				new PropertyMetadata(false, HorizontalScrollChanged)
				);

		private static void HorizontalScrollChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var element = obj as UIElement;

			if((bool)args.NewValue)
				element.PreviewMouseWheel += OnPreviewMouseWheel;
			else
				element.PreviewMouseWheel -= OnPreviewMouseWheel;
		}

		private static void OnPreviewMouseWheel(object obj, MouseWheelEventArgs args)
		{
			var scrollViewer = ViewFinder.GetSelfOrDescendant<ScrollViewer>(obj as DependencyObject);

			if(scrollViewer == null)
				return;

			if(Keyboard.Modifiers != ModifierKeys.Shift)
				return;

			if(args.Delta < 0)
				scrollViewer.LineRight();
			else
				scrollViewer.LineLeft();

			args.Handled = true;
		}

		public static void SetHorizontalScroll(UIElement element, bool value) => element.SetValue(HorizontalScrollProperty, value);

		public static bool GetHorizontalScroll(UIElement element) => (bool)element.GetValue(HorizontalScrollProperty);
	}
}
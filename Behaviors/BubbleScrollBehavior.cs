using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PathFinder.Behaviors
{
	public static class BubbleScrollBehavior
	{
		public static readonly DependencyProperty BubbleScrollProperty =
			DependencyProperty.RegisterAttached(
				"BubbleScroll",
				typeof(bool),
				typeof(BubbleScrollBehavior),
				new PropertyMetadata(false, BubbleScrollChanged)
				);

		private static void BubbleScrollChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var element = obj as UIElement;

			if((bool)args.NewValue)
				element.PreviewMouseWheel += OnPreviewMouseWheel;
			else
				element.PreviewMouseWheel -= OnPreviewMouseWheel;
		}

		private static void OnPreviewMouseWheel(object obj, MouseWheelEventArgs args)
		{
			args.Handled = true;
			var arg = new MouseWheelEventArgs(args.MouseDevice, args.Timestamp, args.Delta);
			arg.RoutedEvent = UIElement.MouseWheelEvent;
			arg.Source = obj;
			var parent = ((Control)obj).Parent as UIElement;
			parent.RaiseEvent(arg);
		}

		public static void SetBubbleScroll(UIElement element, bool value) => element.SetValue(BubbleScrollProperty, value);

		public static bool GetBubbleScroll(UIElement element) => (bool)element.GetValue(BubbleScrollProperty);
	}
}
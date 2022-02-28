using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PathFinder.Behaviors
{
	public static class SelectTextBehavior
	{
		public static readonly DependencyProperty SelectTextProperty =
			DependencyProperty.RegisterAttached(
				"SelectText",
				typeof(bool),
				typeof(SelectTextBehavior),
				new PropertyMetadata(false, SelectTextChanged)
				);

		private static void SelectTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var box = obj as TextBox;

			if((bool)args.NewValue)
			{
				box.PreviewMouseLeftButtonDown += OnPreviewMouseDown;
				box.GotFocus += OnGotFocus;
			}
			else
			{
				box.PreviewMouseLeftButtonDown -= OnPreviewMouseDown;
				box.GotFocus -= OnGotFocus;
			}
		}

		private static void OnPreviewMouseDown(object obj, MouseButtonEventArgs args)
		{
			if(obj is TextBox box && !box.IsKeyboardFocusWithin)
			{
				if(args.OriginalSource.GetType().Name == "TextBoxView")
				{
					args.Handled = true;
					box.Focus();
				}
			}
		}

		private static void OnGotFocus(object obj, RoutedEventArgs args)
		{
			if(obj is TextBox box)
				box.SelectAll();
		}

		public static void SetSelectText(UIElement element, bool value) => element.SetValue(SelectTextProperty, value);

		public static bool GetSelectText(UIElement element) => (bool)element.GetValue(SelectTextProperty);
	}
}
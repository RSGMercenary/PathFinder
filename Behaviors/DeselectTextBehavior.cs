using System.Windows;
using System.Windows.Controls;

namespace PathFinder.Behaviors
{
	public static class DeselectTextBehavior
	{
		public static readonly DependencyProperty DeselectTextProperty =
			DependencyProperty.RegisterAttached(
				"DeselectText",
				typeof(bool),
				typeof(DeselectTextBehavior),
				new PropertyMetadata(false, DeselectTextChanged)
				);

		private static void DeselectTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var element = obj as UIElement;

			if((bool)args.NewValue)
				element.GotFocus += GotFocusChanged;
			else
				element.GotFocus -= GotFocusChanged;
		}

		private static void GotFocusChanged(object obj, RoutedEventArgs args)
		{
			var combo = obj as ComboBox;
			var text = combo.Template.FindName("PART_EditableTextBox", combo) as TextBox;
			text.SelectionStart = text.Text.Length;
		}

		public static void SetDeselectText(UIElement element, bool value) => element.SetValue(DeselectTextProperty, value);

		public static bool GetDeselectText(UIElement element) => (bool)element.GetValue(DeselectTextProperty);
	}
}
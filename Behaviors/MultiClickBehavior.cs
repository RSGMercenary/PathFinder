using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace PathFinder.Behaviors
{
	public static class MultiClickBehavior
	{
		public static readonly DependencyProperty MultiClickProperty =
			DependencyProperty.RegisterAttached(
				"MultiClick",
				typeof(bool),
				typeof(MultiClickBehavior),
				new PropertyMetadata(false, MultiClickChanged)
				);

		private static void MultiClickChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var element = obj as UIElement;

			if((bool)args.NewValue)
				element.PreviewMouseDown += OnPreviewMouseDown;
			else
				element.PreviewMouseDown -= OnPreviewMouseDown;
		}

		private static void OnPreviewMouseDown(object obj, MouseButtonEventArgs args)
		{
			if(obj is TextBoxBase text)
			{
				if(args.ClickCount == 3 && obj is TextBox box)
				{
					var line = box.GetLineIndexFromCharacterIndex(box.CaretIndex);
					var index = box.GetCharacterIndexFromLineIndex(line);
					var length = 0;
					while(index + length < box.Text.Length)
					{
						if(box.Text[index + length++] != '\n')
							continue;
						//length++ has moved us 1 past the newline.
						//Move back 2 to the char before the newline.
						length -= 2;
						break;
					}
					box.Select(index, length);
					var match = BehaviorUtility.GetSelectionLines(box).First();
					box.CaretIndex = index + match.Index;
					box.Select(box.CaretIndex, match.Length);
				}
				else if(args.ClickCount >= 3)
					text.SelectAll();
			}
		}

		public static void SetMultiClick(UIElement element, bool value) => element.SetValue(MultiClickProperty, value);

		public static bool GetMultiClick(UIElement element) => (bool)element.GetValue(MultiClickProperty);
	}
}
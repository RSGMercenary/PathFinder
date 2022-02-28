using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace PathFinder.Behaviors
{
	public static class ScrollSelectBehavior
	{
		public static readonly DependencyProperty ScrollSelectProperty =
			DependencyProperty.RegisterAttached(
				"ScrollSelect",
				typeof(Match),
				typeof(ScrollSelectBehavior),
				new PropertyMetadata(null, ScrollSelectChanged, Coerce)
				);

		private static void ScrollSelectChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var box = obj as TextBox;
			var match = (Match)args.NewValue;

			if(match == null || match.Index > box.Text.Length)
				return;

			box.CaretIndex = match.Index;
			box.Select(match.Index, match.Length);
			var rect = GetSelectionRect(box);

			if(rect.IsEmpty)
				return;

			CenterSelection(box, rect);
		}

		private static Rect GetSelectionRect(TextBox box)
		{
			var selection = box.GetRectFromCharacterIndex(box.SelectionStart);

			if(selection.IsEmpty)
				return selection;

			foreach(var match in BehaviorUtility.GetSelectionLines(box))
			{
				var side = box.GetRectFromCharacterIndex(box.SelectionStart + match.Index);
				if(side.Left < selection.Left)
					selection.X = side.Left;
				side = box.GetRectFromCharacterIndex(box.SelectionStart + match.Index + match.Length - 1);
				if(side.Right > selection.Right)
					selection.Width = side.Right - selection.Left;
			}

			var bottom = box.GetRectFromCharacterIndex(box.SelectionStart + box.SelectionLength - 1);
			selection.Height = bottom.Bottom - selection.Top;
			return selection;
		}

		private static void CenterSelection(TextBox box, Rect rect)
		{
			if(rect.Height > box.ViewportHeight)
				box.ScrollToVerticalOffset(rect.Top + box.VerticalOffset - box.Padding.Top);
			else
				box.ScrollToVerticalOffset((rect.Top + rect.Bottom - box.ViewportHeight) / 2 + box.VerticalOffset);

			if(rect.Width > box.ViewportWidth)
				box.ScrollToVerticalOffset(rect.Left + box.HorizontalOffset - box.Padding.Left);
			else
				box.ScrollToVerticalOffset((rect.Left + rect.Right - box.ViewportWidth) / 2 + box.HorizontalOffset);

			box.Focus();
		}

		private static object Coerce(DependencyObject obj, object value)
		{
			if(value != null && obj.GetValue(ScrollSelectProperty) == value)
			{
				obj.SetCurrentValue(ScrollSelectProperty, null);
				obj.SetCurrentValue(ScrollSelectProperty, value);
			}
			return value;
		}

		public static void SetScrollSelect(UIElement element, Match value) => element.SetValue(ScrollSelectProperty, value);

		public static Match GetScrollSelect(UIElement element) => (Match)element.GetValue(ScrollSelectProperty);
	}
}
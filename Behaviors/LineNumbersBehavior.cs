using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace PathFinder.Behaviors
{
	public static class LineNumbersBehavior
	{
		public static readonly DependencyProperty LineNumbersProperty =
			DependencyProperty.RegisterAttached(
				"LineNumbers",
				typeof(TextBlock),
				typeof(LineNumbersBehavior),
				new PropertyMetadata(null, LineNumbersChanged)
				);

		private static void LineNumbersChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var box = obj as TextBox;

			if(args.NewValue != null)
			{
				box.TextChanged += TextChanged;
				box.SizeChanged += SizeChanged;
				box.Loaded += Loaded;
				box.Unloaded += Unloaded;
				if(box.IsLoaded)
					Loaded(box);
			}
			else
			{
				box.TextChanged -= TextChanged;
				box.SizeChanged -= SizeChanged;
				box.Loaded -= Loaded;
				box.Unloaded -= Unloaded;
			}
		}

		private static void Loaded(object sender, RoutedEventArgs e = null)
		{
			DependencyPropertyDescriptor
				.FromProperty(TextBox.TextWrappingProperty, typeof(TextBox))
				.AddValueChanged(sender, ChangeLineNumbers);
		}

		private static void Unloaded(object sender, RoutedEventArgs e)
		{
			DependencyPropertyDescriptor
				.FromProperty(TextBox.TextWrappingProperty, typeof(TextBox))
				.RemoveValueChanged(sender, ChangeLineNumbers);
		}

		private static void TextChanged(object sender, TextChangedEventArgs args) => ChangeLineNumbers(sender as TextBox);

		private static void SizeChanged(object sender, SizeChangedEventArgs args)
		{
			if(sender is TextBox box)
			{
				if(!args.HeightChanged && args.WidthChanged && box.TextWrapping == TextWrapping.NoWrap)
					return;
				ChangeLineNumbers(box);
			}
		}

		private static void ChangeLineNumbers(object sender, EventArgs args) => ChangeLineNumbers(sender as TextBox);

		private static void ChangeLineNumbers(TextBox box)
		{
			var numbers = (TextBlock)box.GetValue(LineNumbersProperty);

			//These seem to happen when closing a tab. View might be destroyed already.
			if(numbers == null || box.LineCount < 1)
				return;

			numbers.Text = box.Text.Length > 0 ? GetNumbers(box) : "";
		}

		private static string GetNumbers(TextBox box)
		{
			List<int> starts;
			List<int> ends;

			if(box.TextWrapping == TextWrapping.NoWrap)
			{
				starts = Enumerable.Range(0, box.LineCount).ToList();
				ends = starts;
			}
			else
			{
				starts = Regex.Matches(box.Text, "^", RegexOptions.Multiline).Cast<Match>().Select(m => box.GetLineIndexFromCharacterIndex(m.Index)).ToList();
				ends = Regex.Matches(box.Text, "$", RegexOptions.Multiline).Cast<Match>().Select(m => box.GetLineIndexFromCharacterIndex(m.Index)).ToList();
			}

			var builder = new StringBuilder();

			for(int index = 0; index < starts.Count; ++index)
			{
				builder.Append((index + 1).ToString());
				for(var line = starts[index]; line < ends[index]; ++line)
					builder.AppendLine();
				if(index < starts.Count - 1)
					builder.AppendLine();
			}

			return builder.ToString();
		}

		public static void SetLineNumbers(UIElement element, bool value) => element.SetValue(LineNumbersProperty, value);

		public static bool GetLineNumbers(UIElement element) => (bool)element.GetValue(LineNumbersProperty);
	}
}
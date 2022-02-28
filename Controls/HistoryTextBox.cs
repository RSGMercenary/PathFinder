using PathFinder.Extensions;
using PathFinder.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PathFinder.Controls
{
	public class HistoryTextBox : TextBox
	{
		#region Scroll
		public static readonly DependencyProperty ComputedHorizontalScrollVisibilityProperty = DependencyProperty.Register("ComputedHorizontalScrollVisibility", typeof(Visibility), typeof(HistoryTextBox));

		public Visibility ScrollVisibility
		{
			get => (Visibility)GetValue(ComputedHorizontalScrollVisibilityProperty);
			set => SetValue(ComputedHorizontalScrollVisibilityProperty, value);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			var scrollViewer = ViewFinder.GetDescendant<ScrollViewer>(this);
			scrollViewer.ScrollChanged += OnScrollChanged;
		}

		private void OnScrollChanged(object sender, ScrollChangedEventArgs args)
		{
			SetValue(ComputedHorizontalScrollVisibilityProperty, (sender as ScrollViewer).ComputedHorizontalScrollBarVisibility);
		}
		#endregion

		#region History
		private struct History
		{
			public string Text { get; }
			public int Index { get; }

			public History(string text, int index)
			{
				Text = text;
				Index = index;
			}
		}

		private bool IgnoreChange = false;
		private const int HistoryLimit = 2048;
		private readonly List<History> Undos = new List<History>();
		private readonly Stack<History> Redos = new Stack<History>();

		protected override void OnTextChanged(TextChangedEventArgs e)
		{
			base.OnTextChanged(e);
			if(!IgnoreChange)
			{
				Redos.Clear();
				Undos.Add(new History(Text, CaretIndex));
				if(Undos.Count > HistoryLimit)
					Undos.RemoveAt(0);
			}
		}

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			if(e.Key == Key.Z && e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control))
				UndoHistory();
			if(e.Key == Key.Y && e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control))
				RedoHistory();
			base.OnPreviewKeyDown(e);
		}

		private void UndoHistory()
		{
			if(Undos.Count > 1)
			{
				Redos.Push(Undos.Pop());
				SetHistory();
			}
		}

		private void RedoHistory()
		{
			if(Redos.Count > 0)
			{
				Undos.Add(Redos.Pop());
				SetHistory();
			}
		}

		private void SetHistory()
		{
			var history = Undos.Last();

			IgnoreChange = true;
			Text = history.Text;
			CaretIndex = history.Index;
			ScrollToLine(GetLineIndexFromCharacterIndex(CaretIndex));
			IgnoreChange = false;
		}
		#endregion
	}
}
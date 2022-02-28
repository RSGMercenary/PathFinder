using PathFinder.Models;
using PathFinder.Utilities;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PathFinder
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = MainModelProvider.Load();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			MainModelProvider.Save(DataContext as MainModel);
			base.OnClosing(e);
		}

		#region Window Controls
		private void Move(object sender, MouseButtonEventArgs e) => DragMove();

		private void Minimize(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

		private void Maximize(object sender, RoutedEventArgs e) => ToggleMaximize();

		private void Maximize(object sender, MouseButtonEventArgs e) => ToggleMaximize();

		private void ToggleMaximize()
		{
			if(WindowState == WindowState.Normal)
			{
				MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
				BorderThickness = new Thickness(6);
				WindowState = WindowState.Maximized;
			}
			else
			{
				MaxHeight = double.PositiveInfinity;
				BorderThickness = new Thickness(0);
				WindowState = WindowState.Normal;
			}
		}

		private void Exit(object sender, RoutedEventArgs e) => Close();
		#endregion

		#region Pages
		private void AddPage(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
			(DataContext as MainModel).AddPage();
		}

		private void RemovePage(object sender, RoutedEventArgs e)
		{
			var tab = ViewFinder.GetAncestor<TabItem>(sender as DependencyObject);
			(DataContext as MainModel).RemovePage(tab.Content as PageModel);
		}

		private void ClonePage(object sender, RoutedEventArgs e)
		{
			var tab = ViewFinder.GetAncestor<TabItem>(sender as DependencyObject);
			(DataContext as MainModel).ClonePage(tab.Content as PageModel);
		}
		#endregion
	}
}
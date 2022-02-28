using PathFinder.Extensions;
using PathFinder.Models;
using PathFinder.Processors.Datas;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PathFinder
{
	public partial class App : Application
	{
		private void StartUp(object sender, StartupEventArgs args)
		{
			SetupExceptionHandling();
			CloseDuplicateProcesses();

			var window = new MainWindow();
			if(args.Args.Length == 1)
			{
				var model = window.DataContext as MainModel;
				var page = new PageModel();
				page.Input = File.ReadAllText(args.Args[0]);
				page.ProcessType = GetProcessType(args.Args[0]);
				model.AddPage(page);
			}
			window.Show();
		}

		private void SetupExceptionHandling()
		{
			AppDomain.CurrentDomain.UnhandledException += (s, e) =>
			{
				LogUnhandleException((Exception)e.ExceptionObject);
			};

			Dispatcher.UnhandledException += (s, e) =>
			{
				LogUnhandleException(e.Exception);
				//e.Handled = true;
			};

			TaskScheduler.UnobservedTaskException += (s, e) =>
			{
				LogUnhandleException(e.Exception);
				//e.SetObserved();
			};
		}

		private void LogUnhandleException(Exception exception)
		{
			File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CrashLog.txt"), exception.ToString());
		}

		private void CloseDuplicateProcesses()
		{
			var process = Process.GetCurrentProcess();
			Process.GetProcessesByName(process.ProcessName)
				.Where(p => p.Id != process.Id)
				.ForEach(p => p.CloseMainWindow());
		}

		private ProcessType GetProcessType(string path)
		{
			switch(Path.GetExtension(path).ToLower())
			{
				case ".html":
				case ".xml":
					return ProcessType.SearchXml;
				case ".json":
					return ProcessType.SearchJson;
				default:
					return ProcessType.Search;
			}
		}
	}
}
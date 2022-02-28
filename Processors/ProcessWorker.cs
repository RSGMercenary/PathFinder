using PathFinder.Processors.Datas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Threading;

namespace PathFinder.Processors
{
	public class ProcessWorker
	{
		private readonly BackgroundWorker Worker = new BackgroundWorker();
		private readonly DispatcherTimer Timer = new DispatcherTimer();
		private readonly Action Tick;
		private readonly Func<(int, IEnumerable<ProcessResult>)> Work;
		private readonly Action<int, ProcessResult> Progress;
		private bool QueueWork = false;

		public ProcessWorker(Action tick, Func<(int, IEnumerable<ProcessResult>)> work, Action<int, ProcessResult> progress)
		{
			Tick = tick;
			Work = work;
			Progress = progress;

			Timer.Tick += OnTick;
			Timer.Stop();

			Worker.WorkerSupportsCancellation = true;
			Worker.WorkerReportsProgress = true;
			Worker.DoWork += OnWork;
			Worker.ProgressChanged += OnProgress;
			Worker.RunWorkerCompleted += OnComplete;
		}

		private void OnTick(object sender, EventArgs e)
		{
			((DispatcherTimer)sender).Stop();
			Tick.Invoke();
			if(Worker.IsBusy)
				QueueWork = true;
			else
				Worker.RunWorkerAsync();
		}

		private void OnWork(object sender, DoWorkEventArgs args)
		{
			var values = Work.Invoke();
			var index = 0;
			foreach(var output in values.Item2)
				Worker.ReportProgress(++index * 100 / values.Item1, output);
		}

		private void OnProgress(object sender, ProgressChangedEventArgs args)
		{
			if(((BackgroundWorker)sender).CancellationPending)
				return;
			Progress.Invoke(args.ProgressPercentage, (ProcessResult)args.UserState);
		}

		private void OnComplete(object sender, RunWorkerCompletedEventArgs args)
		{
			if(QueueWork)
			{
				QueueWork = false;
				Worker.RunWorkerAsync();
			}
		}

		public void Start() => Start(0);

		public void Start(double delay)
		{
			Stop();
			Timer.Interval = TimeSpan.FromSeconds(delay);
			Timer.Start();
		}

		public void Stop()
		{
			Worker.CancelAsync();
			Timer.Stop();
		}
	}
}
using System;
using System.Windows.Input;

namespace PathFinder.Commands
{
	public class Command<T> : ICommand
	{
#pragma warning disable 0067
		public event EventHandler CanExecuteChanged;
#pragma warning restore 0067
		private readonly Action<T> Method;

		public Command(Action<T> method) => Method = method;

		public bool CanExecute(object parameter) => true;

		public void Execute(object parameter) => Method.Invoke((T)Convert.ChangeType(parameter, typeof(T)));
	}

	public class Command : Command<object>
	{
		public Command(Action method) : base(p => method.Invoke()) { }
	}
}
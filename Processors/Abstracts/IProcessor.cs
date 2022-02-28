using PathFinder.Models;
using PathFinder.Processors.Datas;
using System;
using System.Collections.Generic;

namespace PathFinder.Processors.Abstracts
{
	public interface IProcessor
	{
		ProcessType ProcessType { get; }

		int Priority { get; }

		bool Implements(Type type, bool current);

		bool Implements(Type type);

		bool Implements<T>(bool current) where T : IProcessor;

		bool Implements<T>() where T : IProcessor;

		IEnumerable<ProcessResult> Process(IPageModel state);

		string Reinput(IEnumerable<string> outputs);
	}
}
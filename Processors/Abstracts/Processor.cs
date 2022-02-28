using PathFinder.Models;
using PathFinder.Processors.Datas;
using PathFinder.Processors.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PathFinder.Processors.Abstracts
{
	public abstract class Processor : IProcessor
	{
		public abstract ProcessType ProcessType { get; }

		public virtual int Priority { get; } = 0;

		public IEnumerable<ProcessResult> Process(IPageModel state)
		{
			var result = ValidateValues(state.Input, state.Parameter);
			if(result == ProcessPhase.Outputs)
				return Processing(state);
			throw new ProcessorException(result);
		}

		private ProcessPhase ValidateValues(string input, string parameter)
		{
			if(string.IsNullOrWhiteSpace(input) && string.IsNullOrWhiteSpace(parameter))
				return ProcessPhase.Idle;
			if(!string.IsNullOrWhiteSpace(input) && string.IsNullOrWhiteSpace(parameter))
			{
				if(Implements<IParameterProcessor>())
					return ProcessPhase.Parameters;
				else
					return ProcessPhase.Outputs;
			}
			if(string.IsNullOrWhiteSpace(input) && !string.IsNullOrWhiteSpace(parameter))
			{
				if(Implements<IParameterProcessor>())
					return ProcessPhase.Input;
				else
					return ProcessPhase.Idle;
			}
			return ProcessPhase.Outputs;
		}

		protected abstract IEnumerable<ProcessResult> Processing(IPageModel state);

		public virtual bool Implements(Type type, bool current) => type.IsAssignableFrom(GetType());

		public bool Implements(Type type) => Implements(type, false);

		public bool Implements<T>(bool current) where T : IProcessor => Implements(typeof(T), current);

		public bool Implements<T>() where T : IProcessor => Implements(typeof(T));

		public virtual string Reinput(IEnumerable<string> outputs) => outputs.Aggregate((o1, o2) => o1 + Environment.NewLine + o2);
	}
}
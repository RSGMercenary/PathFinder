using PathFinder.Models;
using PathFinder.Processors.Datas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PathFinder.Processors.Abstracts
{
	public abstract class AutoProcessor<T> : Processor, IAutoProcessor where T : IProcessor
	{
		private IEnumerable<T> Processors { get; }
		private T Processor { get; set; }

		public AutoProcessor(IEnumerable<IProcessor> processors)
		{
			Processors = processors.OfType<T>().OrderBy(p => p.Priority);
		}

		public override bool Implements(Type type, bool current)
		{
			if(current)
				return Processor?.Implements(type) ?? false;
			else
			{
				foreach(var processor in Processors)
				{
					if(processor.Implements(type))
						return true;
				}
				return false;
			}
		}

		protected override IEnumerable<ProcessResult> Processing(IPageModel state)
		{
			Processor = default;
			foreach(var processor in Processors)
			{
				try
				{
					var results = processor.Process(state);
					Processor = processor;
					return results;
				}
				catch { }
			}
			throw new ProcessorException(ProcessPhase.Processor);
		}

		public override string Reinput(IEnumerable<string> outputs)
		{
			return Processor?.Reinput(outputs) ?? base.Reinput(outputs);
		}
	}
}
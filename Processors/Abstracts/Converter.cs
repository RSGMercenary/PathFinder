using PathFinder.Extensions;
using PathFinder.Models;
using PathFinder.Processors.Datas;
using System.Collections.Generic;

namespace PathFinder.Processors.Abstracts
{
	public abstract class Converter : Processor, IConverter
	{
		protected override IEnumerable<ProcessResult> Processing(IPageModel state)
		{
			try
			{
				return new ProcessResult(state.Convert ? ConvertTo(state.Input) : ConvertFrom(state.Input)).Enumerate();
			}
			catch { throw new ProcessorException(ProcessPhase.Input); };
		}

		protected abstract string ConvertTo(string value);

		protected abstract string ConvertFrom(string value);
	}
}
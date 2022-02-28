using PathFinder.Models;
using PathFinder.Processors.Abstracts;
using PathFinder.Processors.Datas;
using PathFinder.Processors.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PathFinder.Processors.Searchers
{
	public class DelimiterSearcher : Searcher, IDelimiterProcessor
	{
		public override ProcessType ProcessType => ProcessType.SearchDelimiter;

		public override int Priority => 4;

		protected override IEnumerable<ProcessResult> Processing(IPageModel state)
		{
			try
			{
				var parameters = state.Parameters;
				var parameter1 = parameters.ElementAt(0);
				var parameter2 = parameters.ElementAt(1);
				var regex = new Regex($@"(?<={Regex.Escape(parameter1)})[\s\S]+?(?={Regex.Escape(parameter2)})");
				try
				{
					return regex.Matches(state.Input)
						.Cast<Match>()
						.Select(m => new ProcessResult(
							m.Value,
							parameter1 + m.Value + parameter2,
							m));
				}
				catch { throw new ProcessorException(ProcessPhase.Input); }
			}
			catch { throw new ProcessorException(ProcessPhase.Parameters); }
		}
	}
}
using PathFinder.Models;
using PathFinder.Processors.Abstracts;
using PathFinder.Processors.Datas;
using PathFinder.Processors.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PathFinder.Processors.Searchers
{
	public class TextSearcher : Searcher, ICaseSensitiveProcessor, IDelimiterProcessor
	{
		public override ProcessType ProcessType => ProcessType.SearchText;

		public override int Priority => 3;

		protected override IEnumerable<ProcessResult> Processing(IPageModel state)
		{
			try
			{
				var regexes = state.Parameters.Select(p => new Regex(Regex.Escape(p), ProcessorUtility.CaseSensitive(state.CaseSensitive)));
				try
				{
					return regexes.SelectMany(r => r.Matches(state.Input)
						.Cast<Match>()
						.Select(m => new ProcessResult(
							m.Value,
							"",
							m)));
				}
				catch { throw new ProcessorException(ProcessPhase.Input); }
			}
			catch { throw new ProcessorException(ProcessPhase.Parameters); }
		}
	}
}
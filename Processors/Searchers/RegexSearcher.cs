using PathFinder.Models;
using PathFinder.Processors.Abstracts;
using PathFinder.Processors.Datas;
using PathFinder.Processors.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PathFinder.Processors.Searchers
{
	public class RegexSearcher : Searcher, ICaseSensitiveProcessor, IInfoProcessor
	{
		public override ProcessType ProcessType => ProcessType.SearchRegex;

		public override int Priority => int.MaxValue;

		public string InfoUrl => "https://www.regex101.com/";

		protected override IEnumerable<ProcessResult> Processing(IPageModel state)
		{
			try
			{
				var regex = new Regex(state.Parameter, RegexOptions.Multiline | ProcessorUtility.CaseSensitive(state.CaseSensitive));
				try
				{
					return regex.Matches(state.Input)
						.Cast<Match>()
						.Select(m => new ProcessResult(
							m.Value,
							"",
							m));
				}
				catch { throw new ProcessorException(ProcessPhase.Input); }
			}
			catch { throw new ProcessorException(ProcessPhase.Parameters); }
		}
	}
}
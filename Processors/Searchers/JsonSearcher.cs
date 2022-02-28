using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PathFinder.Models;
using PathFinder.Processors.Abstracts;
using PathFinder.Processors.Datas;
using PathFinder.Processors.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PathFinder.Processors.Searchers
{
	public class JsonSearcher : Searcher, IFormatterProcessor, IInfoProcessor
	{
		public override ProcessType ProcessType => ProcessType.SearchJson;

		public override int Priority => 2;

		public string InfoUrl => "https://www.newtonsoft.com/json/help/html/Introduction.htm";

		protected override IEnumerable<ProcessResult> Processing(IPageModel state)
		{
			try
			{
				var json = JToken.Parse(state.Input);
				try
				{
					return json.SelectTokens(state.Parameter).Select(j => new ProcessResult(Format(j)));
				}
				catch { throw new ProcessorException(ProcessPhase.Parameters); }
			}
			catch { throw new ProcessorException(ProcessPhase.Input); }
		}

		public string Format(string input) => Format(JToken.Parse(input));

		private string Format(JToken token) => token.ToString(Formatting.Indented);

		public override string Reinput(IEnumerable<string> outputs)
		{
			var input = outputs.Aggregate((o1, o2) => $"{o1},{Environment.NewLine}{o2}");
			return outputs.Count() > 1 ? Format($"[{Environment.NewLine}{input}{Environment.NewLine}]") : input;
		}
	}
}
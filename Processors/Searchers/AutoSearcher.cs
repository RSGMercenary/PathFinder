using PathFinder.Processors.Abstracts;
using PathFinder.Processors.Datas;
using System.Collections.Generic;

namespace PathFinder.Processors.Searchers
{
	public class AutoSearcher : AutoProcessor<ISearcher>, ISearcher
	{
		public override ProcessType ProcessType => ProcessType.Search;

		public AutoSearcher(IEnumerable<IProcessor> processors) : base(processors) { }
	}
}
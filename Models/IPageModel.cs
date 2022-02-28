using System.Collections.Generic;

namespace PathFinder.Models
{
	public interface IPageModel
	{
		string Parameter { get; }
		IEnumerable<string> Parameters { get; }
		string Input { get; }
		bool FormData { get; }
		bool Convert { get; }
		bool CaseSensitive { get; }
	}
}
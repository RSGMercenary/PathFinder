using System.Text.RegularExpressions;

namespace PathFinder.Processors.Datas
{
	public struct ProcessResult
	{
		public string Output { get; }
		public string Details { get; }
		public Match Scroll { get; }

		public ProcessResult(string output)
		{
			Output = output;
			Details = "";
			Scroll = null;
		}

		public ProcessResult(string output, string details)
		{
			Output = output;
			Details = details;
			Scroll = null;
		}

		public ProcessResult(string output, string details, Match scroll)
		{
			Output = output;
			Details = details;
			Scroll = scroll;
		}
	}
}
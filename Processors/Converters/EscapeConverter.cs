using PathFinder.Processors.Abstracts;
using PathFinder.Processors.Datas;
using PathFinder.Processors.Interfaces;
using System.Text.RegularExpressions;

namespace PathFinder.Processors.Converters
{
	public class EscapeConverter : Converter, IInfoProcessor
	{
		public override ProcessType ProcessType => ProcessType.ConvertEscape;

		public string InfoUrl => "https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.escape";

		protected override string ConvertTo(string value) => Regex.Escape(value);

		protected override string ConvertFrom(string value) => Regex.Unescape(value);
	}
}
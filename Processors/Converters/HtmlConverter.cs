using PathFinder.Processors.Abstracts;
using PathFinder.Processors.Datas;
using PathFinder.Processors.Interfaces;
using PathFinder.Utilities;
using System.Collections.Generic;
using System.Web;

namespace PathFinder.Processors.Converters
{
	public class HtmlConverter : Converter, IFormatterProcessor, IInfoProcessor
	{
		public override ProcessType ProcessType => ProcessType.ConvertHtml;

		public string InfoUrl => "https://docs.microsoft.com/en-us/dotnet/api/system.web.httputility.htmlencode";

		protected override string ConvertTo(string value) => HttpUtility.HtmlDecode(value);

		protected override string ConvertFrom(string value) => HttpUtility.HtmlEncode(value);

		public override string Reinput(IEnumerable<string> outputs) => Xml.Format(outputs);

		public string Format(string input) => Xml.Format(input);
	}
}
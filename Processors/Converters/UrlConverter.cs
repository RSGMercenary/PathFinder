using PathFinder.Processors.Abstracts;
using PathFinder.Processors.Datas;
using PathFinder.Processors.Interfaces;
using System.Web;

namespace PathFinder.Processors.Converters
{
	public class UrlConverter : Converter, IInfoProcessor
	{
		public override ProcessType ProcessType => ProcessType.ConvertUrl;

		public string InfoUrl => "https://docs.microsoft.com/en-us/dotnet/api/system.web.httputility.urlencode";

		protected override string ConvertTo(string value) => HttpUtility.UrlDecode(value);

		protected override string ConvertFrom(string value) => HttpUtility.UrlEncode(value);
	}
}
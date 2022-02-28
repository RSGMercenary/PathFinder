using PathFinder.Processors.Abstracts;
using PathFinder.Processors.Datas;

namespace PathFinder.Processors.Converters
{
	public class Base64Converter : Converter
	{
		public override ProcessType ProcessType => ProcessType.ConvertBase64;

		protected override string ConvertTo(string value) => ProcessorUtility.ToBase64(value);

		protected override string ConvertFrom(string value) => ProcessorUtility.FromBase64(value);
	}
}
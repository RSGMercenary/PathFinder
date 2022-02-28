using PathFinder.Processors.Abstracts;
using PathFinder.Processors.Datas;
using PathFinder.Processors.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PathFinder.Processors.Converters
{
	public class HexConverter : Converter, IFormatterProcessor
	{
		public override ProcessType ProcessType => ProcessType.ConvertHex;

		protected override string ConvertTo(string value)
		{
			return ProcessorUtility.SpaceAggregate(Encoding.Default.GetBytes(value).Select(b => b.ToString("X2")));
		}

		protected override string ConvertFrom(string value)
		{
			return Encoding.Default.GetString(GetHexValues(value).Select(s => byte.Parse(s, NumberStyles.HexNumber)).ToArray());
		}

		private IEnumerable<string> GetHexValues(string value)
		{
			return Regex.Matches(value, @"(?:0x)?[\da-f]{2}", RegexOptions.IgnoreCase)
				.Cast<Match>()
				.Select(m => m.Value)
				.Select(s => s.Replace("0x", ""));
		}

		public string Format(string input) => ProcessorUtility.SpaceAggregate(GetHexValues(input));
	}
}
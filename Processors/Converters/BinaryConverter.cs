using PathFinder.Processors.Abstracts;
using PathFinder.Processors.Datas;
using PathFinder.Processors.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PathFinder.Processors.Converters
{
	public class BinaryConverter : Converter, IFormatterProcessor
	{
		public override ProcessType ProcessType => ProcessType.ConvertBinary;
		private const int ByteLength = 8;
		protected override string ConvertTo(string value)
		{
			return Encoding.UTF8.GetBytes(value)
				.Select(b => Convert.ToString(b, 2))
				.Select(b => b.PadLeft(ByteLength, '0'))
				.Aggregate((b1, b2) => $"{b1} {b2}");
		}

		protected override string ConvertFrom(string value)
		{
			return Encoding.UTF8.GetString(GetBinaryValues(value).Select(b => Convert.ToByte(b, 2)).ToArray());
		}

		private IEnumerable<string> GetBinaryValues(string value)
		{
			return GetBytes(ProcessorUtility.SpaceSplit(value)
				.Select(b => b.PadLeft(ByteLength, '0'))
				.Aggregate((b1, b2) => $"{b1}{b2}"));
		}

		private IEnumerable<string> GetBytes(string value)
		{
			for(var index = 0; index < value.Length; index += ByteLength)
				yield return value.Substring(index, ByteLength);
		}

		public string Format(string input)
		{
			if(Regex.IsMatch(input, @"[^01\s]"))
				throw new ArgumentException();
			return ProcessorUtility.SpaceAggregate(GetBinaryValues(input));
		}
	}
}
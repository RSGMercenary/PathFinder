using Sgml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace PathFinder.Utilities
{
	public static class Xml
	{
		public static XmlDocument Get(string input)
		{
			using(var reader = new SgmlReader())
			{
				reader.InputStream = new StringReader(Fix(input));
				reader.DocType = "HTML";
				reader.WhitespaceHandling = WhitespaceHandling.None;
				reader.CaseFolding = CaseFolding.ToLower;

				var xml = new XmlDocument();
				xml.Load(reader);
				return xml;
			}
		}

		private static string Fix(string input)
		{
			input = Regex.Replace(input, @"<!DOCTYPE.*?>\s*", "", RegexOptions.IgnoreCase);
			input = Regex.Replace(input, @"(?<=<html).*?(?=>)", "", RegexOptions.IgnoreCase);
			return input;
		}

		public static string Format(string input) => Format(Get(input));

		public static string Format(XmlNode node)
		{
			using(var sw = new StringWriter())
			{
				using(var xw = new XmlTextWriter(sw))
				{
					xw.Formatting = Formatting.Indented;
					xw.Indentation = 2;
					node.WriteTo(xw);
				}
				return sw.ToString();
			}
		}

		public static string Format(IEnumerable<string> inputs)
		{
			var input = inputs.Aggregate((o1, o2) => $"{o1}{Environment.NewLine}{o2}");
			return Format($"<html>{Environment.NewLine}{input}{Environment.NewLine}</html>");
		}
	}
}
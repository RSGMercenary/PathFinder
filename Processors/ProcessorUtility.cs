using PathFinder.Extensions;
using PathFinder.Processors.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PathFinder.Processors
{
	public static class ProcessorUtility
	{
		public static string PadBase64(string value) => value.PadRight(value.Length + (4 - value.Length % 4) % 4, '=');

		public static string ToBase64(string value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));

		public static string FromBase64(string value) => Encoding.UTF8.GetString(Convert.FromBase64String(PadBase64(value.Trim())));

		public static RegexOptions CaseSensitive(bool caseSensitive) => caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase;

		public static IEnumerable<string> SpaceSplit(string value) => Regex.Split(value, @"(?:\W|_)+").Where(h => !string.IsNullOrWhiteSpace(h));

		public static string SpaceAggregate(IEnumerable<string> values) => values.Aggregate((a, b) => $"{a} {b}");

		public static IEnumerable<ProcessResult> GetOutputs(ProcessType processType, ProcessPhase phase, IEnumerable<ProcessResult> results)
		{
			var typeString = processType.GetDescriptionOrToString();
			var phaseString = phase.GetDescriptionOrToString().ToLower();

			if(phase == ProcessPhase.Input)
				return new ProcessResult($"Invalid {typeString} {phaseString}.").Enumerate();
			else if(phase == ProcessPhase.Parameters)
				return new ProcessResult($"Invalid {typeString} {phaseString}.").Enumerate();
			else if(phase == ProcessPhase.Processor)
				return new ProcessResult($"Unimplemented {typeString} {phaseString}.", DarthSearchious).Enumerate();
			else if(phase == ProcessPhase.Outputs && !results.Any())
				return new ProcessResult($"No {typeString} {phaseString}.").Enumerate();
			return results;
		}

		private const string DarthSearchious = "\"The dark side of the Parse is a pathway to outputs some may consider to be... unnatural.\" - Darth Searchious";

		public static Match GetMatch(string input, string value, int index, bool casing)
		{
			var complileWhitespace = Regex.Replace(value, @"\s+", @"\s*");
			var escapeCharacters = Regex.Escape(complileWhitespace);
			var fixWhitespace = Regex.Replace(escapeCharacters, @"\\\\s\\\*", @"\s*");
			var matches = Regex.Matches(input, fixWhitespace, CaseSensitive(casing));

			if(matches.Count <= 0)
				return null;
			if(index > matches.Count - 1)
				index = matches.Count - 1;
			return matches[index];
		}
	}
}
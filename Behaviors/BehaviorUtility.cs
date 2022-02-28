using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace PathFinder.Behaviors
{
	public static class BehaviorUtility
	{
		public static IEnumerable<Match> GetSelectionLines(TextBox box)
		{
			return Regex.Matches(box.SelectedText, @"(?<=^\s*?)\S.*?(?=\s*?$)", RegexOptions.Multiline).Cast<Match>();
		}
	}
}
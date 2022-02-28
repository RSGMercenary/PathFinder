using PathFinder.Extensions;
using System.ComponentModel;

namespace PathFinder.Processors.Datas
{
	public class ProcessCategory
	{
		public ProcessType ProcessType { get; }
		public string Category { get; }

		public ProcessCategory(ProcessType processType)
		{
			ProcessType = processType;
			Category = processType.GetType().GetField(processType.ToString()).GetAttribute<CategoryAttribute>().Category;
		}
	}
}
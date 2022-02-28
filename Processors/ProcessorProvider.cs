using PathFinder.Processors.Abstracts;
using PathFinder.Processors.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PathFinder.Processors
{
	public static class ProcessorProvider
	{
		public static IEnumerable<IProcessor> GetProcessors()
		{
			var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(IProcessor).IsAssignableFrom(t));
			var processors = GetProcessors(types, false);
			var autoProcessors = GetProcessors(types, true, processors);
			return processors.Concat(autoProcessors);
		}

		private static IEnumerable<IProcessor> GetProcessors(IEnumerable<Type> types, bool auto, params object[] args)
		{
			return types.Where(t => typeof(IAutoProcessor).IsAssignableFrom(t) == auto)
				.Select(t => (IProcessor)Activator.CreateInstance(t, args)).Where(p => p.ProcessType != ProcessType.None);
		}
	}
}
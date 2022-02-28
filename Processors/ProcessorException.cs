using PathFinder.Processors.Datas;
using System;

namespace PathFinder.Processors
{
	public class ProcessorException : Exception
	{
		public ProcessPhase Phase { get; }

		public ProcessorException(ProcessPhase phase) => Phase = phase;
	}
}
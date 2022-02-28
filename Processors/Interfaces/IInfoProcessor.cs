using PathFinder.Processors.Abstracts;

namespace PathFinder.Processors.Interfaces
{
	public interface IInfoProcessor : IProcessor
	{
		string InfoUrl { get; }
	}
}
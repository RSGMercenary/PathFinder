using PathFinder.Processors.Abstracts;

namespace PathFinder.Processors.Interfaces
{
	public interface IFormatterProcessor : IProcessor
	{
		string Format(string input);
	}
}
namespace Mapex.Specifications
{
	public interface IWhereSpecification : ISpecification
	{
		bool Matches(IDocument document);
	}
}

using System.Dynamic;

namespace Mapex.Specifications
{
	public interface IResolveSpecification : ISpecification
	{
		bool Bypass { get; set; }
		void Resolve(string property, ExpandoObject obj);
	}
}

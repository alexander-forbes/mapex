using System.Dynamic;
using Mapex.Specifications;

namespace Mapex.Services
{
	internal class Resolver : IPropertyTransformer
	{
		public void Apply(PropertyTransformSpecification specification, ExpandoObject obj)
		{
			specification.Resolve?.Resolve(specification.Property, obj);
		}
	}
}

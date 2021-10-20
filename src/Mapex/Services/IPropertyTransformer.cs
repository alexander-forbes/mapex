using System.Dynamic;
using Mapex.Specifications;

namespace Mapex.Services
{
	internal interface IPropertyTransformer
	{
		void Apply(PropertyTransformSpecification specification, ExpandoObject obj);
	}
}

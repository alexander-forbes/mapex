using System.Collections.Generic;
using System.Dynamic;

namespace Mapex.Specifications
{
	public interface IInterceptSpecification : ISpecification
	{
		IEnumerable<ExpandoObject> BeforeFilter(IEnumerable<ExpandoObject> objects);
		IEnumerable<ExpandoObject> AfterFilter(IEnumerable<ExpandoObject> objects);
		IEnumerable<ExpandoObject> BeforeTransform(IEnumerable<ExpandoObject> objects);
		IEnumerable<ExpandoObject> BeforeMap(IEnumerable<ExpandoObject> objects);
	}
}

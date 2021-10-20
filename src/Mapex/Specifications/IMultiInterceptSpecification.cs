using System.Collections.Generic;
using System.Dynamic;

namespace Mapex.Specifications
{
	public interface IMultiInterceptSpecification : IInterceptSpecification
	{
		IEnumerable<IInterceptSpecification> Specifications { get; set; }
	}
}

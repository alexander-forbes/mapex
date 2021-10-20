using System.Collections.Generic;
using System.Dynamic;

namespace Mapex.Specifications
{
	public interface IExtractSpecification : ISpecification
	{
		IEnumerable<ExpandoObject> Process(IDocument document);
	}
}

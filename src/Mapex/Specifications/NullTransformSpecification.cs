using System.Collections.Generic;
using System.Dynamic;
using Notus;

namespace Mapex.Specifications
{
	internal class NullTransformSpecification : ITransformSpecification
	{
		public IEnumerable<PropertyTransformSpecification> Properties { get; set; }

		public void Process(IEnumerable<ExpandoObject> objects)
		{
		}

		public void Validate(Notification notification)
		{
		}
	}
}

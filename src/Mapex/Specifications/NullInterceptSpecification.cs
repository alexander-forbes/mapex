using System.Collections.Generic;
using System.Dynamic;
using Notus;

namespace Mapex.Specifications
{
	internal class NullInterceptSpecification : IInterceptSpecification
	{
		public IEnumerable<ExpandoObject> BeforeTransform(IEnumerable<ExpandoObject> objects)
		{
			return objects;
		}

		public IEnumerable<ExpandoObject> BeforeMap(IEnumerable<ExpandoObject> objects)
		{
			return objects;
		}

		public IEnumerable<ExpandoObject> BeforeFilter(IEnumerable<ExpandoObject> objects)
		{
			return objects;
		}

		public IEnumerable<ExpandoObject> AfterFilter(IEnumerable<ExpandoObject> objects)
		{
			return objects;
		}

		public void Validate(Notification notification)
		{
		}
	}
}

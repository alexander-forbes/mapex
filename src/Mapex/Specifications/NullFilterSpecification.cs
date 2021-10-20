using System.Collections.Generic;
using System.Dynamic;
using Notus;

namespace Mapex.Specifications
{
	internal class NullFilterSpecification : IFilterSpecification
	{
		public string Where { get; set; }

		public IEnumerable<ExpandoObject> Process(IEnumerable<ExpandoObject> objects)
		{
			return objects;
		}

		public void Validate(Notification notification)
		{
		}
	}
}

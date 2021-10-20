using System.Dynamic;
using Notus;

namespace Mapex.Specifications
{
	internal class NullResolveSpecification : IResolveSpecification
	{
		public bool Bypass { get; set; }

		public void Resolve(string property, ExpandoObject obj)
		{
		}

		public void Validate(Notification notification)
		{
		}
	}
}

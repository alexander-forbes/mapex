using Mapex.Specifications;
using Notus;

namespace Mapex.Tests.Stubs
{
	public class AppendSpecificationSpy : AppendSpecification
	{
		public bool ValidateCalled;

		public override void Validate(Notification notification)
		{
			ValidateCalled = true;
		}
	}
}

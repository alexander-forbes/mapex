using Mapex.Specifications;
using Notus;

namespace Mapex.Tests.Stubs
{
	public class ConvertSpecificationSpy : ConvertSpecification
	{
		public bool ValidateCalled;

		public override void Validate(Notification notification)
		{
			ValidateCalled = true;
		}
	}
}

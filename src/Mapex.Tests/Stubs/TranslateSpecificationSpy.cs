using Mapex.Specifications;
using Notus;

namespace Mapex.Tests.Stubs
{
	public class TranslateSpecificationSpy : TranslateSpecification
	{
		public bool ValidateCalled;

		public override void Validate(Notification notification)
		{
			ValidateCalled = true;
		}
	}
}

using Mapex.Specifications;
using Notus;
using NUnit.Framework;

namespace Mapex.Tests.Specifications
{
	[TestFixture]
	public class When_calling_validate_on_null_resolve_specification
	{
		[Test]
		public void It_should_not_add_any_notices()
		{
			var notification = new Notification();

			var specification = new NullResolveSpecification();
			specification.Validate(notification);

			Assert.IsEmpty(notification.Errors);
			Assert.IsEmpty(notification.Warnings);
		}
	}
}

using Mapex.Specifications;
using Notus;
using NUnit.Framework;

namespace Mapex.Tests.Specifications
{
	[TestFixture]
	public class When_calling_validate_on_create_specification
	{
		[Test]
		public void It_should_add_a_notification_when_the_value_property_is_not_set()
		{
			var notification = new Notification();

			var specification = new CreateSpecification();
			specification.Validate(notification);

			Assert.IsTrue(notification.IncludesError("The Value property of the create specification has not been specified."));
		}
	}
}

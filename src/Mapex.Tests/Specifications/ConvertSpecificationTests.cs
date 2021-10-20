using System;
using Mapex.Specifications;
using Notus;
using NUnit.Framework;

namespace Mapex.Tests.Specifications
{
	public class When_calling_validate_on_convert_specification
	{
		[Test]
		public void It_should_throw_an_exception_when_the_notification_property_is_null()
		{
			var specification = new ConvertSpecification();
			Assert.Throws<ArgumentNullException>(() => specification.Validate(null));
		}

		[Test]
		public void It_should_add_an_error_notification_when_the_to_property_is_not_set()
		{
			var notification = new Notification();

			var specification = new ConvertSpecification();
			specification.Validate(notification);

			Assert.IsTrue(notification.IncludesError("The To property of the convert specification is not set."));
		}
	}
}

using System;
using Mapex.Specifications;
using Notus;
using NUnit.Framework;

namespace Mapex.Tests.Specifications
{
	public class When_calling_validate_on_translate_specification
	{
		[Test]
		public void It_should_throw_an_exception_when_the_notification_property_is_null()
		{
			var specification = new TranslateSpecification();
			Assert.Throws<ArgumentNullException>(() => specification.Validate(null));
		}

		[Test]
		public void It_should_add_an_error_notification_when_the_when_property_is_not_set()
		{
			var notification = new Notification();

			var specification = new TranslateSpecification
			{
				Then = "replacement-value"
			};

			specification.Validate(notification);

			Assert.IsTrue(notification.IncludesError("The When property of the translate specification is not set."));
		}

		[Test]
		public void It_should_add_an_error_notification_when_the_then_property_is_not_set()
		{
			var notification = new Notification();

			var specification = new TranslateSpecification
			{
				When = "search-value"
			};

			specification.Validate(notification);

			Assert.IsTrue(notification.IncludesError("The Then property of the translate specification is not set."));
		}
	}
}

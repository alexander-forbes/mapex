using System;
using System.Dynamic;
using Mapex.Services;
using Mapex.Specifications;
using Moq;
using Notus;
using NUnit.Framework;

namespace Mapex.Tests.Specifications
{
	public class When_calling_validate_on_map_specification
	{
		[Test]
		public void It_should_throw_an_exception_when_the_notification_property_is_null()
		{
			var specification = new MapSpecification();
			Assert.Throws<ArgumentNullException>(() => specification.Validate(null));
		}

		[Test]
		public void It_should_add_an_error_notification_when_the_as_property_is_not_set()
		{
			var notification = new Notification();

			var specification = new MapSpecification();
			specification.Validate(notification);

			Assert.IsTrue(notification.IncludesError("The As property of the map specification is not set."));
		}
	}

	[TestFixture]
	public class When_calling_process_on_map_specification
	{
		[Test]
		public void It_should_throw_an_exception_when_the_objects_parameter_is_null()
		{
			var specification = new MapSpecification();
			Assert.Throws<ArgumentNullException>(() => specification.Process(null));
		}

		[Test]
		public void It_should_call_the_object_mapper_with_the_specification()
		{
			var objects = new[] { new ExpandoObject() };

			var objectMapper = new Mock<IObjectMapper>();

			var specification = new MapSpecification(objectMapper.Object);
			specification.Process(objects);

			objectMapper.Verify(m => m.Map(objects, specification));
		}
	}
}

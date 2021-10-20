using System;
using System.Dynamic;
using System.Linq;
using Mapex.Services;
using Mapex.Specifications;
using Mapex.Tests.Stubs;
using Moq;
using Notus;
using NUnit.Framework;

namespace Mapex.Tests.Specifications
{
	[TestFixture]
	public class When_constructing_a_transform_specification
	{
		[Test] 
		public void It_should_throw_an_exception_when_the_object_transformer_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new TransformSpecification(null));
		}
	}

	[TestFixture]
	public class When_calling_validate_on_transform_specification
	{
		[Test]
		public void It_should_throw_an_exception_when_the_notification_property_is_null()
		{
			var specification = new TransformSpecification();
			Assert.Throws<ArgumentNullException>(() => specification.Validate(null));
		}

		[Test]
		public void It_should_add_an_error_notification_when_the_properties_property_is_not_set()
		{
			var notification = new Notification();

			var specification = new TransformSpecification();

			specification.Validate(notification);

			Assert.IsTrue(notification.IncludesError("The Properties property of the transform specification is not set."));
		}

		[Test]
		public void It_should_validate_every_property_transform_specification()
		{
			var notification = new Notification();

			var specification = new TransformSpecification
			{
				Properties = new[]
				{
					new PropertyTransformSpecificationSpy(),
					new PropertyTransformSpecificationSpy()
				}
			};

			specification.Validate(notification);

			var spies = specification.Properties.Cast<PropertyTransformSpecificationSpy>();
			
			Assert.IsTrue(spies.All(s => s.ValidateCalled));
		}
	}

	[TestFixture]
	public class When_calling_process_on_transform_specification
	{
		[Test]
		public void It_should_throw_an_exception_when_the_objects_parameter_is_null()
		{
			var specification = new TransformSpecification(new Mock<IObjectTransformer>().Object);
			Assert.Throws<ArgumentNullException>(() => specification.Process(null));
		}

		[Test]
		public void It_should_call_the_the_object_transformer()
		{
			var objectTransformer = new Mock<IObjectTransformer>();
			var specification = new TransformSpecification(objectTransformer.Object);

			var objects = new[] {new ExpandoObject()};

			specification.Process(objects);

			objectTransformer.Verify(t => t.Transform(objects, specification));
		}
	}
}

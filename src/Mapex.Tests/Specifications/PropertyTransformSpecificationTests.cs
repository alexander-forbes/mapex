using System;
using System.Linq;
using Mapex.Specifications;
using Mapex.Tests.Stubs;
using Notus;
using NUnit.Framework;

namespace Mapex.Tests.Specifications
{
	[TestFixture]
	public class When_constructing_a_property_transform_specification
	{
		[Test]
		public void It_should_set_the_translate_property_to_an_empty_enumerable()
		{
			CollectionAssert.IsEmpty(new PropertyTransformSpecification().Translate);
		}

		[Test]
		public void It_should_set_the_resolve_property_to_a_null_resolve_specification()
		{
			Assert.IsInstanceOf<NullResolveSpecification>(new PropertyTransformSpecification().Resolve);
		}
	}

	[TestFixture]
	public class When_calling_validate_on_property_transform_specification
	{
		[Test]
		public void It_should_throw_an_exception_when_the_notification_property_is_null()
		{
			var specification = new PropertyTransformSpecification();
			Assert.Throws<ArgumentNullException>(() => specification.Validate(null));
		}

		[Test]
		public void It_should_add_an_error_notification_if_the_property_property_is_not_set()
		{
			var notification = new Notification();
			
			var specification = new PropertyTransformSpecification();
			specification.Validate(notification);

			Assert.IsTrue(notification.IncludesError("The Property property of the property transform specification is not set."));
		}

		[Test]
		public void It_should_add_an_error_notification_if_the_translate_property_is_not_set()
		{
			var notification = new Notification();

			var specification = new PropertyTransformSpecification
			{
				Property = "property",
				Translate = null
			};

			specification.Validate(notification);

			Assert.IsTrue(notification.IncludesError("The translate property of the property transform specification is not set."));
		}

		[Test]
		public void It_should_call_validate_on_the_translate_specifications_when_the_property_is_set()
		{
			var notification = new Notification();

			var specification = new PropertyTransformSpecification
			{
				Property = "property",
				Translate = new []
				{
					new TranslateSpecificationSpy(),
					new TranslateSpecificationSpy()
				}
			};

			specification.Validate(notification);

			var spies = specification.Translate.Cast<TranslateSpecificationSpy>();

			Assert.IsTrue(spies.All(s => s.ValidateCalled));
		}

		[Test]
		public void It_should_call_validate_on_the_convert_specification_when_the_property_is_set()
		{
			var notification = new Notification();

			var specification = new PropertyTransformSpecification
			{
				Property = "property",
				Convert = new ConvertSpecificationSpy()
			};

			specification.Validate(notification);

			var spy = specification.Convert as ConvertSpecificationSpy;

			Assert.IsTrue(spy.ValidateCalled);
		}

		[Test]
		public void It_should_call_validate_on_the_append_specification_when_the_property_is_set()
		{
			var notification = new Notification();

			var specification = new PropertyTransformSpecification
			{
				Property = "property",
				Append = new AppendSpecificationSpy()
			};

			specification.Validate(notification);

			var spy = specification.Append as AppendSpecificationSpy;

			Assert.IsTrue(spy.ValidateCalled);
		}

		[Test]
		public void It_should_call_validate_on_the_create_specification_when_the_property_is_set()
		{
			var notification = new Notification();

			var specification = new PropertyTransformSpecification
			{
				Property = "property",
				Create = new CreateSpecificationSpy()
			};

			specification.Validate(notification);

			var spy = specification.Create as CreateSpecificationSpy;

			Assert.IsTrue(spy.ValidateCalled);
		}
	}
}

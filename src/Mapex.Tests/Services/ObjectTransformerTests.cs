using System;
using System.Dynamic;
using Mapex.Services;
using Mapex.Specifications;
using Moq;
using NUnit.Framework;

namespace Mapex.Tests.Services
{
	[TestFixture]
	public class When_constructing_an_object_transformer
	{
		[Test] 
		public void It_should_throw_an_exception_when_the_field_transformers_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new ObjectTransformer(null));
		}
	}

	[TestFixture]
	public class When_calling_transform_on_object_transformer
	{
		[Test]
		public void It_should_apply_every_property_transformer_to_every_object()
		{
			var transformer1 = new Mock<IPropertyTransformer>();
			var transformer2 = new Mock<IPropertyTransformer>();

			var transformers = new []
			{
				transformer1.Object,
				transformer2.Object
			};

			var propertyTransformSpecification1 = new PropertyTransformSpecification();
			var propertyTransformSpecification2 = new PropertyTransformSpecification();

			var objectTransformer = new ObjectTransformer(transformers);

			var objects = new[] {new ExpandoObject(), new ExpandoObject()};

			var specification = new TransformSpecification
			{
				Properties = new[]
				{
					propertyTransformSpecification1,
					propertyTransformSpecification2
				}
			};

			objectTransformer.Transform(objects, specification);

			transformer1.Verify(t => t.Apply(propertyTransformSpecification1, objects[0]));
			transformer2.Verify(t => t.Apply(propertyTransformSpecification1, objects[0]));

			transformer1.Verify(t => t.Apply(propertyTransformSpecification2, objects[0]));
			transformer2.Verify(t => t.Apply(propertyTransformSpecification2, objects[0]));

			transformer1.Verify(t => t.Apply(propertyTransformSpecification1, objects[1]));
			transformer2.Verify(t => t.Apply(propertyTransformSpecification1, objects[1]));

			transformer1.Verify(t => t.Apply(propertyTransformSpecification2, objects[1]));
			transformer2.Verify(t => t.Apply(propertyTransformSpecification2, objects[1]));
		}
	}
}

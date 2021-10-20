using System;
using System.Dynamic;
using Mapex.Services;
using Mapex.Specifications;
using NUnit.Framework;

namespace Mapex.Tests.Services
{
	[TestFixture]
	public class When_calling_apply_on_creator
	{
		[Test]
		public void It_should_not_do_anything_when_a_create_specification_has_not_been_specified()
		{
			var target = new ExpandoObject();

			var specification = new PropertyTransformSpecification
			{
				Property = "Amount",
				Create = null
			};

			var creator = new Creator();
			Assert.DoesNotThrow(() => creator.Apply(specification, target));
		}

		[Test]
		public void It_should_throw_an_exception_when_the_object_has_the_property_but_not_set_for_overwrite()
		{
			dynamic obj = new ExpandoObject();
			obj.Name = "Joe Soap";

			var specification = new PropertyTransformSpecification
			{
				Property = "Name",
				Create = new CreateSpecification { Overwrite = false, Value = "Jane Doe" }
			};

			var creator = new Creator();
			var exception = Assert.Throws<Exception>(() => creator.Apply(specification, obj));
			Assert.AreEqual("Property Name exists on the object and cannot be overwritten.", exception.Message);
		}

		[Test]
		public void It_should_overwrite_the_property_when_the_object_has_the_property_and_it_is_set_for_overwrite()
		{
			dynamic obj = new ExpandoObject();
			obj.Name = "Joe Soap";

			var specification = new PropertyTransformSpecification
			{
				Property = "Name",
				Create = new CreateSpecification { Overwrite = true, Value = "Jane Doe" }
			};

			var creator = new Creator();
			creator.Apply(specification, obj);
			Assert.AreEqual("Jane Doe", obj.Name);
		}

		[Test]
		public void It_should_create_the_property_and_set_the_property_value_to_null()
		{
			dynamic obj = new ExpandoObject();

			var specification = new PropertyTransformSpecification
			{
				Property = "Name",
				Create = new CreateSpecification { Overwrite = false, Value = "[null]" }
			};

			var creator = new Creator();
			creator.Apply(specification, obj);
			Assert.IsNull(obj.Name);
		}

		[Test]
		public void It_should_create_the_property_and_set_the_property_value_to_the_specified_value()
		{
			dynamic obj = new ExpandoObject();

			var specification = new PropertyTransformSpecification
			{
				Property = "Name",
				Create = new CreateSpecification { Overwrite = false, Value = "Joe Soap" }
			};

			var creator = new Creator();
			creator.Apply(specification, obj);
			Assert.AreEqual("Joe Soap", obj.Name);
		}
	}
}

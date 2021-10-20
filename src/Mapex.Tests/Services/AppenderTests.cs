using System;
using System.Dynamic;
using Mapex.Services;
using Mapex.Specifications;
using NUnit.Framework;

namespace Mapex.Tests.Services
{
	[TestFixture]
	public class When_calling_apply_on_appender
	{
		[Test]
		public void It_should_not_do_anything_when_an_append_value_has_not_been_specified()
		{
			var target = new ExpandoObject();

			var specification = new PropertyTransformSpecification
			{
				Property = "Amount",
				Append = null
			};

			var appender = new Appender();
			Assert.DoesNotThrow(() => appender.Apply(specification, target));
		}

		[Test]
		public void It_should_throw_an_exception_when_the_object_does_not_contain_the_field_to_append()
		{
			dynamic obj = new ExpandoObject();
			obj.Name = "Joe Soap";

			var specification = new PropertyTransformSpecification
			{
				Property = "FirstName",
				Append = new AppendSpecification()
			};

			var appender = new Appender();
			var exception = Assert.Throws<Exception>(() => appender.Apply(specification, obj));
			Assert.AreEqual("Object does not have field FirstName.", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_field_is_null()
		{
			dynamic obj = new ExpandoObject();
			obj.Name = null;

			var specification = new PropertyTransformSpecification
			{
				Property = "Name",
				Append = new AppendSpecification()
			};

			var appender = new Appender();
			var exception = Assert.Throws<Exception>(() => appender.Apply(specification, obj));
			Assert.AreEqual("Fetched a null for property Name", exception.Message);
		}

		[Test]
		public void It_should_prefix_the_property_value_on_the_object_with_the_given_value()
		{
			dynamic obj = new ExpandoObject();
			obj.Name = "Joe Soap";

			var specification = new PropertyTransformSpecification
			{
				Property = "Name",
				Append = new AppendSpecification { Postfix = false, Value = "First name: "}
			};

			var appender = new Appender();
			appender.Apply(specification, obj);
			Assert.AreEqual("First name: Joe Soap", obj.Name);
		}

		[Test]
		public void It_should_postfix_the_property_value_on_the_object_with_the_given_value()
		{
			dynamic obj = new ExpandoObject();
			obj.Name = "Joe Soap";

			var specification = new PropertyTransformSpecification
			{
				Property = "Name",
				Append = new AppendSpecification { Postfix = true, Value = "*"}
			};

			var appender = new Appender();
			appender.Apply(specification, obj);
			Assert.AreEqual("Joe Soap*", obj.Name);
		}
	}
}

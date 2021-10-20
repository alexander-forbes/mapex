using System;
using System.Collections.Generic;
using System.Dynamic;
using NUnit.Framework;

namespace Mapex.Tests
{
	[TestFixture]
	public class When_calling_to_dictionary_on_dynamic
	{
		[Test]
		public void It_should_throw_an_exception_when_the_object_is_null()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => Dynamic.ToDictionary(null));
			Assert.AreEqual("Value cannot be null. (Parameter 'obj')", exception.Message);
		}

		[Test]
		public void It_should_cast_the_dynamic_to_a_dictionary()
		{
			dynamic obj = new ExpandoObject();
			obj.Name = "Joe";

			IDictionary<string, object> result = Dynamic.ToDictionary(obj);
			Assert.IsInstanceOf<IDictionary<string, object>>(result);
			Assert.AreEqual("Joe", result["Name"]);
		}
	}

	[TestFixture]
	public class When_calling_throw_if_field_is_missing
	{
		[Test]
		public void It_should_throw_an_exception_when_the_object_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => Dynamic.ThrowIfFieldMissing(null, "Value"));
		}

		[Test]
		public void It_should_throw_an_exception_when_the_field_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => Dynamic.ThrowIfFieldMissing(new ExpandoObject(), null));
		}

		[Test]
		public void It_should_throw_an_exception_if_the_field_is_missing_from_the_object()
		{
			dynamic obj = new ExpandoObject();
			obj.Name = "Joe";

			var exception = Assert.Throws<Exception>(() => Dynamic.ThrowIfFieldMissing(obj, "Value"));
			Assert.AreEqual("Object does not have field Value.", exception.Message);
		}

		[Test]
		public void It_should_not_throw_an_exception_if_the_field_is_present_on_the_object()
		{
			dynamic obj = new ExpandoObject();
			obj.Name = "Joe";

			Assert.DoesNotThrow(() => Dynamic.ThrowIfFieldMissing(obj, "Name"));
		}
	}
}

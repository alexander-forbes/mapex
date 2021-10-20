using System;
using System.Collections.Generic;
using System.Linq;
using Mapex.Specifications;
using NUnit.Framework;

namespace Mapex.Tests
{
	[TestFixture]
	public class When_calling_is_empty_on_extensions
	{
		[Test]
		public void It_should_return_true_if_the_list_is_empty()
		{
			Assert.IsTrue(Enumerable.Empty<object>().IsEmpty());
		}

		[Test]
		public void It_should_return_false_if_the_list_contains_items()
		{
			Assert.IsFalse(new[] { new object() }.IsEmpty());
		}

		[Test]
		public void It_should_return_true_if_the_list_is_null()
		{
			Assert.IsTrue(Extensions.IsEmpty(null));
		}
	}

	[TestFixture]
	public class When_calling_get_current_property_value_on_extensions
	{
		[Test]
		public void It_should_get_the_the_property_value_from_the_dictionary()
		{
			var obj = new Dictionary<string, object>
			{
				{"Name", "Joe Soap"}
			};

			var value = obj.GetCurrentPropertyValue(new PropertyTransformSpecification { Property = "Name" });

			Assert.AreEqual("Joe Soap", value);
		}

		[Test]
		public void It_should_throw_an_exception_for_a_null_property_value_when_specified()
		{
			var obj = new Dictionary<string, object>
			{
				{"Name", null}
			};

			var exception = Assert.Throws<Exception>(() => 
				obj.GetCurrentPropertyValue(new PropertyTransformSpecification { Property = "Name" })
			);

			Assert.AreEqual("Fetched a null for property Name", exception.Message);
		}

		[Test]
		public void It_should_not_throw_an_exception_for_a_null_property_value_when_specified()
		{
			var obj = new Dictionary<string, object>
			{
				{"Name", null}
			};

			Assert.DoesNotThrow(() => 
				obj.GetCurrentPropertyValue(new PropertyTransformSpecification { Property = "Name" }, false)
			);
		}
	}
}

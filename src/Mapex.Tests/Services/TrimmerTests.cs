using System;
using System.Dynamic;
using Mapex.Services;
using Mapex.Specifications;
using NUnit.Framework;

namespace Mapex.Tests.Services
{
	[TestFixture]
	public class When_calling_apply_on_trimmer
	{
		[Test]
		public void It_should_not_do_anything_when_a_trim_value_has_not_been_specified()
		{
			var target = new ExpandoObject();

			var specification = new PropertyTransformSpecification
			{
				Property = "Amount",
				Trim = null
			};

			var trimmer = new Trimmer();
			Assert.DoesNotThrow(() => trimmer.Apply(specification, target));
		}

		[Test]
		public void It_should_throw_an_exception_when_the_object_does_not_contain_the_field_to_trim()
		{
			dynamic obj = new ExpandoObject();
			obj.Amount = "R3,000.00";

			var specification = new PropertyTransformSpecification
			{
				Property = "FirstName",
				Trim = "[a-zA-Z,]"
			};

			var trimmer = new Trimmer();
			var exception = Assert.Throws<Exception>(() => trimmer.Apply(specification, obj));
			Assert.AreEqual("Object does not have field FirstName.", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_field_is_null()
		{
			dynamic obj = new ExpandoObject();
			obj.Amount = null;

			var specification = new PropertyTransformSpecification
			{
				Property = "Amount",
				Trim = "[a-zA-Z,]"
			};

			var trimmer = new Trimmer();
			var exception = Assert.Throws<Exception>(() => trimmer.Apply(specification, obj));
			Assert.AreEqual("Fetched a null for property Amount", exception.Message);
		}

		[Test]
		public void It_should_replace_the_values_on_the_object_according_to_the_provided_expression()
		{
			dynamic obj = new ExpandoObject();
			obj.Amount = "R3,000.00";

			var specification = new PropertyTransformSpecification
			{
				Property = "Amount",
				Trim = "[a-zA-Z,]"
			};

			var trimmer = new Trimmer();
			trimmer.Apply(specification, obj);
			Assert.AreEqual("3000.00", obj.Amount);
		}
	}
}

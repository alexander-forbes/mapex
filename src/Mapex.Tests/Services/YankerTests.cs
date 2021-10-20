using System;
using System.Dynamic;
using Mapex.Services;
using Mapex.Specifications;
using NUnit.Framework;

namespace Mapex.Tests.Services
{
	[TestFixture]
	public class When_calling_apply_on_yanker
	{
		[Test]
		public void It_should_not_do_anything_when_a_yank_value_has_not_been_specified()
		{
			var target = new ExpandoObject();

			var specification = new PropertyTransformSpecification
			{
				Property = "ID",
				Yank = null
			};

			var yanker = new Yanker();
			Assert.DoesNotThrow(() => yanker.Apply(specification, target));
		}

		[Test]
		public void It_should_throw_an_exception_when_the_object_does_not_contain_the_field_to_yank()
		{
			dynamic obj = new ExpandoObject();
			obj.ID = "Article (FG-123)";

			var specification = new PropertyTransformSpecification
			{
				Property = "FirstName",
				Yank = @"^[\w]+ \((?<ID>[\w-]+)\)$"
			};

			var yanker = new Yanker();
			var exception = Assert.Throws<Exception>(() => yanker.Apply(specification, obj));
			Assert.AreEqual("Object does not have field FirstName.", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_field_is_null()
		{
			dynamic obj = new ExpandoObject();
			obj.ID = null;

			var specification = new PropertyTransformSpecification
			{
				Property = "ID",
				Yank = @"^[\w]+ \((?<ID>[\w-]+)\)$"
			};

			var yanker = new Yanker();
			var exception = Assert.Throws<Exception>(() => yanker.Apply(specification, obj));
			Assert.AreEqual("Fetched a null for property ID", exception.Message);
		}

		[Test]
		public void It_should_yank_the_first_capture_group_returned_by_the_expression()
		{
			dynamic obj = new ExpandoObject();
			obj.ID = "Article (FG-123)";

			var specification = new PropertyTransformSpecification
			{
				Property = "ID",
				Yank = @"^[\w]+ \((?<ID>[\w-]+)\)$"
			};

			var yanker = new Yanker();
			yanker.Apply(specification, obj);
			Assert.AreEqual("FG-123", obj.ID);
		}

		[Test]
		public void It_should_leave_the_property_value_intact_when_the_yank_result_does_not_have_a_matching_capture_group()
		{
			dynamic obj = new ExpandoObject();
			obj.ID = "Article (FG-123)";

			var specification = new PropertyTransformSpecification
			{
				Property = "ID",
				Yank = @"^[\w]+ \((?<unknown>[\w-]+)\)$"
			};

			var yanker = new Yanker();
			yanker.Apply(specification, obj);
			Assert.AreEqual("Article (FG-123)", obj.ID);
		}

		[Test]
		public void It_should_leave_the_property_value_intact_when_the_yank_does_not_match_anything()
		{
			dynamic obj = new ExpandoObject();
			obj.ID = "Article (FG-123)";

			var specification = new PropertyTransformSpecification
			{
				Property = "ID",
				Yank = @"^$"
			};

			var yanker = new Yanker();
			yanker.Apply(specification, obj);
			Assert.AreEqual("Article (FG-123)", obj.ID);
		}
	}
}

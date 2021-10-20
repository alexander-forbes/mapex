using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Mapex.Services;
using Mapex.Specifications;
using Mapex.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace Mapex.Tests.Services
{
	[TestFixture]
	public class When_calling_filter_on_object_filter
	{
		[Test]
		public void It_should_throw_an_exception_when_the_objects_parameter_is_null()
		{
			var filter = new ObjectFilter();
			Assert.Throws<ArgumentNullException>(() => filter.Filter(null, new Mock<IFilterSpecification>().Object));
		}

		[Test]
		public void It_should_throw_an_exception_when_the_specification_parameter_is_null()
		{
			var filter = new ObjectFilter();
			Assert.Throws<ArgumentNullException>(() => filter.Filter(new[] { new ExpandoObject() }, null));
		}

		[Test]
		public void It_should_not_filter_when_the_list_of_objects_is_empty()
		{
			var filter = new ObjectFilter();

			var objects = Enumerable.Empty<ExpandoObject>();

			var result = filter.Filter(objects, new Mock<IFilterSpecification>().Object);

			Assert.AreSame(objects, result);
		}

		[Test]
		public void It_should_filter_the_objects_with_the_given_expression()
		{
			dynamic item1 = new ExpandoObject();
			item1.Name = "Joe";
			item1.Surname = "Bloggs";

			dynamic item2 = new ExpandoObject();
			item2.Name = "Jane";
			item2.Surname = "Doe";

			dynamic item3 = new ExpandoObject();
			item3.Name = "John";
			item3.Surname = "Soap";

			var objects = new List<ExpandoObject>
			{
				item1,
				item2,
				item3
			};

			var specification = new FilterSpecification { Where = "Item[\"Name\"] == \"Joe\" OR Item[\"Surname\"] == \"Soap\"" };

			var result = new ObjectFilter().Filter(objects, specification);

			CollectionAssert.Contains(result, item1);
			CollectionAssert.Contains(result, item3);
		}
	}
}

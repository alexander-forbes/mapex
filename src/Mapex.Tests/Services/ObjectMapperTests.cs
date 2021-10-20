using System;
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
	public class When_calling_map_on_object_mapper
	{
		[Test]
		public void It_should_throw_an_exception_when_the_objects_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new ObjectMapper().Map(null, new Mock<IMapSpecification>().Object));
		}

		[Test]
		public void It_should_throw_an_exception_when_the_specification_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new ObjectMapper().Map(Enumerable.Empty<ExpandoObject>(), null));
		}

		[Test]
		public void It_should_map_the_objects_to_the_specified_type()
		{
			dynamic obj = new ExpandoObject();
			obj.Name = "Joe";
			obj.Surname = "Soap";

			var objects = new ExpandoObject[]
			{
				obj
			};

			var specification = new MapSpecification { As = "Mapex.Tests.Stubs.Stub, Mapex.Tests" };

			var mapper = new ObjectMapper();
			var result = mapper.Map(objects, specification);

			var mapped = result.Cast<Stub>().First();
			Assert.AreEqual("Joe", mapped.Name);
			Assert.AreEqual("Soap", mapped.Surname);
		}
	}
}

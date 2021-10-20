using System.Dynamic;
using Mapex.Specifications;
using Moq;
using NUnit.Framework;

namespace Mapex.Tests.Services
{
	[TestFixture]
	public class When_calling_apply_on_resolver
	{
		private const string PropertyName = "Code";

		[Test]
		public void It_should_return_when_the_resolve_property_on_the_specification_is_null()
		{
			var propertyTransformSpecification = new PropertyTransformSpecification 
			{
				Property = PropertyName,
				Resolve = null
			};

			var resolver = new Mapex.Services.Resolver();
			Assert.DoesNotThrow(() => resolver.Apply(propertyTransformSpecification, new ExpandoObject()));
		}

		[Test]
		public void It_should_call_resolve_for_the_property()
		{
			var obj = new ExpandoObject();
			var d = obj.ToDictionary();
			d.Add(PropertyName, "ABC-ABD");
			
			var specification = new Mock<IResolveSpecification>();
			specification.Setup(s => s.Resolve(PropertyName, obj)).Callback<string, ExpandoObject>((p, eo) => 
			{ 
				var x = eo.ToDictionary();
				x[p] = "XYZ-ZYZ";
			});

			var propertyTransformSpecification = new PropertyTransformSpecification 
			{
				Property = PropertyName,
				Resolve = specification.Object
			};

			var resolver = new Mapex.Services.Resolver();
			resolver.Apply(propertyTransformSpecification, obj);

			Assert.AreEqual("XYZ-ZYZ", d[PropertyName]);
		}
	}
}

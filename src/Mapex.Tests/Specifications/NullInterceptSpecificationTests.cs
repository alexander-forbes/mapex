using System.Dynamic;
using System.Linq;
using Mapex.Specifications;
using Notus;
using NUnit.Framework;

namespace Mapex.Tests.Specifications
{
	[TestFixture]
	public class When_calling_validate_on_null_intercept_specification
	{
		[Test]
		public void It_should_not_add_any_notices()
		{
			var notification = new Notification();

			var specification = new NullInterceptSpecification();
			specification.Validate(notification);

			Assert.IsEmpty(notification.Errors);
			Assert.IsEmpty(notification.Warnings);
		}
	}

	[TestFixture]
	public class When_calling_before_transform_on_null_intercept_specification
	{
		[Test]
		public void It_should_return_the_objects_without_modifications()
		{
			var objects = new[]
			{
				new ExpandoObject()
			};

			var specification = new NullInterceptSpecification();

			var actual = specification.BeforeTransform(objects);
			CollectionAssert.AreEqual(objects, actual);
			CollectionAssert.IsEmpty(actual.First().ToDictionary().Keys);
		}
	}

	[TestFixture]
	public class When_calling_before_map_on_null_intercept_specification
	{
		[Test]
		public void It_should_return_the_objects_without_modifications()
		{
			var objects = new[]
			{
				new ExpandoObject()
			};

			var specification = new NullInterceptSpecification();

			var actual = specification.BeforeMap(objects);
			CollectionAssert.AreEqual(objects, actual);
			CollectionAssert.IsEmpty(actual.First().ToDictionary().Keys);
		}
	}

	[TestFixture]
	public class When_calling_before_filter_on_null_intercept_specification
	{
		[Test]
		public void It_should_return_the_objects()
		{
			var objects = new[]
			{
				new ExpandoObject()
			};

			var specification = new NullInterceptSpecification();

			var actual = specification.BeforeFilter(objects);
			CollectionAssert.AreEqual(objects, actual);
		}
	}

	[TestFixture]
	public class When_calling_after_filter_on_null_intercept_specification
	{
		[Test]
		public void It_should_return_the_objects()
		{
			var objects = new[]
			{
				new ExpandoObject()
			};

			var specification = new NullInterceptSpecification();

			var actual = specification.AfterFilter(objects);
			CollectionAssert.AreEqual(objects, actual);
		}
	}
}

using System.Dynamic;
using Mapex.Specifications;
using Notus;
using NUnit.Framework;

namespace Mapex.Tests.Specifications
{
	[TestFixture]
	public class When_calling_process_on_null_filter_specification
	{
		[Test]
		public void It_should_return_the_list_of_objects()
		{
			var objects = new[]
			{
				new ExpandoObject()
			};

			var specification = new NullFilterSpecification();
			var result = specification.Process(objects);

			Assert.AreSame(objects, result);
		}
	}

	[TestFixture]
	public class When_calling_validate_on_null_filter_specification
	{
		[Test]
		public void It_should_not_add_any_notices()
		{
			var notification = new Notification();

			var specification = new NullFilterSpecification();
			specification.Validate(notification);

			Assert.IsEmpty(notification.Errors);
			Assert.IsEmpty(notification.Warnings);
		}
	}
}

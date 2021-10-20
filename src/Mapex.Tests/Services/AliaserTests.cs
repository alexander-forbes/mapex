using System;
using System.Collections.Generic;
using System.Dynamic;
using Mapex.Services;
using Mapex.Specifications;
using NUnit.Framework;

namespace Mapex.Tests.Services
{
	[TestFixture]
	public class When_calling_apply_on_aliaser
	{
		private Aliaser _Aliaser;
		private dynamic _Target;

		[SetUp]
		public void Setup()
		{
			_Target = new ExpandoObject();
			_Aliaser = new Aliaser();
		}

		[Test]
		public void It_should_not_process_when_the_as_specification_is_not_provided()
		{
			_Aliaser.Apply(new PropertyTransformSpecification(), _Target);
			Assert.IsEmpty(((IDictionary<string, object>)Dynamic.ToDictionary(_Target)).Keys);
		}

		[Test]
		public void It_should_replace_the_existing_aliased_property_if_it_already_exists()
		{
			_Target.Name = "Jane";
			_Target.FirstName = "Joe";

			_Aliaser.Apply(new PropertyTransformSpecification { Property = "Name", As = "FirstName" }, _Target);

			Assert.AreEqual("Jane", _Target.FirstName);
		}

		[Test]
		public void It_should_add_the_field_to_the_target_object_with_the_same_value()
		{
			_Target.Name = "Jane";

			_Aliaser.Apply(new PropertyTransformSpecification { Property = "Name", As = "FirstName" }, _Target);

			Assert.AreEqual("Jane", _Target.FirstName);
		}
	}
}

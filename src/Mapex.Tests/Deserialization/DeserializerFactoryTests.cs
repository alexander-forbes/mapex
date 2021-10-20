using System;
using Mapex.Deserialization;
using NUnit.Framework;

namespace Mapex.Tests.Deserialization
{
	[TestFixture]
	public class When_calling_build_on_directive_deserializer_factory
	{
		[Test]
		public void It_should_throw_an_exception_when_the_tag_map_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new DirectiveDeserializerFactory().Build(null));
		}

		[Test]
		public void It_should_build_a_directive_deserializer()
		{
			var tagMap = new TagMap();
			tagMap.Register("!email", typeof(object));

			var factory = new DirectiveDeserializerFactory();
			var deserializer = factory.Build(tagMap);

			Assert.IsInstanceOf<DirectiveDeserializer>(deserializer);
		}
	}
}

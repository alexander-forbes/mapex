using System;
using System.Collections.Generic;
using Mapex.Deserialization;
using NUnit.Framework;

namespace Mapex.Tests.Deserialization
{
	[TestFixture]
	public class When_calling_register_on_tag_map
	{
		[Test]
		public void It_should_throw_an_exception_when_the_tag_parameter_is_null()
		{
			var map = new TagMap();
			Assert.Throws<ArgumentNullException>(() => map.Register(null, typeof(object)));
		}

		[Test]
		public void It_should_throw_an_exception_when_the_type_parameter_is_null()
		{
			var map = new TagMap();
			Assert.Throws<ArgumentNullException>(() => map.Register("tag-name", null));
		}

		[Test]
		public void It_should_register_the_tag_mapping()
		{
			const string tag = "!email";
			var type = typeof(object);

			var map = new TagMap();
			map.Register(tag, type);

			CollectionAssert.Contains(map, new KeyValuePair<string, Type>(tag, type));
		}

		[Test]
		public void It_should_overwrite_the_tag_if_it_exists()
		{
			const string tag = "!email";

			var map = new TagMap();
			map.Register(tag, typeof(object));
			map.Register(tag, typeof(int));

			CollectionAssert.Contains(map, new KeyValuePair<string, Type>(tag, typeof(int)));
		}
	}

	[TestFixture]
	public class When_calling_get_enumerator_on_tag_map
	{
		[Test]
		public void It_should_return_the_registered_tags_and_types()
		{
			var map = new TagMap();
			map.Register("!email", typeof(object));
			map.Register("!excel", typeof(object));

			var expected = new[]
			{
				new KeyValuePair<string, Type>("!email", typeof(object)),
				new KeyValuePair<string, Type>("!excel", typeof(object)),
			};

			CollectionAssert.AreEquivalent(expected, map);
		}
	}
}

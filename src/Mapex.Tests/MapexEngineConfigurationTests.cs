using System;
using Mapex.Deserialization;
using Mapex.Logging;
using Moq;
using NUnit.Framework;

namespace Mapex.Tests
{
	[TestFixture]
	public class When_constructing_a_mapex_engine_configuration
	{
		[Test]
		public void It_should_set_the_tag_map_property()
		{
			Assert.IsInstanceOf<TagMap>(new MapexEngineConfiguration().Tags);
		}
	}

	[TestFixture]
	public class When_calling_verify_on_mapex_engine_configuration
	{
		[Test]
		public void It_should_throw_an_exception_when_a_directive_loader_has_not_been_configured()
		{
			var config = new MapexEngineConfiguration();
			var exception = Assert.Throws<Exception>(() => config.Verify());
			Assert.AreEqual("A directive loader has not been configured.", exception.Message);
		}

		[Test]
		public void It_should_not_throw_an_exception_when_the_configuration_is_valid()
		{
			var config = new MapexEngineConfiguration 
			{
				Loader = new Mock<IDirectiveLoader>().Object
			};

			Assert.DoesNotThrow(() => config.Verify());
		}
	}
}

using System;
using System.Collections.Generic;
using Mapex.Deserialization;
using Mapex.Specifications;
using Moq;
using NUnit.Framework;

namespace Mapex.Tests
{
	[TestFixture]
	public class When_constructing_a_mapex_engine
	{
		[Test]
		public void It_should_throw_an_exception_when_the_config_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new MapexEngine(null));
		}

		[Test]
		public void It_should_throw_an_exception_when_the_document_processor_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new MapexEngine(
				cfg => { },
				null,
				new Mock<IDirectiveDeserializerFactory>().Object)
			);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_directive_deserializer_factory_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new MapexEngine(
				cfg => { },
				new Mock<IDocumentProcessor>().Object,
				null)
			);
		}

		[Test]
		public void It_should_provide_a_mapex_configuration_instance()
		{
			new MapexEngine(cfg =>
			{
				Assert.IsInstanceOf<MapexEngineConfiguration>(cfg);
				cfg.Loader = new Mock<IDirectiveLoader>().Object;
			});
		}

		[Test]
		public void It_should_create_a_directive_deserializer_with_the_tag_map()
		{
			var factory = new Mock<IDirectiveDeserializerFactory>();
			TagMap expectedTagMap = null;

			new MapexEngine(
				cfg =>
				{
					cfg.Loader = new Mock<IDirectiveLoader>().Object;
					expectedTagMap = cfg.Tags;
				},
				new Mock<IDocumentProcessor>().Object,
				factory.Object
			);

			factory.Verify(f => f.Build(expectedTagMap));
		}

		[Test]
		public void It_should_register_the_default_tags()
		{
			TagMap tags = null;

			new MapexEngine(
				cfg =>
				{
					cfg.Loader = new Mock<IDirectiveLoader>().Object;
					tags = cfg.Tags;
				},
				new Mock<IDocumentProcessor>().Object,
				new Mock<IDirectiveDeserializerFactory>().Object
			);

			CollectionAssert.Contains(tags, new KeyValuePair<string, object>("!default_filter", typeof(FilterSpecification)));
			CollectionAssert.Contains(tags, new KeyValuePair<string, object>("!default_mapper", typeof(MapSpecification)));
			CollectionAssert.Contains(tags, new KeyValuePair<string, object>("!default_transformer", typeof(TransformSpecification)));
		}
	}

	[TestFixture]
	public class When_calling_process_on_mapex_engine
	{
		private Mock<IDocument> _Document;
		private Metadata _Metadata;
		private Mock<IDirectiveLoader> _Loader;
		private MapexEngine _Engine;
		private Mock<IDocumentProcessor> _DocumentProcessor;
		private Mock<IDirectiveDeserializerFactory> _DirectiveDeserializerFactory;
		private Mock<IDirectiveDeserializer> _DirectiveDeserializer;

		[SetUp]
		public void Setup()
		{
			_Metadata = new Metadata();

			_Document = new Mock<IDocument>();
			_Document.SetupGet(d => d.Metadata).Returns(_Metadata);

			_DirectiveDeserializer = new Mock<IDirectiveDeserializer>();
			_DocumentProcessor = new Mock<IDocumentProcessor>();

			_DirectiveDeserializerFactory = new Mock<IDirectiveDeserializerFactory>();
			_DirectiveDeserializerFactory.Setup(f => f.Build(It.IsAny<TagMap>())).Returns(_DirectiveDeserializer.Object);

			_Loader = new Mock<IDirectiveLoader>();

			_Engine = new MapexEngine(
				cfg => cfg.Loader = _Loader.Object,
				_DocumentProcessor.Object,
				_DirectiveDeserializerFactory.Object
			);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_document_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => _Engine.Process(null));
		}

		[Test]
		public void It_should_return_an_empty_enumerable_of_results_when_no_directives_were_loaded()
		{
			var results = _Engine.Process(_Document.Object);

			Assert.IsNotNull(results);
			CollectionAssert.IsEmpty(results);
		}

		[Test]
		public void It_should_deserialize_the_loaded_directives()
		{
			var directives = StubLoaderWithDirectives();

			_Engine.Process(_Document.Object);

			_DirectiveDeserializer.Verify(d => d.Deserialize(directives));
		}

		[Test]
		public void It_should_call_process_on_the_document_processor_for_enabled_directives()
		{
			var directives = StubLoaderWithDirectives();

			var directive1 = new Directive { Enabled = false };
			var directive2 = new Directive { Enabled = true };

			var deserialized = new[] { directive1, directive2 };

			_DirectiveDeserializer.Setup(d => d.Deserialize(directives)).Returns(deserialized);

			_Engine.Process(_Document.Object);

			_DocumentProcessor.Verify(p => p.Process(_Document.Object, new[] { directive2 }));
		}

		private SerializedDirective[] StubLoaderWithDirectives()
		{
			var directives = new[]
			{
				new SerializedDirective { Data = "---" },
				new SerializedDirective { Data = "---" }
			};

			_Loader.Setup(l => l.Load(_Metadata)).Returns(directives);

			return directives;
		}
	}
}

using System;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace Mapex.Tests
{
	[TestFixture]
	public class When_calling_process_on_document_processor
	{
		private const string DocumentId = "123-123";
		private const int DirectiveId = 2;
		private Mock<IDocument> _Document;
		private DocumentProcessor _DocumentProcessor;

		[SetUp]
		public void Setup()
		{
			_Document = new Mock<IDocument>();
			_Document.SetupGet(d => d.Id).Returns(DocumentId);
			_DocumentProcessor = new DocumentProcessor();
		}

		[Test]
		public void It_should_throw_an_exception_when_the_document_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => _DocumentProcessor.Process(null, Enumerable.Empty<IDirective>()));
		}

		[Test]
		public void It_should_throw_an_exception_when_the_directives_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => _DocumentProcessor.Process(_Document.Object, null));
		}

		[Test]
		public void It_should_call_process_on_the_directives_that_can_process_the_document()
		{
			var directive1 = CreateDirectiveThatCanProcessDocument(false);
			var directive2 = CreateDirectiveThatCanProcessDocument(true);

			_DocumentProcessor.Process(_Document.Object, new [] { directive1.Object, directive2.Object });

			directive1.Verify(d => d.CanProcess(_Document.Object));
			directive1.VerifyNoOtherCalls();

			directive2.Verify(d => d.CanProcess(_Document.Object));
			directive2.Verify(d => d.Process(_Document.Object));
		}

		[Test]
		public void It_should_return_a_list_of_results_from_each_directive()
		{
			var result = new MapexResult();

			var directive = CreateDirectiveThatCanProcessDocument(true);
			directive.Setup(d => d.Process(_Document.Object)).Returns(result);

			var results = _DocumentProcessor.Process(_Document.Object, new [] { directive.Object });

			CollectionAssert.Contains(results, result);
		}
		
		private Mock<IDirective> CreateDirectiveThatCanProcessDocument(bool canProcessDocument)
		{
			var directive = new Mock<IDirective>();
			directive.SetupGet(d => d.Id).Returns(DirectiveId);
			directive.Setup(d => d.CanProcess(_Document.Object)).Returns(canProcessDocument);
			return directive;
		}
	}
}

using System;
using System.Dynamic;
using System.Linq;
using Mapex.Specifications;
using Mapex.Tests.Stubs;
using Moq;
using Notus;
using NUnit.Framework;

namespace Mapex.Tests
{
	[TestFixture]
	public class When_constructing_a_directive
	{
		[Test]
		public void It_should_set_the_filter_specification_to_a_null_filter_specification()
		{
			var directive = new Directive();
			Assert.IsInstanceOf<NullFilterSpecification>(directive.Filter);
		}

		[Test]
		public void It_should_set_the_transform_specification_to_a_null_transform_specification()
		{
			var directive = new Directive();
			Assert.IsInstanceOf<NullTransformSpecification>(directive.Transform);
		}

		[Test]
		public void It_should_set_the_intercept_specification_to_a_null_intercept_specification()
		{
			var directive = new Directive();
			Assert.IsInstanceOf<NullInterceptSpecification>(directive.Intercept.ElementAt(0));
		}
	}

	[TestFixture]
	public class When_calling_can_process_on_directive
	{
		private Mock<IDocument> _Document;
		private Mock<IWhereSpecification> _WhereSpecification;
		private Directive _Directive;

		[SetUp]
		public void Setup()
		{
			_Document = new Mock<IDocument>();
			_WhereSpecification = new Mock<IWhereSpecification>();

			_Directive = new Directive
			{
				Where = _WhereSpecification.Object
			};
		}

		[Test]
		public void It_should_throw_an_exception_when_the_document_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => _Directive.CanProcess(null));
		}

		[Test]
		public void It_should_return_true_when_the_where_specification_matches_the_document()
		{
			_WhereSpecification.Setup(s => s.Matches(_Document.Object)).Returns(true);
			Assert.IsTrue(_Directive.CanProcess(_Document.Object));
		}

		[Test]
		public void It_should_return_false_when_the_where_specification_does_not_match_the_document()
		{
			_WhereSpecification.Setup(s => s.Matches(_Document.Object)).Returns(false);
			Assert.IsFalse(_Directive.CanProcess(_Document.Object));
		}
	}

	[TestFixture]
	public class When_calling_process_on_directive
	{
		private const int DirectiveId = 2;
		private const string DocumentId = "123-123";
		private Mock<IDocument> _Document;
		private Directive _Directive;
		private Mock<IExtractSpecification> _ExtractSpecification;
		private Mock<IFilterSpecification> _FilterSpecification;
		private Mock<ITransformSpecification> _TransformSpecification;
		private Mock<IMapSpecification> _MapSpecification;
		private Mock<IInterceptSpecification> _InterceptSpecification1;
		private Mock<IInterceptSpecification> _InterceptSpecification2;

		[SetUp]
		public void Setup()
		{
			_Document = new Mock<IDocument>();
			_Document.SetupGet(d => d.Id).Returns(DocumentId);

			_ExtractSpecification = new Mock<IExtractSpecification>();
			_FilterSpecification = new Mock<IFilterSpecification>();
			_TransformSpecification = new Mock<ITransformSpecification>();
			_InterceptSpecification1 = new Mock<IInterceptSpecification>();
			_InterceptSpecification2 = new Mock<IInterceptSpecification>();

			_MapSpecification = new Mock<IMapSpecification>();
			_MapSpecification.SetupGet(s => s.As).Returns("Mapex.Tests.Stubs.Stub, Mapex.Tests");

			_Directive = new Directive
			{
				Id = 2,
				Extract = _ExtractSpecification.Object,
				Filter = _FilterSpecification.Object,
				Transform = _TransformSpecification.Object,
				Map = _MapSpecification.Object,
				Intercept = new [] { 
					_InterceptSpecification1.Object,
					_InterceptSpecification2.Object
				}
			};
		}

		[Test]
		public void It_should_throw_an_exception_when_the_document_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => _Directive.Process(null));
		}

		[Test]
		public void It_should_return_the_directive_result_with_the_type_loaded()
		{
			var result = _Directive.Process(_Document.Object);
			Assert.AreEqual(typeof(Stub), result.Type);
		}

		[Test]
		public void It_should_extract_the_data_from_the_document()
		{
			_Directive.Process(_Document.Object);
			_ExtractSpecification.Verify(s => s.Process(_Document.Object));
		}

		[Test]
		public void It_should_intercept_and_filter_the_extracted_data_from_the_document()
		{
			var objects = new[] { new ExpandoObject() };

			_ExtractSpecification.Setup(s => s.Process(_Document.Object)).Returns(objects);
			_InterceptSpecification1.Setup(i => i.BeforeFilter(objects)).Returns(objects);
			_InterceptSpecification2.Setup(i => i.BeforeFilter(objects)).Returns(objects);
			_InterceptSpecification1.Setup(i => i.AfterFilter(objects)).Returns(objects);
			_FilterSpecification.Setup(f => f.Process(objects)).Returns(objects);

			_Directive.Process(_Document.Object);

			_InterceptSpecification1.Verify(i => i.BeforeFilter(objects));
			_InterceptSpecification2.Verify(i => i.BeforeFilter(objects));

			_FilterSpecification.Verify(f => f.Process(objects));
			
			_InterceptSpecification1.Verify(i => i.AfterFilter(objects));
			_InterceptSpecification2.Verify(i => i.AfterFilter(objects));
		}

		[Test]
		public void It_should_intercept_and_transform_the_extracted_data()
		{
			var objects = new[] { new ExpandoObject() };
			_ExtractSpecification.Setup(s => s.Process(_Document.Object)).Returns(objects);
			_InterceptSpecification1.Setup(i => i.BeforeFilter(objects)).Returns(objects);
			_InterceptSpecification2.Setup(i => i.BeforeFilter(objects)).Returns(objects);
			
			_FilterSpecification.Setup(f => f.Process(objects)).Returns(objects);

			_InterceptSpecification1.Setup(i => i.AfterFilter(objects)).Returns(objects);
			_InterceptSpecification2.Setup(i => i.AfterFilter(objects)).Returns(objects);
			_InterceptSpecification1.Setup(i => i.BeforeTransform(objects)).Returns(objects);
			_InterceptSpecification2.Setup(i => i.BeforeTransform(objects)).Returns(objects);
			
			_Directive.Process(_Document.Object);

			_InterceptSpecification1.Verify(i => i.BeforeTransform(objects));
			_InterceptSpecification2.Verify(i => i.BeforeTransform(objects));
			_TransformSpecification.Verify(t => t.Process(objects));
		}

		[Test]
		public void It_should_intercept_and_map_the_objects_to_the_map_type()
		{
			var objects = new[] { new ExpandoObject() };
			_ExtractSpecification.Setup(s => s.Process(_Document.Object)).Returns(objects);
			_InterceptSpecification1.Setup(i => i.BeforeFilter(objects)).Returns(objects);
			_InterceptSpecification2.Setup(i => i.BeforeFilter(objects)).Returns(objects);
			_FilterSpecification.Setup(f => f.Process(objects)).Returns(objects);
			_InterceptSpecification1.Setup(i => i.AfterFilter(objects)).Returns(objects);
			_InterceptSpecification2.Setup(i => i.AfterFilter(objects)).Returns(objects);
			_InterceptSpecification1.Setup(i => i.BeforeTransform(objects)).Returns(objects);
			_InterceptSpecification2.Setup(i => i.BeforeTransform(objects)).Returns(objects);
			_InterceptSpecification1.Setup(i => i.BeforeMap(objects)).Returns(objects);
			_InterceptSpecification2.Setup(i => i.BeforeMap(objects)).Returns(objects);
			
			_Directive.Process(_Document.Object);

			_InterceptSpecification1.Verify(i => i.BeforeMap(objects));
			_InterceptSpecification2.Verify(i => i.BeforeMap(objects));
			_MapSpecification.Verify(s => s.Process(objects));
		}

		[Test]
		public void It_should_return_a_result_with_directive_id_and_document_id()
		{
			var objects = new[] { new ExpandoObject() };
			_ExtractSpecification.Setup(s => s.Process(_Document.Object)).Returns(objects);
			_InterceptSpecification1.Setup(i => i.BeforeFilter(objects)).Returns(objects);
			_FilterSpecification.Setup(f => f.Process(objects)).Returns(objects);
			_InterceptSpecification1.Setup(i => i.AfterFilter(objects)).Returns(objects);
			_InterceptSpecification1.Setup(i => i.BeforeTransform(objects)).Returns(objects);
			_InterceptSpecification1.Setup(i => i.BeforeMap(objects)).Returns(objects);
			_MapSpecification.Setup(s => s.Process(objects)).Returns(objects);
			
			var result = _Directive.Process(_Document.Object);

			Assert.AreEqual(DocumentId, result.DocumentId);
			Assert.AreEqual(DirectiveId, result.DirectiveId);
		}
	}

	[TestFixture]
	public class When_calling_process_on_directive_and_processing_is_successful
	{
		private MapexResult _MapexResult;

		[SetUp]
		public void Setup()
		{
			var document = new Mock<IDocument>();
			document.SetupGet(d => d.Id).Returns("1");

			var extractSpecification = new Mock<IExtractSpecification>();
			extractSpecification.Setup(s => s.Process(document.Object)).Returns(new[] {new ExpandoObject()});

			var directive = new Directive
			{
				Id = 2,
				Extract = extractSpecification.Object,
				Map = { As = "Mapex.Tests.Stubs.Stub, Mapex.Tests" }
			};
			
			_MapexResult = directive.Process(document.Object);
		}

		[Test]
		public void It_should_set_the_outcome_to_processed()
		{
			Assert.AreEqual(MapexOutcome.Processed, _MapexResult.Outcome);
		}

		[Test]
		public void It_should_set_the_reason_to_processed()
		{
			Assert.AreEqual("Document processed", _MapexResult.Reason);
		}
	}

	[TestFixture]
	public class When_calling_process_on_directive_and_processing_resulted_in_no_data
	{
		private MapexResult _MapexResult;

		[SetUp]
		public void Setup()
		{
			var document = new Mock<IDocument>();
			document.SetupGet(d => d.Id).Returns("1");

			var extractSpecification = new Mock<IExtractSpecification>();
			extractSpecification.Setup(s => s.Process(document.Object)).Returns(Enumerable.Empty<ExpandoObject>());

			var directive = new Directive
			{
				Id = 2,
				Extract = extractSpecification.Object,
				Map = { As = "Mapex.Tests.Stubs.Stub, Mapex.Tests" }
			};
			
			_MapexResult = directive.Process(document.Object);
		}

		[Test]
		public void It_should_set_the_outcome_to_ignored()
		{
			Assert.AreEqual(MapexOutcome.Ignored, _MapexResult.Outcome);
		}

		[Test]
		public void It_should_set_the_reason_to_no_results()
		{
			Assert.AreEqual("Document processed - no results", _MapexResult.Reason);
		}
	}

	[TestFixture]
	public class When_calling_process_on_directive_and_processing_results_in_an_exception
	{
		private MapexResult _MapexResult;

		[SetUp]
		public void Setup()
		{
			var document = new Mock<IDocument>();
			document.SetupGet(d => d.Id).Returns("1");

			var extractSpecification = new Mock<IExtractSpecification>();
			extractSpecification.Setup(s => s.Process(document.Object)).Throws(new Exception("error-message"));

			var directive = new Directive
			{
				Id = 2,
				Extract = extractSpecification.Object,
				Map = { As = "Mapex.Tests.Stubs.Stub, Mapex.Tests" }
			};
			
			_MapexResult = directive.Process(document.Object);
		}

		[Test]
		public void It_should_set_the_outcome_to_failed()
		{
			Assert.AreEqual(MapexOutcome.Failed, _MapexResult.Outcome);
		}

		[Test]
		public void It_should_set_the_reason_with_the_exception_message()
		{
			Assert.That(_MapexResult.Reason.Contains("Failed to process document 1 using directive 2. System.Exception: error-message"));
		}
	}

	[TestFixture]
	public class When_calling_validate_on_directive
	{
		private Directive _Directive;
		private Mock<IWhereSpecification> _WhereSpecification;
		private Mock<IExtractSpecification> _ExtractSpecification;
		private Mock<IFilterSpecification> _FilterSpecification;
		private Mock<ITransformSpecification> _TransformSpecification;
		private Notification _Notification;
		private Mock<IMapSpecification> _MapSpecification;
		private Mock<IInterceptSpecification> _InterceptSpecification1;
		private Mock<IInterceptSpecification> _InterceptSpecification2;

		[SetUp]
		public void Setup()
		{
			_WhereSpecification = new Mock<IWhereSpecification>();
			_ExtractSpecification = new Mock<IExtractSpecification>();
			_FilterSpecification = new Mock<IFilterSpecification>();
			_TransformSpecification = new Mock<ITransformSpecification>();
			_MapSpecification = new Mock<IMapSpecification>();
			_InterceptSpecification1 = new Mock<IInterceptSpecification>();
			_InterceptSpecification2 = new Mock<IInterceptSpecification>();

			_Directive = new Directive
			{
				Where = _WhereSpecification.Object,
				Extract = _ExtractSpecification.Object,
				Filter = _FilterSpecification.Object,
				Transform = _TransformSpecification.Object,
				Map = _MapSpecification.Object,
				Intercept = new [] { 
					_InterceptSpecification1.Object, 
					_InterceptSpecification2.Object 
				}
			};
		}

		[Test]
		public void It_should_add_an_error_notification_when_the_where_property_is_not_set()
		{
			_Directive.Where = null;

			_Notification = _Directive.Validate();

			Assert.IsTrue(_Notification.IncludesError("The Where property of the directive is not set."));
		}

		[Test]
		public void It_should_add_an_error_notification_when_the_extract_property_is_not_set()
		{
			_Directive.Extract = null;

			_Notification = _Directive.Validate();

			Assert.IsTrue(_Notification.IncludesError("The Extract property of the directive is not set."));
		}

		[Test]
		public void It_should_add_an_error_notification_when_the_filter_property_is_not_set()
		{
			_Directive.Filter = null;

			_Notification = _Directive.Validate();

			Assert.IsTrue(_Notification.IncludesError("The Filter property of the directive is not set."));
		}

		[Test]
		public void It_should_add_an_error_notification_when_the_transform_property_is_not_set()
		{
			_Directive.Transform = null;

			_Notification = _Directive.Validate();

			Assert.IsTrue(_Notification.IncludesError("The Transform property of the directive is not set."));
		}

		[Test]
		public void It_should_add_an_error_notification_when_the_map_property_is_not_set()
		{
			_Directive.Map = null;

			_Notification = _Directive.Validate();

			Assert.IsTrue(_Notification.IncludesError("The Map property of the directive is not set."));
		}

		[Test]
		public void It_should_add_an_error_notification_when_the_intercept_property_is_not_set()
		{
			_Directive.Intercept = null;

			_Notification = _Directive.Validate();

			Assert.IsTrue(_Notification.IncludesError("The Intercept property of the directive is not set."));
		}

		[Test]
		public void It_should_call_validate_on_the_where_specification()
		{
			_Notification = _Directive.Validate();

			_WhereSpecification.Verify(s => s.Validate(It.IsAny<Notification>()));
		}

		[Test]
		public void It_should_call_validate_on_the_extract_specification()
		{
			_Notification = _Directive.Validate();

			_ExtractSpecification.Verify(s => s.Validate(It.IsAny<Notification>()));
		}

		[Test]
		public void It_should_call_validate_on_the_filter_specification()
		{
			_Notification = _Directive.Validate();

			_FilterSpecification.Verify(s => s.Validate(It.IsAny<Notification>()));
		}

		[Test]
		public void It_should_call_validate_on_the_transform_specification()
		{
			_Notification = _Directive.Validate();

			_TransformSpecification.Verify(s => s.Validate(It.IsAny<Notification>()));
		}

		[Test]
		public void It_should_call_validate_on_the_map_specification()
		{
			_Notification = _Directive.Validate();

			_MapSpecification.Verify(s => s.Validate(It.IsAny<Notification>()));
		}

		[Test]
		public void It_should_call_validate_on_the_intercept_specifications()
		{
			_Notification = _Directive.Validate();

			_InterceptSpecification1.Verify(s => s.Validate(It.IsAny<Notification>()));
			_InterceptSpecification2.Verify(s => s.Validate(It.IsAny<Notification>()));
		}
	}
}

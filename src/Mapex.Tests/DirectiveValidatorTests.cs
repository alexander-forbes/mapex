using System;
using Moq;
using Notus;
using NUnit.Framework;

namespace Mapex.Tests
{
	[TestFixture]
	public class When_calling_validate_on_directive_validator
	{
		private DirectiveValidator _Validator;

		[SetUp]
		public void Setup()
		{
			_Validator = new DirectiveValidator();
		}

		[Test]
		public void It_should_throw_an_exception_when_the_directives_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new DirectiveValidator().Validate(null));
		}

		[Test]
		public void It_should_throw_an_exception_when_a_directive_is_invalid()
		{
			var notification = new Notification();
			notification.AddError("test-error-1");
			notification.AddError("test-error-2");

			var directives = new[]
			{
			CreateDirective(notification).Object
		};

			var exception = Assert.Throws<Exception>(() => _Validator.Validate(directives));

			Assert.AreEqual(
				$"Directive 0 is invalid because of the following errors:{Environment.NewLine}test-error-1,{Environment.NewLine}test-error-2.",
				exception.Message);
		}

		[Test]
		public void It_should_not_throw_an_exception_when_all_directives_are_valid()
		{
			var notification = new Notification();

			var directives = new[]
			{
			CreateDirective(notification).Object
		};

			Assert.DoesNotThrow(() => _Validator.Validate(directives));
		}

		private static Mock<IDirective> CreateDirective(Notification notification)
		{
			var directive = new Mock<IDirective>();
			directive.Setup(d => d.Validate()).Returns(notification);
			return directive;
		}
	}
}

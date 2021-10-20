using System;
using System.Linq;
using Mapex.Deserialization;
using Moq;
using NUnit.Framework;

namespace Mapex.Tests.Deserialization
{
	[TestFixture]
	public class When_constructing_a_directive_deserializer
	{
		[Test]
		public void It_should_throw_an_exception_when_the_validator_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() =>
				new DirectiveDeserializer(null, new Mock<IYamlDeserializer>().Object)
			);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_yaml_deserializer_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() =>
				new DirectiveDeserializer(new Mock<IDirectiveValidator>().Object, null)
			);
		}
	}

	[TestFixture]
	public class When_calling_deserialize_on_directive_deserializer
	{
		private Mock<IDirectiveValidator> _Validator;
		private Mock<IYamlDeserializer> _YamlDeserializer;
		private DirectiveDeserializer _Deserializer;
		private SerializedDirective[] _Directives;

		[SetUp]
		public void Setup()
		{
			_Directives = new[]
			{
				new SerializedDirective { Id = 1, Data = "prop1: val1", Enabled = false },
				new SerializedDirective { Id = 2, Data = "prop2: val2", Enabled = true }
			};

			_Validator = new Mock<IDirectiveValidator>();
			_YamlDeserializer = new Mock<IYamlDeserializer>();
			_Deserializer = new DirectiveDeserializer(_Validator.Object, _YamlDeserializer.Object);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_directives_parameter_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => _Deserializer.Deserialize(null));
		}

		[Test]
		public void It_should_deserialize_all_the_directives()
		{
			var deserialized = new[]
			{
				new Directive(),
				new Directive()
			};

			_YamlDeserializer.Setup(d => d.Deserialize<Directive>(_Directives.First().Data)).Returns(deserialized.First());
			_YamlDeserializer.Setup(d => d.Deserialize<Directive>(_Directives.Last().Data)).Returns(deserialized.Last());

			var directives = _Deserializer.Deserialize(_Directives);

			Assert.IsTrue(directives.Any(d => d.Id == 1 && !d.Enabled));
			Assert.IsTrue(directives.Any(d => d.Id == 2 && d.Enabled));
		}

		[Test]
		public void It_should_validate_the_deserialized_directives()
		{
			var deserialized = new[]
			{
				new Directive(),
				new Directive()
			};

			_YamlDeserializer.Setup(d => d.Deserialize<Directive>(_Directives.First().Data)).Returns(deserialized.First());
			_YamlDeserializer.Setup(d => d.Deserialize<Directive>(_Directives.Last().Data)).Returns(deserialized.Last());

			_Deserializer.Deserialize(_Directives);

			_Validator.Verify(v => v.Validate(deserialized));
		}
	}

	[TestFixture]
	public class When_calling_deserialize_on_directive_deserializer_with_an_invalid_directive
	{
		private Mock<IDirectiveValidator> _Validator;
		private Mock<IYamlDeserializer> _YamlDeserializer;
		private DirectiveDeserializer _Deserializer;
		private SerializedDirective[] _Directives;

		[SetUp]
		public void Setup()
		{
			_Directives = new[]
			{
				new SerializedDirective { Id = 1, Data = "\t is invalid" }
			};

			_Validator = new Mock<IDirectiveValidator>();
			_YamlDeserializer = new Mock<IYamlDeserializer>();
			_Deserializer = new DirectiveDeserializer(_Validator.Object, _YamlDeserializer.Object);
		}

		[Test] 
		public void It_should_throw_a_directive_exception_when_the_directive_is_invalid()
		{
			_YamlDeserializer.Setup(d => d.Deserialize<Directive>(_Directives.First().Data)).Throws(new Exception("error-message"));
			var exception = Assert.Throws<DirectiveException>(() => _Deserializer.Deserialize(_Directives));
			Assert.AreEqual(1, exception.DirectiveId);
			Assert.AreEqual("error-message", exception.Message);
		}
	}
}

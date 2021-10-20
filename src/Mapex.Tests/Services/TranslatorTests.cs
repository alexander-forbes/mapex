using System;
using System.Dynamic;
using Mapex.Services;
using Mapex.Specifications;
using NUnit.Framework;

namespace Mapex.Tests.Services
{
	[TestFixture]
	public class When_calling_apply_on_translator
	{
		[Test]
		public void It_should_not_do_anything_when_a_translation_specification_has_not_been_provided()
		{
			dynamic obj = new ExpandoObject();
			obj.Name = "Joe";

			var specification = new PropertyTransformSpecification
			{
				Property = "Name",
				Translate = null
			};

			var translator = new Translator();
			Assert.DoesNotThrow(() => translator.Apply(specification, obj));
		}

		[Test]
		public void It_should_translate_the_values()
		{
			dynamic obj = new ExpandoObject();
			obj.Name = "Joe";

			var specification = new PropertyTransformSpecification
			{
				Property = "Name",
				Translate = new [] {
					new TranslateSpecification
					{
						When = "Doe",
						Then = "Soap"
					},
					new TranslateSpecification
					{
						When = "Joe",
						Then = "Jane"
					}
				}
			};

			var translator = new Translator();
			translator.Apply(specification, obj);

			Assert.AreEqual("Jane", obj.Name);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_object_does_not_contain_the_field_to_translate()
		{
			dynamic obj = new ExpandoObject();
			obj.Name = "Joe";

			var specification = new PropertyTransformSpecification
			{
				Property = "FirstName",
				Translate = new[] {
					new TranslateSpecification
					{
						When = "Joe",
						Then = "Jane"
					}
				}
			};

			var translator = new Translator();
			var exception = Assert.Throws<Exception>(() => translator.Apply(specification, obj));
			Assert.AreEqual("Object does not have field FirstName.", exception.Message);
		}

		[TestCase("", TestName = "It_should_translate_the_values_when_the_field_is_empty")]
		[TestCase(null, TestName = "It_should_translate_the_values_when_the_field_is_null")]
		public void It_should_translate_the_values_when_the_field_is_empty_or_null(object value)
		{
			dynamic obj = new ExpandoObject();
			obj.Name = value;

			var specification = new PropertyTransformSpecification
			{
				Property = "Name",
				Translate = new [] {
					new TranslateSpecification
					{
						When = "[empty]",
						Then = "Soap"
					}
				}
			};

			var translator = new Translator();
			translator.Apply(specification, obj);

			Assert.AreEqual("Soap", obj.Name);
		}
	}
}

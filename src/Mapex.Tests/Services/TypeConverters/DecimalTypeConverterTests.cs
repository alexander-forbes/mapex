using System;

using Mapex.Services.TypeConverters;
using Mapex.Specifications;

using NUnit.Framework;

namespace Mapex.Tests.Services.TypeConverters
{
	[TestFixture]
	public class When_calling_convert_on_decimal_type_converter
	{
		[Test]
		public void It_should_convert_decimal_with_a_period_separator()
		{
			const string value = "220.56";

			var specification = new PropertyTransformSpecification
			{
				Property = "Value",
				Convert = new ConvertSpecification
				{
					To = "System.Decimal, mscorlib"
				}
			};

			var converter = new DecimalTypeConverter();
			var result = converter.Convert(value, specification);

			Assert.AreEqual(220.56m, result);
		}

		[Test]
		public void It_should_convert_decimal_with_a_comma_separator()
		{
			const string value = "230,82.42";

			var specification = new PropertyTransformSpecification
			{
				Property = "Value",
				Convert = new ConvertSpecification
				{
					Format = "en",
					To = "System.Decimal, mscorlib"
				}
			};

			var converter = new DecimalTypeConverter();
			var result = converter.Convert(value, specification);

			Assert.AreEqual(23082.42m, result);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_decimal_cannot_be_converted()
		{
			const string value = "382.xx";

			var specification = new PropertyTransformSpecification
			{
				Property = "Value",
				Convert = new ConvertSpecification
				{
					To = "System.Decimal, mscorlib",
				}
			};

			var converter = new DecimalTypeConverter();
			var exception = Assert.Throws<Exception>(() => converter.Convert(value, specification));
			Assert.AreEqual("Failed to convert decimal 382.xx.", exception.Message);
		}
	}
}

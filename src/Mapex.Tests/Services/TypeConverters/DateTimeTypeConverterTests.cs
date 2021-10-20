using System;
using Mapex.Services.TypeConverters;
using Mapex.Specifications;
using NUnit.Framework;

namespace Mapex.Tests.Services.TypeConverters
{
	[TestFixture]
	public class When_calling_convert_on_date_time_type_converter
	{
		[Test]
		public void It_should_convert_date_using_the_given_format()
		{
			const string date = "12/02/2018";

			var specification = new PropertyTransformSpecification
			{
				Property = "Date",
				Convert = new ConvertSpecification
				{
					To = "System.DateTime, mscorlib",
					Format = "dd/MM/yyyy"
				}
			};

			var converter = new DateTimeTypeConverter();
			var result = converter.Convert(date, specification);

			Assert.AreEqual(new DateTime(2018, 02, 12), result);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_date_cannot_be_converted_using_the_date_format()
		{
			const string date = "12/02/2018";

			var specification = new PropertyTransformSpecification
			{
				Property = "Date",
				Convert = new ConvertSpecification
				{
					To = "System.DateTime, mscorlib",
					Format = "yyyy-MM-dd"
				}
			};

			var converter = new DateTimeTypeConverter();
			var exception = Assert.Throws<Exception>(() => converter.Convert(date, specification));
			Assert.AreEqual("Failed to convert date 12/02/2018 using the format yyyy-MM-dd.", exception.Message);
		}
	}
}

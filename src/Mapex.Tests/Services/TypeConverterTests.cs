using System;
using System.Dynamic;
using Mapex.Services;
using Mapex.Specifications;
using NUnit.Framework;

namespace Mapex.Tests.Services
{
	[TestFixture]
	public class When_calling_apply_on_type_converter
	{
		[Test]
		public void It_should_not_do_anything_if_a_convert_specification_is_not_present()
		{
			var target = new ExpandoObject();

			var specification = new PropertyTransformSpecification
			{
				Convert = null
			};

			var converter = new TypeConverter();
			Assert.DoesNotThrow(() => converter.Apply(specification, target));
		}

		[Test]
		public void It_should_convert_dates_using_the_given_format()
		{
			dynamic obj = new ExpandoObject();
			obj.Date = "12/02/2018";

			var specification = new PropertyTransformSpecification
			{
				Property = "Date",
				Convert = new ConvertSpecification
				{
					To = "System.DateTime, mscorlib",
					Format = "dd/MM/yyyy"
				}
			};

			var converter = new TypeConverter();
			converter.Apply(specification, obj);

			Assert.AreEqual(new DateTime(2018, 02, 12), obj.Date);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_date_cannot_be_converted_using_the_date_format()
		{
			dynamic obj = new ExpandoObject();
			obj.Date = "12/02/2018";

			var specification = new PropertyTransformSpecification
			{
				Property = "Date",
				Convert = new ConvertSpecification
				{
					To = "System.DateTime, mscorlib",
					Format = "yyyy-MM-dd"
				}
			};

			var converter = new TypeConverter();
			var exception = Assert.Throws<Exception>(() => converter.Apply(specification, obj));
			Assert.AreEqual("Failed to convert date 12/02/2018 using the format yyyy-MM-dd.", exception.Message);
		}

		[Test]
		public void It_should_convert_decimal_values()
		{
			dynamic obj = new ExpandoObject();
			obj.Amount = "123.90";

			var specification = new PropertyTransformSpecification
			{
				Property = "Amount",
				Convert = new ConvertSpecification
				{
					To = "System.Decimal, mscorlib"
				}
			};

			var converter = new TypeConverter();
			converter.Apply(specification, obj);

			Assert.AreEqual(123.90M, obj.Amount);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_object_does_not_contain_the_field_to_convert()
		{
			dynamic obj = new ExpandoObject();
			obj.Amount = "123456.78";

			var specification = new PropertyTransformSpecification
			{
				Property = "Value",
				Convert = new ConvertSpecification
				{
					To = "System.Decimal, mscorlib"
				}
			};

			var converter = new TypeConverter();
			var exception = Assert.Throws<Exception>(() => converter.Apply(specification, obj));
			Assert.AreEqual("Object does not have field Value.", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_field_is_null_and_the_destination_type_is_not_nullable()
		{
			dynamic obj = new ExpandoObject();
			obj.Value = null;

			var specification = new PropertyTransformSpecification
			{
				Property = "Value",
				Convert = new ConvertSpecification
				{
					To = "System.Decimal, mscorlib"
				}
			};

			var converter = new TypeConverter();
			var exception = Assert.Throws<Exception>(() => converter.Apply(specification, obj));
			Assert.AreEqual("Current value is null, but the destination type is neither Nullable nor Nullable<>", exception.Message);
		}

		[TestCase("", TestName = "It_should_set_the_value_to_null_when_the_field_is_empty_and_the_destination_type_is_nullable")]
		[TestCase(null, TestName = "It_should_set_the_value_to_null_when_the_field_is_null_and_the_destination_type_is_nullable")]
		public void It_should_set_the_value_to_null_when_the_field_is_null_or_empty_and_the_destination_type_is_nullable(object value)
		{
			dynamic obj = new ExpandoObject();
			obj.Value = value;

			var specification = new PropertyTransformSpecification
			{
				Property = "Value",
				Convert = new ConvertSpecification
				{
					To = "System.Nullable`1[System.Decimal], mscorlib"
				}
			};

			var converter = new TypeConverter();
			converter.Apply(specification, obj);

			Assert.IsNull(obj.Value);
		}

		[Test]
		public void It_should_set_the_value_to_to_the_converted_value_when_the_destination_type_is_nullable()
		{
			dynamic obj = new ExpandoObject();
			obj.Value = "200.56";

			var specification = new PropertyTransformSpecification
			{
				Property = "Value",
				Convert = new ConvertSpecification
				{
					To = "System.Nullable`1[System.Decimal], mscorlib"
				}
			};

			var converter = new TypeConverter();
			converter.Apply(specification, obj);

			Assert.AreEqual(200.56M, obj.Value);
		}

		[TestCase(null, TestName = "It_should_set_the_date_value_to_null_when_the_source_value_is_null_and_the_underlying_type_is_nullable_date_time")]
		[TestCase("", TestName = "It_should_set_the_date_value_to_null_when_the_source_value_is_empty_and_the_underlying_type_is_nullable_date_time")]
		public void It_should_handle_null_and_empty_values_when_converting_the_date_value(object date)
		{
			dynamic obj = new ExpandoObject();
			obj.Date = date;

			var specification = new PropertyTransformSpecification
			{
				Property = "Date",
				Convert = new ConvertSpecification
				{
					To = "System.Nullable`1[System.DateTime], mscorlib",
					Format = "dd/MM/yyyy"
				}
			};

			var converter = new TypeConverter();
			converter.Apply(specification, obj);

			Assert.IsNull(obj.Date);
		}

		[Test]
		public void It_should_convert_dates_using_the_given_format_when_the_underlying_type_is_nullable()
		{
			dynamic obj = new ExpandoObject();
			obj.Date = "20180913";

			var specification = new PropertyTransformSpecification
			{
				Property = "Date",
				Convert = new ConvertSpecification
				{
					To = "System.Nullable`1[System.DateTime], mscorlib",
					Format = "yyyyMMdd"
				}
			};

			var converter = new TypeConverter();
			converter.Apply(specification, obj);

			Assert.AreEqual(new DateTime(2018, 09, 13), obj.Date);
		}
	}
}

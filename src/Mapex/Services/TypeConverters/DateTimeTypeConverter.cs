using System;
using System.Globalization;
using Mapex.Logging;
using Mapex.Specifications;

namespace Mapex.Services.TypeConverters
{
	internal class DateTimeTypeConverter : ITypeConverter
	{
		private static readonly ILog Log = LogProvider.For<DateTimeTypeConverter>();

		public object Convert(string value, PropertyTransformSpecification specification)
		{
			if (specification == null)
				throw new ArgumentNullException(nameof(specification));

			if (specification.Convert.Format == null)
				throw new Exception("Date format has not been specified.");

			return ConvertDate(value, specification);
		}

		private static object ConvertDate(string currentValue, PropertyTransformSpecification specification)
		{
			Log.Debug($"Converting date {currentValue} to date format {specification.Convert.Format}");

			var success = DateTime.TryParseExact(currentValue, specification.Convert.Format, CultureInfo.InvariantCulture,
				DateTimeStyles.AllowWhiteSpaces, out var date);

			if (success)
				return date;

			Log.Error($"Failed to convert date {currentValue} using the format {specification.Convert.Format}.");
			throw new Exception($"Failed to convert date {currentValue} using the format {specification.Convert.Format}.");
		}
	}
}

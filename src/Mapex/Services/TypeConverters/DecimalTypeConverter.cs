using Mapex.Logging;
using Mapex.Specifications;

using System;
using System.Globalization;

namespace Mapex.Services.TypeConverters
{
	internal class DecimalTypeConverter : ITypeConverter
	{
		private static readonly ILog Log = LogProvider.For<DecimalTypeConverter>();

		public object Convert(string value, PropertyTransformSpecification specification)
		{
			if (specification == null)
				throw new ArgumentNullException(nameof(specification));

			if (string.IsNullOrEmpty(specification.Convert.Format))
			{
				if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var converted))
				{
					Log.Error($"Failed to convert decimal {value} using InvariantCulture");
					throw new Exception($"Failed to convert decimal {value} using an InvariantCulture. Try specifying a culture in the format property...");
				}
				else return converted;
			}
			else
			{
				if (!decimal.TryParse(value, NumberStyles.Any, CultureInfo.GetCultureInfo(specification.Convert.Format), out var converted))
				{
					Log.Error($"Failed to convert decimal {value} using {specification.Convert.Format} culture");
					throw new Exception($"Failed to convert decimal {value} using an {specification.Convert.Format} culture.");
				}
				else return converted;
			}
		}
	}
}

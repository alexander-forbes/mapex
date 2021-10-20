using System;
using System.Collections.Generic;
using System.Dynamic;
using Mapex.Logging;
using Mapex.Services.TypeConverters;
using Mapex.Specifications;

namespace Mapex.Services
{
	internal class TypeConverter : IPropertyTransformer
	{
		private static readonly ILog Log = LogProvider.For<TypeConverter>();

		public void Apply(PropertyTransformSpecification specification, ExpandoObject obj)
		{
			if (specification.Convert == null)
				return;

			var target = obj.ToDictionary();
			Dynamic.ThrowIfFieldMissing(target, specification.Property);

			Log.Debug($"Converting field {specification.Property} to {specification.Convert.To}");
			var convertedValue = ConvertField(target, specification);
			ReplaceField(target, convertedValue, specification);
		}

		private static object ConvertField(IDictionary<string, object> target, PropertyTransformSpecification specification)
		{
			var destinationType = Type.GetType(specification.Convert.To, throwOnError: true, ignoreCase: true);
			var typeToConvertTo = ResolveDestinationType(destinationType);

			var currentValue = FindCurrentValue(target, specification);

			if (IsNullOrEmpty(currentValue))
			{
				if (IsNullable(destinationType))
					return null;
				else
				{
					Log.Error($"Property {specification.Property} is null and the destination type is neither Nullable nor Nullable<>");
					throw new Exception("Current value is null, but the destination type is neither Nullable nor Nullable<>");
				}
			}

			return ConvertValue(specification, typeToConvertTo, currentValue);
		}

		private static object ConvertValue(PropertyTransformSpecification specification, Type typeToConvertTo, object currentValue)
		{
			if (typeToConvertTo == typeof(DateTime))
				return new DateTimeTypeConverter().Convert(currentValue.ToString(), specification);

			if (typeToConvertTo == typeof(decimal))
				return new DecimalTypeConverter().Convert(currentValue.ToString(), specification);

			return Convert.ChangeType(currentValue, typeToConvertTo);
		}

		private static bool IsNullOrEmpty(object currentValue)
		{
			return currentValue == null || string.IsNullOrEmpty(Convert.ToString(currentValue));
		}

		private static Type ResolveDestinationType(Type destinationType)
		{
			var typeToConvertTo = IsNullable(destinationType)
				? Nullable.GetUnderlyingType(destinationType)
				: destinationType;

			if (typeToConvertTo == null)
				throw new Exception($"Failed to resolve the underlying type to convert to from {destinationType}");

			return typeToConvertTo;
		}

		private static bool IsNullable(Type destinationType)
		{
			return Nullable.GetUnderlyingType(destinationType) != null;
		}

		private static object FindCurrentValue(IDictionary<string, object> target, PropertyTransformSpecification specification)
		{
			var currentValue = target[specification.Property];

			if (currentValue != null) 
				return currentValue;

			Log.Warn($"Property {specification.Property} is null.");
			return null;
		}

		private static void ReplaceField(IDictionary<string, object> target, object newValue, PropertyTransformSpecification specification)
		{
			if (target.ContainsKey(specification.Property))
			{
				Log.Debug($"Replacing {specification.Property} with converted value {newValue}");
				target.Remove(specification.Property);
				target.Add(specification.Property, newValue);
			}
		}
	}
}

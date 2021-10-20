using System;
using System.Collections.Generic;
using System.Linq;
using Mapex.Logging;
using Mapex.Specifications;

namespace Mapex
{
	internal static class Extensions
	{
		private static readonly ILog Log = LogProvider.GetLogger("Extensions");

		public static bool IsEmpty(this IEnumerable<object> objects)
		{
			if(objects == null)
				return true;

			return !objects.Any();
		}

		public static object GetCurrentPropertyValue(this IDictionary<string, object> target, PropertyTransformSpecification specification, bool throwForNullPropertyValue = true)
		{
			var currentValue = target[specification.Property];

			if (currentValue == null && throwForNullPropertyValue)
			{
				Log.Error($"Fetched a null for property {specification.Property}");
				throw new Exception($"Fetched a null for property {specification.Property}");
			}

			return currentValue;
		}
	}
}

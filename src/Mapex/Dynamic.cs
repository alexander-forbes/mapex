using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Mapex
{
	internal static class Dynamic
	{
		public static IDictionary<string, object> ToDictionary(this ExpandoObject obj)
		{
			if (obj == null)
				throw new ArgumentNullException(nameof(obj));

			return obj;
		}

		public static void ThrowIfFieldMissing(IDictionary<string, object> obj, string field)
		{
			if (obj == null)
				throw new ArgumentNullException(nameof(obj));
			
			if (string.IsNullOrEmpty(field))
				throw new ArgumentNullException(nameof(field));

			if (!obj.ContainsKey(field))
				throw new Exception($"Object does not have field {field}.");
		}
	}
}

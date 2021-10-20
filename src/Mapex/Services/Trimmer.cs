using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.RegularExpressions;
using Mapex.Logging;
using Mapex.Specifications;

namespace Mapex.Services
{
	internal class Trimmer : IPropertyTransformer
	{
		private static readonly ILog Log = LogProvider.For<Trimmer>();

		public void Apply(PropertyTransformSpecification specification, ExpandoObject obj)
		{
			if (string.IsNullOrEmpty(specification.Trim))
				return;

			var target = obj.ToDictionary();
			Dynamic.ThrowIfFieldMissing(target, specification.Property);

			var currentValue = target.GetCurrentPropertyValue(specification).ToString();

			if (specification.Trim != null)
			{
				Log.Debug($"Trimming {specification.Property} using {specification.Trim}");
				target[specification.Property] = new Regex(specification.Trim).Replace(currentValue, "");
			}
		}
	}
}

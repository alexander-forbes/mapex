using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.RegularExpressions;
using Mapex.Logging;
using Mapex.Specifications;

namespace Mapex.Services
{
	internal class Yanker : IPropertyTransformer
	{
		private static readonly ILog Log = LogProvider.For<Yanker>();

		public void Apply(PropertyTransformSpecification specification, ExpandoObject obj)
		{
			if (string.IsNullOrEmpty(specification.Yank))
				return;

			var target = obj.ToDictionary();
			Dynamic.ThrowIfFieldMissing(target, specification.Property);

			var currentValue = target.GetCurrentPropertyValue(specification).ToString();

			Log.Debug($"Yanking {specification.Property} using {specification.Yank}");

			var regex = new Regex(specification.Yank);
			
			var match = regex.Match(currentValue);

			if (match.Success && match.Groups[specification.Property].Success)
				target[specification.Property] = match.Groups[specification.Property].Value;
			else
				target[specification.Property] = currentValue;
		}
	}
}

using System;
using System.Dynamic;
using Mapex.Logging;
using Mapex.Specifications;

namespace Mapex.Services
{
	internal class Translator : IPropertyTransformer
	{
		private static readonly ILog Log = LogProvider.For<Translator>();

		public void Apply(PropertyTransformSpecification specification, ExpandoObject obj)
		{
			if (specification.Translate.IsEmpty())
				return;

			var target = obj.ToDictionary();
			Dynamic.ThrowIfFieldMissing(target, specification.Property);

			var currentValue = Convert.ToString(target.GetCurrentPropertyValue(specification, false));

			foreach (var translation in specification.Translate)
			{
				if (translation.When.Trim().Equals("[empty]", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(currentValue))
				{
					Log.Debug($"Current value is empty, replacing it with {translation.Then}");
					target[specification.Property] = translation.Then;
				}
				else if (currentValue == translation.When)
				{
					Log.Debug($"Replacing {currentValue} with {translation.Then}");
					target[specification.Property] = translation.Then;
				}
			}
		}
	}
}

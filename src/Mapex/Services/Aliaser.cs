using System;
using System.Dynamic;
using Mapex.Logging;
using Mapex.Specifications;

namespace Mapex.Services
{
	internal class Aliaser : IPropertyTransformer
	{
		private static readonly ILog Log = LogProvider.For<Aliaser>();

		public void Apply(PropertyTransformSpecification specification, ExpandoObject obj)
		{
			if (string.IsNullOrEmpty(specification.As))
				return;

			var dictionary = obj.ToDictionary();

			if (dictionary.ContainsKey(specification.As))
			{
				Log.Debug($"{specification.As} already exists on the target object. It will be replaced with the source property value.");
				
				var value = dictionary[specification.Property];
				dictionary.Remove(specification.As);
				dictionary.Add(specification.As, value);

				return;
			}

			dictionary.Add(specification.As, dictionary[specification.Property]);
			Log.Debug($"Aliased {specification.Property} as {specification.As}. Property value is {dictionary[specification.As]??"null"}");
		}
	}
}

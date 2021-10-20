using System;
using System.Dynamic;
using Mapex.Logging;
using Mapex.Specifications;

namespace Mapex.Services
{
	internal class Creator : IPropertyTransformer
	{
		private static readonly ILog Log = LogProvider.For<Creator>();

		public void Apply(PropertyTransformSpecification specification, ExpandoObject obj)
		{
			if(specification.Create == null)
				return;

			var target = obj.ToDictionary();

			if (target.ContainsKey(specification.Property))
			{
				if (specification.Create.Overwrite)
					target.Remove(specification.Property);
				else
					throw new Exception($"Property {specification.Property} exists on the object and cannot be overwritten.");
			}

			target.Add(specification.Property, specification.Create.Value.ToLower() == "[null]" 
				? null 
				: specification.Create.Value
			);
		}
	}
}

using System.Dynamic;
using Mapex.Logging;
using Mapex.Specifications;

namespace Mapex.Services
{
	internal class Appender : IPropertyTransformer
	{
		private static readonly ILog Log = LogProvider.For<Appender>();

		public void Apply(PropertyTransformSpecification specification, ExpandoObject obj)
		{
			if(specification.Append == null)
				return;

			var target = obj.ToDictionary();
			Dynamic.ThrowIfFieldMissing(target, specification.Property);

			var currentValue = target.GetCurrentPropertyValue(specification).ToString();

			Log.Debug($"{(specification.Append.Postfix ? "Post-fixing" : "Pre-fixing")} {specification.Append.Value} to {specification.Property}");

			if(specification.Append.Postfix)
				target[specification.Property] = currentValue + specification.Append.Value;
			else
				target[specification.Property] = specification.Append.Value + currentValue;
		}
	}
}

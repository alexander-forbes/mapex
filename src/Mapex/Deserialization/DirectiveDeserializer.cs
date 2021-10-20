using System;
using System.Collections.Generic;
using System.Linq;
using Mapex.Logging;

namespace Mapex.Deserialization
{
	internal interface IDirectiveDeserializer
	{
		IEnumerable<IDirective> Deserialize(IEnumerable<SerializedDirective> directives);
	}

	internal class DirectiveDeserializer : IDirectiveDeserializer
	{
		private static readonly ILog Log = LogProvider.For<DirectiveDeserializer>();
		private readonly IDirectiveValidator _Validator;
		private readonly IYamlDeserializer _YamlDeserializer;

		public DirectiveDeserializer(IDirectiveValidator validator, IYamlDeserializer deserializer)
		{
			_Validator = validator ?? throw new ArgumentNullException(nameof(validator));
			_YamlDeserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
		}

		public IEnumerable<IDirective> Deserialize(IEnumerable<SerializedDirective> directives)
		{
			if (directives == null)
				throw new ArgumentNullException(nameof(directives));

			Log.Debug($"Deserializing {directives.Count()} directives");
			var deserialized = DeserializeDirectives(directives);

			Log.Debug($"Validating {deserialized.Count} directives");
			_Validator.Validate(deserialized);

			return deserialized;
		}

		private List<Directive> DeserializeDirectives(IEnumerable<SerializedDirective> directives)
		{
			var deserialized = new List<Directive>();

			foreach (var serializedDirective in directives)
				DeserializeDirective(serializedDirective, deserialized);

			return deserialized;
		}

		private void DeserializeDirective(SerializedDirective serializedDirective, List<Directive> deserialized)
		{
			Log.Debug($"Deserializing directive {serializedDirective.Id}");

			try
			{
				var directive = _YamlDeserializer.Deserialize<Directive>(serializedDirective.Data);

				directive.Id = serializedDirective.Id;
				directive.Enabled = serializedDirective.Enabled;
				deserialized.Add(directive);
			}
			catch (Exception ex)
			{
				Log.Error($"Failed to deserialize directive {serializedDirective.Id}. Exception = {ex}");
				throw new DirectiveException(serializedDirective.Id, ex.Message, ex);
			}
		}
	}
}

using System;
using System.Linq;
using Mapex.Logging;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Mapex.Deserialization
{
	internal interface IDirectiveDeserializerFactory
	{
		IDirectiveDeserializer Build(TagMap tagMap);
	}

	internal class DirectiveDeserializerFactory : IDirectiveDeserializerFactory
	{
		private static readonly ILog Log = LogProvider.For<DirectiveDeserializerFactory>();

		public IDirectiveDeserializer Build(TagMap tagMap)
		{
			if (tagMap == null)
				throw new ArgumentNullException(nameof(tagMap));

			Log.Debug($"Building Yaml deserializer with {tagMap.Count()} tags");
			var yamlDeserializer = BuildYamlDeserializer(tagMap);

			var validator = new DirectiveValidator();

			return new DirectiveDeserializer(validator, yamlDeserializer);
		}

		private static IYamlDeserializer BuildYamlDeserializer(TagMap tagMap)
		{
			var builder = new DeserializerBuilder();
			builder.WithNamingConvention(CamelCaseNamingConvention.Instance);

			foreach (var mapping in tagMap)
			{
				Log.Debug($"\tRegistering tag {mapping.Key}->{mapping.Value} with the deserializer");
				builder.WithTagMapping(mapping.Key, mapping.Value);
			}

			return new YamlDeserializerProxy(builder.Build());
		}
	}
}

using System;
using Mapex.Logging;
using YamlDotNet.Serialization;

namespace Mapex.Deserialization
{
	internal interface IYamlDeserializer
	{
		T Deserialize<T>(string input);
	}

	internal class YamlDeserializerProxy : IYamlDeserializer
	{
		private static readonly ILog Log = LogProvider.For<YamlDeserializerProxy>();
		private readonly IDeserializer _Deserializer;
		
		public YamlDeserializerProxy(IDeserializer deserializer)
		{
			_Deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
		}

		public T Deserialize<T>(string input)
		{
			Log.Debug($"Deserializing: {input}");
			return _Deserializer.Deserialize<T>(input);
;		}
	}
}

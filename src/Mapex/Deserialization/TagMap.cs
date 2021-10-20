using System;
using System.Collections;
using System.Collections.Generic;
using Mapex.Logging;

namespace Mapex.Deserialization
{
	public class TagMap : IEnumerable<KeyValuePair<string, Type>>
	{
		private static readonly ILog Log = LogProvider.For<TagMap>();
		private readonly IDictionary<string, Type> _TagMappings = new Dictionary<string, Type>();

		public void Register(string tag, Type type)
		{
			if (string.IsNullOrEmpty(tag))
				throw new ArgumentNullException(nameof(tag));

			if(type == null)
				throw new ArgumentNullException(nameof(type));

			if (_TagMappings.ContainsKey(tag))
			{
				Log.Debug($"Overwriting tag {tag} with {type.Name}");
				_TagMappings[tag] = type;
			}
			else
			{
				Log.Debug($"Registering tag {tag} as {type.Name}");
				_TagMappings.Add(tag, type);
			}
		}

		public IEnumerator<KeyValuePair<string, Type>> GetEnumerator()
		{
			return _TagMappings.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}

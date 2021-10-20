using System;
using Mapex.Deserialization;

namespace Mapex
{
	public class MapexEngineConfiguration
	{
		public IDirectiveLoader Loader { get; set; }
		public TagMap Tags { get; } = new TagMap();
		
		public void Verify()
		{
			if (Loader == null)
				throw new Exception("A directive loader has not been configured.");
		}
	}
}

using System.Collections.Generic;

namespace Mapex
{
	public interface IDirectiveLoader
	{
		IEnumerable<SerializedDirective> Load(Metadata metadata);
	}
}

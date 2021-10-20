using System;
using System.Collections.Generic;

namespace Mapex
{
	public class SerializedDirective
	{
		public int Id { get; set; }
		public string Data { get; set; }
		public bool Enabled { get; set; }

		public sealed class EqualityComparer : IEqualityComparer<SerializedDirective>
		{
			public bool Equals(SerializedDirective x, SerializedDirective y)
			{
				if (ReferenceEquals(x, y))
					return true;

				if (ReferenceEquals(x, null))
					return false;

				if (ReferenceEquals(y, null))
					return false;

				if (x.GetType() != y.GetType())
					return false;

				return x.Id == y.Id &&
					   string.Equals(x.Data, y.Data, StringComparison.InvariantCulture) &&
					   x.Enabled == y.Enabled;
			}

			public int GetHashCode(SerializedDirective obj)
			{
				unchecked
				{
					var hashCode = obj.Id;
					hashCode = (hashCode * 397) ^ (obj.Data != null ? StringComparer.InvariantCulture.GetHashCode(obj.Data) : 0);
					hashCode = (hashCode * 397) ^ obj.Enabled.GetHashCode();
					return hashCode;
				}
			}
		}
	}
}

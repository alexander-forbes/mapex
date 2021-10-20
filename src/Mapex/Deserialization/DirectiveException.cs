using System;
using System.Runtime.Serialization;

namespace Mapex.Deserialization
{
	public class DirectiveException : Exception
	{
		public int DirectiveId { get; }

		public DirectiveException(int directiveId)
		{
			DirectiveId = directiveId;
		}

		public DirectiveException(int directiveId, string message) : base(message)
		{
			DirectiveId = directiveId;
		}

		public DirectiveException(int directiveId, string message, Exception innerException) : base(message, innerException)
		{
			DirectiveId = directiveId;
		}

		public DirectiveException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}

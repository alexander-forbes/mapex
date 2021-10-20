using System;
using System.Collections.Generic;

namespace Mapex
{
	public enum MapexOutcome
	{
		Processed,
		Ignored,
		Failed
	}

	public class MapexResult
	{
		public string DocumentId { get; set; }
		public int DirectiveId { get; set; }
		public Type Type { get; set; }
		public IEnumerable<object> Data { get; set; }
		public MapexOutcome Outcome { get; set; }
		public string Reason { get; set; }
	}
}

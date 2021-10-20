using System;
using System.Collections.Generic;
using Mapex.Logging;

namespace Mapex
{
	internal interface IDocumentProcessor
	{
		IEnumerable<MapexResult> Process(IDocument document, IEnumerable<IDirective> directives);
	}

	internal class DocumentProcessor : IDocumentProcessor
	{
		private static readonly ILog Log = LogProvider.For<DocumentProcessor>();

		public IEnumerable<MapexResult> Process(IDocument document, IEnumerable<IDirective> directives)
		{
			if (document == null)
				throw new ArgumentNullException(nameof(document));

			if (directives == null)
				throw new ArgumentNullException(nameof(directives));
			
			var results = new List<MapexResult>();

			foreach (var directive in directives)
			{
				if (directive.CanProcess(document))
				{
					Log.Debug($"Processing document {document.Id ?? "[No Id]"} using directive {directive.Id}");

					var result = directive.Process(document);
					results.Add(result);
				}
			}

			Log.Debug($"Processed document {document.Id ?? "[No Id]"}");

			return results;
		}
	}
}

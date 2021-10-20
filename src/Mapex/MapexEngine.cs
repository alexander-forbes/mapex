using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mapex.Deserialization;
using Mapex.Logging;
using Mapex.Specifications;

namespace Mapex
{
	public interface IMapexEngine
	{
		IEnumerable<MapexResult> Process(IDocument document);
	}

	public class MapexEngine : IMapexEngine
	{
		private static readonly ILog Log = LogProvider.For<MapexEngine>();
		private readonly MapexEngineConfiguration _Configuration = new MapexEngineConfiguration();
		private readonly IDocumentProcessor _DocumentProcessor;
		private IDirectiveDeserializer _DirectiveDeserializer;

		public MapexEngine(Action<MapexEngineConfiguration> config) 
			: this(config, new DocumentProcessor(), new DirectiveDeserializerFactory())
		{
		}

		internal MapexEngine(Action<MapexEngineConfiguration> config, IDocumentProcessor documentProcessor, IDirectiveDeserializerFactory directiveDeserializerFactory)
		{
			if (config == null)
				throw new ArgumentNullException(nameof(config));

			if(directiveDeserializerFactory == null)
				throw new ArgumentNullException(nameof(directiveDeserializerFactory));

			_DocumentProcessor = documentProcessor ?? throw new ArgumentNullException(nameof(documentProcessor));
			
			RegisterDefaultTags();

			config(_Configuration);

			BuildDeserializer(directiveDeserializerFactory);
		}

		private void BuildDeserializer(IDirectiveDeserializerFactory directiveDeserializerFactory)
		{
			_DirectiveDeserializer = directiveDeserializerFactory.Build(_Configuration.Tags);
		}

		private void RegisterDefaultTags()
		{
			_Configuration.Tags.Register("!default_filter", typeof(FilterSpecification));
			_Configuration.Tags.Register("!default_mapper", typeof(MapSpecification));
			_Configuration.Tags.Register("!default_transformer", typeof(TransformSpecification));
		}

		public IEnumerable<MapexResult> Process(IDocument document)
		{
			if (document == null)
				throw new ArgumentNullException(nameof(document));

			var sw = new Stopwatch();
			
			try
			{
				sw.Start();
				var directives = LoadDirectives(document);
				sw.Stop();
				Log.Debug($"Loaded {directives.Count()} directives in {sw.ElapsedMilliseconds} (ms.)");

				sw.Restart();
				var deserialized = _DirectiveDeserializer.Deserialize(directives);
				sw.Stop();
				Log.Debug($"Deserialized {directives.Count()} directives in {sw.ElapsedMilliseconds} (ms.)");

				var enabledDirectives = deserialized.Where(dir => dir.Enabled);
				Log.Debug($"Found {enabledDirectives.Count()} enabled directives.");

				sw.Restart();
				var results = ProcessDocument(document, enabledDirectives);
				sw.Stop();
				Log.Debug($"Processed document {document.Id??"[No Id]"} in {sw.ElapsedMilliseconds} (ms.)");

				return results;
			}
			catch(Exception ex)
			{
				Log.Error(ex, $"Failed to process document {document.Id??"[No Id]"}. {ex.ToString()}");
				throw;
			}
		}

		private IEnumerable<MapexResult> ProcessDocument(IDocument document, IEnumerable<IDirective> directives)
		{
			return directives.Any() ? _DocumentProcessor.Process(document, directives) : Enumerable.Empty<MapexResult>();
		}

		private IEnumerable<SerializedDirective> LoadDirectives(IDocument document)
		{
			return _Configuration.Loader.Load(document.Metadata);
		}
	}
}

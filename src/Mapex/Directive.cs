using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Mapex.Logging;
using Mapex.Specifications;
using Notus;

namespace Mapex
{
	public interface IDirective
	{
		int Id {get; set; }
		bool Enabled { get; set; }
		bool CanProcess(IDocument document);
		MapexResult Process(IDocument document);
		Notification Validate();
	}

	internal class Directive : IDirective
	{
		private static readonly ILog Log = LogProvider.For<Directive>();

		public int Id { get; set; }

		public bool Enabled { get; set; }

		public IWhereSpecification Where { get; set; }
		public IExtractSpecification Extract { get; set; }
		public ITransformSpecification Transform { get; set; } = new NullTransformSpecification();
		public IMapSpecification Map { get; set; } = new MapSpecification();
		public IFilterSpecification Filter { get; set; } = new NullFilterSpecification();
		public IEnumerable<IInterceptSpecification> Intercept { get; set; } = new [] { new NullInterceptSpecification() };

		public bool CanProcess(IDocument document)
		{
			if (document == null)
				throw new ArgumentNullException(nameof(document));

			Log.Debug($"Checking if directive {Id} can process document id {document.Id ?? "null"} using {Where.GetType().Name}");
			
			var result = Where.Matches(document);
			
			Log.Debug($"Directive {Id} {(result ? "can" : "cannot")} process document id {document.Id ?? "null"}");

			return result;
		}

		public MapexResult Process(IDocument document)
		{
			if (document == null)
				throw new ArgumentNullException(nameof(document));

			Log.Debug($"Directive {Id} is processing document id {document.Id ?? "null"}");

			try
			{
				var result = CreateMapexResult(document);

				var objects = Extract.Process(document);
				objects = FilterObjects(objects);
				objects = TransformObjects(objects);
				var mapped = MapObjects(objects);

				result.Data = mapped;
				Log.Debug($"Directive {Id} processed document id {document.Id ?? "null"} with {result.Data.Count()} objects");

				SetResultOutcome(result);

				return result;
			}
			catch(Exception ex)
			{
				return CreateFailedResult(document, ex);
			}
		}

		private MapexResult CreateFailedResult(IDocument document, Exception ex)
		{
			var result = new MapexResult
			{
				Outcome = MapexOutcome.Failed,
				Reason = $"Failed to process document {document.Id ?? "[No Id]"} using directive {Id}. {ex.ToString()}"
			};

			Log.Error($"Failed to process document {document.Id ?? "[No Id]"} using directive {Id}. {ex.ToString()}");

			return result;
		}

		private static void SetResultOutcome(MapexResult result)
		{
			if (result.Data.Any())
			{
				result.Outcome = MapexOutcome.Processed;
				result.Reason = "Document processed";
			}
			else
			{
				result.Outcome = MapexOutcome.Ignored;
				result.Reason = "Document processed - no results";
			}
		}

		private MapexResult CreateMapexResult(IDocument document)
		{
			var result = new MapexResult
			{
				DirectiveId = Id,
				DocumentId = document.Id,
				Type = Type.GetType(Map.As, true, true)
			};

			return result;
		}

		private IEnumerable<ExpandoObject> FilterObjects(IEnumerable<ExpandoObject> objects)
		{
			Log.Debug($"Directive {Id} is filtering {objects.Count()} objects using {Filter.GetType().Name}");
			
			Log.Debug($"Calling BeforeFilter() on {Intercept.GetType().Name} with {objects.Count()} objects");
			objects = InterceptObjects((i, o) => i.BeforeFilter(o), objects);
			
			Log.Debug($"Calling Filter.Process() on {Filter.GetType().Name} with {objects.Count()} objects");
			objects = Filter.Process(objects);
			
			Log.Debug($"Calling AfterFilter() on {Intercept.GetType().Name} with {objects.Count()} objects");
			objects = InterceptObjects((i, o) => i.AfterFilter(o), objects);
			
			Log.Debug($"Filtering completed with {objects.Count()} objects remaining");
			return objects;
		}

		private IEnumerable<object> MapObjects(IEnumerable<ExpandoObject> objects)
		{
			Log.Debug($"Directive {Id} is mapping {objects.Count()} objects using {Map.GetType().Name}");
			
			Log.Debug($"Calling BeforeMap() on {Intercept.GetType().Name} with {objects.Count()} objects");
			objects = InterceptObjects((i, o) => i.BeforeMap(o), objects);
			
			Log.Debug($"Calling Map.Process() on {Map.GetType().Name} with {objects.Count()} objects");
			var mapped = Map.Process(objects);
			
			Log.Debug($"Mapping completed with {mapped.Count()} objects");
			return mapped;
		}

		private IEnumerable<ExpandoObject> TransformObjects(IEnumerable<ExpandoObject> objects)
		{
			Log.Debug($"Directive {Id} is transforming {objects.Count()} objects using {Transform.GetType().Name}");
			
			Log.Debug($"Calling BeforeTransform on {Intercept.GetType().Name} with {objects.Count()} objects");
			objects = InterceptObjects((i, o) => i.BeforeTransform(o), objects);
			
			Log.Debug($"Calling Transform.Process on {Intercept.GetType().Name} with {objects.Count()} objects");
			Transform.Process(objects);
			
			Log.Debug($"Transform completed with {objects.Count()} objects");
			return objects;
		}

		private IEnumerable<ExpandoObject> InterceptObjects(Func<IInterceptSpecification, IEnumerable<ExpandoObject>, IEnumerable<ExpandoObject>> interceptMethod, IEnumerable<ExpandoObject> objects)
		{
			var o = objects;

			foreach (var interceptor in Intercept)
			{
				Log.Debug($"Intercepting objects using interceptor {interceptor.GetType().Name}...");
				o = InterceptObjects(interceptor, interceptMethod, o);
			}

			return o;
		}

		private static IEnumerable<ExpandoObject> InterceptObjects(IInterceptSpecification interceptor, Func<IInterceptSpecification, IEnumerable<ExpandoObject>, IEnumerable<ExpandoObject>> interceptMethod, IEnumerable<ExpandoObject> objects)
		{
			return interceptMethod(interceptor, objects);
		}

		public Notification Validate()
		{
			var notification = new Notification();

			Validate(notification, Where, nameof(Where));
			Validate(notification, Extract, nameof(Extract));
			Validate(notification, Filter, nameof(Filter));
			Validate(notification, Transform, nameof(Transform));
			Validate(notification, Map, nameof(Map));

			if(Intercept == null)
			{
				Log.Error($"Specification Intercept is not set for directive {Id}.");
				notification.AddError($"The Intercept property of the directive is not set.");
			}
			else
			{
				foreach (var interceptor in Intercept)
					Validate(notification, interceptor, nameof(Intercept));
			}

			return notification;
		}

		private void Validate(Notification notification, ISpecification specification, string propertyName)
		{
			if (specification == null)
			{
				Log.Error($"Specification {propertyName} is not set for directive {Id}.");
				notification.AddError($"The {propertyName} property of the directive is not set.");
			}
			else
			{
				Log.Debug($"Validating specification {specification?.GetType().Name ?? "null"}");
				specification.Validate(notification);
			}
		}
	}
}

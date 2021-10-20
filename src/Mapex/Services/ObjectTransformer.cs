using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Mapex.Logging;
using Mapex.Specifications;

namespace Mapex.Services
{
	internal interface IObjectTransformer
	{
		void Transform(IEnumerable<ExpandoObject> objects, TransformSpecification specification);
	}

	internal class ObjectTransformer : IObjectTransformer
	{
		private static readonly ILog Log = LogProvider.For<ObjectTransformer>();
		private readonly IEnumerable<IPropertyTransformer> _FieldTransformers;

		public ObjectTransformer()
			: this(new IPropertyTransformer[] { 
				new Creator(),
				new Appender(),
				new Yanker(),
				new Trimmer(),
				new Translator(),
				new TypeConverter(),
				new Resolver(),
				new Aliaser()
			})
		{
		}

		internal ObjectTransformer(IEnumerable<IPropertyTransformer> propertyTransformers)
		{
			_FieldTransformers = propertyTransformers ?? throw new ArgumentNullException(nameof(propertyTransformers));
		}

		public void Transform(IEnumerable<ExpandoObject> objects, TransformSpecification specification)
		{
			Log.Debug($"Transforming {objects.Count()} objects");

			foreach(var obj in objects)
			{
				foreach (var propertyTransformSpecification in specification.Properties)
				{
					foreach (var transformer in _FieldTransformers)
					{
						transformer.Apply(propertyTransformSpecification, obj);
					}
				}
			}

			Log.Debug($"Transformed {objects.Count()} objects");
		}
	}
}

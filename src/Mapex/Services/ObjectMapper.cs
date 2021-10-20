using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using AutoMapper;
using Mapex.Logging;
using Mapex.Specifications;

namespace Mapex.Services
{
	internal interface IObjectMapper
	{
		IEnumerable<object> Map(IEnumerable<ExpandoObject> objects, IMapSpecification specification);
	}

	internal class ObjectMapper : IObjectMapper
	{
		private static readonly ILog Log = LogProvider.For<ObjectMapper>();
		private static readonly IMapper _Mapper;

		static ObjectMapper()
		{
			var configuration = new MapperConfiguration(cfg =>
			{
			});

			_Mapper = configuration.CreateMapper();
		}

		public IEnumerable<object> Map(IEnumerable<ExpandoObject> objects, IMapSpecification specification)
		{
			if (objects == null)
				throw new ArgumentNullException(nameof(objects));

			if (specification == null)
				throw new ArgumentNullException(nameof(specification));

			var collectionType = CreateCollectionType(specification);

			Log.Debug($"Mapping {objects.Count()} objects to {specification.As}");

			var result = _Mapper.Map(objects, typeof(IEnumerable<ExpandoObject>), collectionType) as IEnumerable<object>;
			
			Log.Debug($"Mapped {result.Count()} objects to {specification.As}");

			return result;
		}

		private static Type CreateCollectionType(IMapSpecification specification)
		{
			Log.Debug($"Resolving type {specification.As} to create generic collection");
			var type = Type.GetType(specification.As, true, true);
			var collectionType = typeof(IEnumerable<>).MakeGenericType(type);
			return collectionType;
		}
	}
}

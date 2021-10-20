using System;
using System.Collections.Generic;
using System.Dynamic;
using Mapex.Services;
using Notus;

namespace Mapex.Specifications
{
	public interface IMapSpecification : ISpecification
	{
		string As { get; set; }
		IEnumerable<object> Process(IEnumerable<ExpandoObject> objects);
	}

	public class MapSpecification : IMapSpecification
	{
		private readonly IObjectMapper _ObjectMapper;

		public MapSpecification()
			: this(new ObjectMapper())
		{
		}

		internal MapSpecification(IObjectMapper objectMapper)
		{
			_ObjectMapper = objectMapper ?? throw new ArgumentNullException(nameof(objectMapper));
		}

		public string As { get; set; }

		public IEnumerable<object> Process(IEnumerable<ExpandoObject> objects)
		{
			if (objects == null)
				throw new ArgumentNullException(nameof(objects));

			return _ObjectMapper.Map(objects, this);
		}

		public void Validate(Notification notification)
		{
			if (notification == null)
				throw new ArgumentNullException(nameof(notification));

			if (string.IsNullOrEmpty(As))
				notification.AddError("The As property of the map specification is not set.");
		}
	}
}

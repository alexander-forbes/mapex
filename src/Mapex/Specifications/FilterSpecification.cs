using System;
using System.Collections.Generic;
using System.Dynamic;
using Mapex.Services;
using Notus;

namespace Mapex.Specifications
{
	public interface IFilterSpecification : ISpecification
	{
		string Where { get; set; }
		IEnumerable<ExpandoObject> Process(IEnumerable<ExpandoObject> objects);
	}

	public class FilterSpecification : IFilterSpecification
	{
		private readonly IObjectFilter _ObjectFilter;

		public FilterSpecification()
			: this(new ObjectFilter())
		{
		}

		internal FilterSpecification(IObjectFilter objectFilter)
		{
			_ObjectFilter = objectFilter ?? throw new ArgumentNullException(nameof(objectFilter));
		}

		public string Where { get; set; }

		public IEnumerable<ExpandoObject> Process(IEnumerable<ExpandoObject> objects)
		{
			if (objects == null)
				throw new ArgumentNullException(nameof(objects));

			return _ObjectFilter.Filter(objects, this);
		}

		public void Validate(Notification notification)
		{
			if (notification == null)
				throw new ArgumentNullException(nameof(notification));

			if (string.IsNullOrEmpty(Where))
				notification.AddError("The Where property of the filter specification is not set.");
		}
	}
}

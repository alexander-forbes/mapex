using System;
using System.Collections.Generic;
using System.Dynamic;
using Mapex.Services;
using Notus;

namespace Mapex.Specifications
{
	public interface ITransformSpecification : ISpecification
	{
		IEnumerable<PropertyTransformSpecification> Properties {get;set;}
		
		void Process(IEnumerable<ExpandoObject> objects);
	}

	public class TransformSpecification : ITransformSpecification
	{
		private readonly IObjectTransformer _ObjectTransformer;

		public TransformSpecification()
			: this(new ObjectTransformer())
		{
		}

		internal  TransformSpecification(IObjectTransformer objectTransformer)
		{
			_ObjectTransformer = objectTransformer ?? throw new ArgumentNullException(nameof(objectTransformer));
		}

		public IEnumerable<PropertyTransformSpecification> Properties {get;set;}

		public void Process(IEnumerable<ExpandoObject> objects)
		{
			if (objects == null)
				throw new ArgumentNullException(nameof(objects));

			_ObjectTransformer.Transform(objects, this);
		}

		public void Validate(Notification notification)
		{
			if (notification == null)
				throw new ArgumentNullException(nameof(notification));

			if (Properties == null)
			{
				notification.AddError("The Properties property of the transform specification is not set.");
				return;
			}

			foreach(var specification in Properties)
				specification.Validate(notification);
		}
	}
}

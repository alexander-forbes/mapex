using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Mapex.Logging;
using Mapex.Specifications;

namespace Mapex.Services
{
	internal interface IObjectFilter
	{
		IEnumerable<ExpandoObject> Filter(IEnumerable<ExpandoObject> objects, IFilterSpecification specification);
	}

	internal class ObjectFilter : IObjectFilter
	{
		private static readonly ILog Log = LogProvider.For<ObjectFilter>();

		public IEnumerable<ExpandoObject> Filter(IEnumerable<ExpandoObject> objects, IFilterSpecification specification)
		{
			if (objects == null)
				throw new ArgumentNullException(nameof(objects));

			if (specification == null)
				throw new ArgumentNullException(nameof(specification));

			if (objects.IsEmpty())
			{
				Log.Warn("There are no objects in the list to filter.");
				return objects;
			}

			var lambda = BuildLambda(specification.Where, typeof(IDictionary<string, object>));
			var result = FilterObjects(objects, lambda);
			
			Log.Debug($"Expression {specification.Where} matched {result.Count()} objects");

			return result;
		}

		private static IEnumerable<ExpandoObject> FilterObjects(IEnumerable<ExpandoObject> objects, LambdaExpression lambda)
		{
			var result = new List<ExpandoObject>();

			foreach (var obj in objects)
			{
				var matches = (bool) lambda.Compile().DynamicInvoke(obj);
				if (matches)
					result.Add(obj);
			}

			return result;
		}

		private static LambdaExpression BuildLambda(string expression, Type type)
		{
			Log.Debug($"Building lambda for {expression} for {type.Name}");

			var parameter = Expression.Parameter(type, "Item");
			return System.Linq.Dynamic.Core.DynamicExpressionParser.ParseLambda(new[] {parameter}, null, expression);
		}
	}
}

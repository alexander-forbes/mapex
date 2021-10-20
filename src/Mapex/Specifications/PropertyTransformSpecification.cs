using System;
using System.Collections.Generic;
using System.Linq;
using Notus;

namespace Mapex.Specifications
{
	public class PropertyTransformSpecification : ISpecification
	{
		public string Property { get; set; }
		public CreateSpecification Create { get; set; }
		public string Yank { get; set; }
		public string Trim { get; set; }
		public IEnumerable<TranslateSpecification> Translate { get; set; } = Enumerable.Empty<TranslateSpecification>();
		public ConvertSpecification Convert { get; set; }
		public AppendSpecification Append { get; set; }
		public IResolveSpecification Resolve { get; set; } = new NullResolveSpecification();
		public string As { get; set; }

		public virtual void Validate(Notification notification)
		{
			if (notification == null)
				throw new ArgumentNullException(nameof(notification));

			if (string.IsNullOrEmpty(Property))
				notification.AddError("The Property property of the property transform specification is not set.");

			if (Translate == null)
				notification.AddError("The Translate property of the property transform specification is not set.");

			if (Translate != null)
			{
				foreach (var specification in Translate)
					specification.Validate(notification);
			}

			Convert?.Validate(notification);

			Append?.Validate(notification);

			Create?.Validate(notification);
		}
	}
}

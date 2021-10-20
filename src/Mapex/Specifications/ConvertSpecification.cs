using System;
using Notus;

namespace Mapex.Specifications
{
	public class ConvertSpecification : ISpecification
	{
		public string To { get; set; }
		public string Format { get; set; }

		public virtual void Validate(Notification notification)
		{
			if (notification == null)
				throw new ArgumentNullException(nameof(notification));

			if (string.IsNullOrEmpty(To))
				notification.AddError("The To property of the convert specification is not set.");
		}
	}
}

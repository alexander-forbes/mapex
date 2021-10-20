using System;
using Notus;

namespace Mapex.Specifications
{
	public class TranslateSpecification : ISpecification
	{
		public string When { get; set; }
		public string Then { get; set; }

		public virtual void Validate(Notification notification)
		{
			if (notification == null)
				throw new ArgumentNullException(nameof(notification));

			if (string.IsNullOrEmpty(When))
				notification.AddError("The When property of the translate specification is not set.");

			if (string.IsNullOrEmpty(Then))
				notification.AddError("The Then property of the translate specification is not set.");
		}
	}
}

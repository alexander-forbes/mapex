using Notus;

namespace Mapex.Specifications
{
	public class CreateSpecification : ISpecification
	{
		public string Value { get; set; }
		public bool Overwrite { get; set; }

		public virtual void Validate(Notification notification)
		{
			if (string.IsNullOrEmpty(Value))
				notification.AddError("The Value property of the create specification has not been specified.");
		}
	}
}

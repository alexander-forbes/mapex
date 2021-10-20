using Notus;

namespace Mapex.Specifications
{
	public class AppendSpecification : ISpecification
	{
		public string Value { get; set; }
		public bool Postfix { get; set; }

		public virtual void Validate(Notification notification)
		{
			if (string.IsNullOrEmpty(Value))
				notification.AddError("The Value property of the append specification has not been specified.");
		}
	}
}

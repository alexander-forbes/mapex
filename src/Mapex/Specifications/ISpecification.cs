using Notus;

namespace Mapex.Specifications
{
	public interface ISpecification
	{
		void Validate(Notification notification);
	}
}

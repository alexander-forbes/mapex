using Mapex.Specifications;

namespace Mapex.Services.TypeConverters
{
	internal interface ITypeConverter
	{
		object Convert(string value, PropertyTransformSpecification specification);
	}
}

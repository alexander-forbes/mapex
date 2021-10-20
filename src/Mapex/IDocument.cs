namespace Mapex
{
	public interface IDocument
	{
		string Id {get;set;}
		Metadata Metadata { get; set; }
		byte[] Data { get; set; }
	}
}

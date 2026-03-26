namespace PayGram.Public
{
	public interface IPaygramResponse
	{
		PaygramResponseTypes Type { get; set; }
		bool Success { get; }
		string Message { get; set; }
	}
}

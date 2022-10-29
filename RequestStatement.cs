using PayGram.Types;

namespace PayGram.Public
{
	public class RequestStatement : PaygramRequest
	{
		public DateTime FromUtc { get; set; }
		public DateTime ToUtc { get; set; }
		public int PageSize { get; set; }
		public int ResultsPerPage { get; set; }
	}
}

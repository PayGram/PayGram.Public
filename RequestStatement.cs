using PayGram.Types;

namespace PayGram.Public
{
	public class RequestStatement : PaygramRequest
	{
		public DateTime FromUtc { get; set; }
		public DateTime ToExcludingUtc { get; set; }
		public int StartIdx { get; set; }
		public int ResultsPerPage { get; set; }
	}
}

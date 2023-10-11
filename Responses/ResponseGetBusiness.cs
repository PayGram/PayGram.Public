using PayGram.Public.UserAPI;

namespace PayGram.Public
{
	public class ResponseGetBusiness : PaygramResponse
	{
		public ResponseGetBusiness() : base(PaygramResponseTypes.ResponseGetBusiness)
		{

		}

		public List<Business> Businesses { get; set; } = new();
	}
}

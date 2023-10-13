using PayGram.Public.UserAPI;

namespace PayGram.Public
{
	public class ResponseGetBusiness : PaygramResponse
	{
		public ResponseGetBusiness() : base(PaygramResponseTypes.ResponseGetBusiness)
		{

		}
		public ResponseGetBusiness(ResponseCodes code) : base(PaygramResponseTypes.ResponseGetBusiness, code)
		{

		}
		public List<Business> Businesses { get; set; } = new();
	}
}

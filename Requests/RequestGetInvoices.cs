namespace PayGram.Public
{
	public class RequestGetInvoices : PaygramRequest
	{
		public string CallbackData { get; set; }
		public RequestGetInvoices()
		{

		}
	}
}

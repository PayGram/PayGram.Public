namespace PayGram.Public
{
	public class ResponseGetInvoices : PaygramResponse
	{
		public Guid[] Invoices { get; set; }
		public ResponseGetInvoices() : base(PaygramResponseTypes.ResponseGetInvoices)
		{
			Invoices = new Guid[0];
		}
		public ResponseGetInvoices(string message, ResponseCodes code) : base(message, PaygramResponseTypes.ResponseGetInvoices, code)
		{
			Invoices = new Guid[0];
		}
	}
}

namespace PayGram.Public
{
	public class ResponseGetInvoices : PaygramResponse
	{
		public Invoice[] Invoices { get; set; }
		public ResponseGetInvoices() : base(PaygramResponseTypes.ResponseGetInvoices)
		{
			Invoices = new Invoice[0];
		}
		public ResponseGetInvoices(string message, ResponseCodes code) : base(message, PaygramResponseTypes.ResponseGetInvoices, code)
		{
			Invoices = new Invoice[0];
		}
	}
}

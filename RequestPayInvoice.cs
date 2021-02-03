using System;

namespace PayGram.Types
{
	public class RequestPayInvoice : PaygramRequest
	{
		public Guid InvoiceCode { get; set; }
	}
}
using System;

namespace PayGram.Types
{
	public class RequestRedeemInvoice : PaygramRequest
	{
		public Guid InvoiceCode { get; set; }
	}
}
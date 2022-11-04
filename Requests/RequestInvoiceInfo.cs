namespace PayGram.Public.Requests
{
    public class RequestInvoiceInfo : PaygramRequest
    {
        public Guid InvoiceCode { get; set; }
    }
}

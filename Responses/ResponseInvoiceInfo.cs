using CurrenciesLib;

namespace PayGram.Public.Responses
{
	public class ResponseInvoiceInfo : PaygramResponse
	{
		public Guid InvoiceCode { get; set; }
		public InvoiceStatuses Status { get; set; }
		public DateTime LastEventUtc { get; set; }
		public DateTime CreatedUtc { get; set; }
		/// <summary>
		/// The amount inclusive of the fees
		/// </summary>
		public decimal Amount { get; set; }
		/// <summary>
		/// The fees paid or expected to be paid for this invoice
		/// </summary>
		public decimal ExpectedFee { get; set; }
		/// <summary>
		/// The data that will be sent back when the invoice is completed
		/// </summary>
		public string? CallbackData { get; set; }
		/// <summary>
		/// The currency code of the invoice 
		/// It is the string representing one of the <see cref="Currencies"/>.
		/// </summary>
		public Currencies CurrencyCode { get; set; }
		/// <summary>
		/// The sender UserId at the client side. This info is not populated if the request is not made through 
		/// the client owning this user, or the user making the request corresponds to FromUser
		/// </summary>
		public string? FromUser { get; set; }
		/// <summary>
		/// The receiver UserId at the client side. This info is not populated if the request is not made through 
		/// the client owning this user, or the user making the request corresponds to ToUser
		/// </summary>
		public string? ToUser { get; set; }
		/// <summary>
		/// The UserId at the client side that originated the invoice. This info is not populated if the request is not made through 
		/// the client owning this user, or the user making the request corresponds to ToUser
		/// </summary>
		public string? OriginatingUser { get; set; }
		/// <summary>
		/// Gets or sets whether the invoice was paid
		/// </summary>
		public bool IsPaid { get; set; }
		/// <summary>
		/// Gets or sets whether the invoice was redeemed
		/// </summary>
		public bool IsRedeemed { get; set; }
		public InvoiceBusinessTypes InvoiceType { get; set; }
		public ResponseInvoiceInfo()
				: base(PaygramResponseTypes.ResponseInvoiceInfo)
		{

		}
	}
}

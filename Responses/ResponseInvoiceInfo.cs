using CurrenciesLib;

namespace PayGram.Public.Responses
{
	/// <summary>
	/// Returned by <c>InvoiceInfo</c> / <c>InvoiceInfoV2</c> and <c>PayVoucher</c> / <c>PayVoucherV2</c>.
	/// Contains detailed information about an invoice including its status, amounts, and the parties involved.
	/// May be a <see cref="ResponseRedEnvelopeInvoiceInfo"/> if the invoice is a red envelope.
	/// </summary>
	public class ResponseInvoiceInfo : PaygramResponse
	{
		/// <summary>
		/// The unique GUID identifying this invoice. Used to look up or reference the invoice across the system.
		/// </summary>
		public Guid InvoiceCode { get; set; }
		/// <summary>
		/// A human-readable code for the invoice/voucher (e.g. "ABC-1234"), easier to share than the GUID.
		/// </summary>
		public string? FriendlyVoucherCode { get; set; }
		/// <summary>
		/// The current status of the invoice (e.g. Pending, Completed, Expired).
		/// </summary>
		public InvoiceStatuses Status { get; set; }
		/// <summary>
		/// The UTC timestamp of the most recent event on this invoice (payment, redemption, etc.).
		/// </summary>
		public DateTime LastEventUtc { get; set; }
		/// <summary>
		/// The UTC timestamp when this invoice was originally created.
		/// </summary>
		public DateTime CreatedUtc { get; set; }
		/// <summary>
		/// The total invoice amount inclusive of fees, in <see cref="CurrencyCode"/>.
		/// </summary>
		public decimal Amount { get; set; }
		/// <summary>
		/// The fees paid or expected to be paid for this invoice, in <see cref="CurrencyCode"/>.
		/// </summary>
		public decimal ExpectedFee { get; set; }
		/// <summary>
		/// Client-provided callback data associated with this invoice. Returned in notifications when the invoice is paid/redeemed.
		/// Only populated if the requesting user/client is the invoice originator.
		/// </summary>
		public string? CallbackData { get; set; }
		/// <summary>
		/// The currency of the invoice.
		/// </summary>
		public Currencies CurrencyCode { get; set; }
		/// <summary>
		/// The client-side user ID of the payer (who sent the funds).
		/// Only populated if the requester is the payer, an admin, or the payer belongs to the requesting client.
		/// For business invoices, always populated.
		/// </summary>
		public string? FromUser { get; set; }
		/// <summary>
		/// The client-side user ID of the receiver (who received the funds).
		/// Only populated if the requester is the receiver, an admin, or the receiver belongs to the requesting client.
		/// </summary>
		public string? ToUser { get; set; }
		/// <summary>
		/// The client-side user ID that originally created the invoice.
		/// Only populated if the requester is the originator, an admin, or the originator belongs to the requesting client.
		/// </summary>
		public string? OriginatingUser { get; set; }
		/// <summary>
		/// True if the invoice has been paid (funds debited from the payer).
		/// Determined by whether a MoneySentInternalTran exists for this invoice.
		/// </summary>
		public bool IsPaid { get; set; }
		/// <summary>
		/// True if the invoice has been redeemed (funds credited to the receiver).
		/// Determined by whether a MoneyReceivedInternalTran exists for this invoice.
		/// </summary>
		public bool IsRedeemed { get; set; }
		/// <summary>
		/// Whether this invoice was created in a business context (Business) or user-to-user context (User).
		/// </summary>
		public InvoiceBusinessTypes InvoiceBusinessType { get; set; }
		/// <summary>
		/// The type of invoice (InternalInvoice, WithdrawInvoice, TopUpInvoice, Swap, Voucher, PaymentRequest, RedEnvInvoice, etc.).
		/// </summary>
        public InvoiceTypes InvoiceType { get; set; }
        public ResponseInvoiceInfo()
				: base(PaygramResponseTypes.ResponseInvoiceInfo)
		{

		}
		public ResponseInvoiceInfo(PaygramResponseTypes type, ResponseCodes code) : base(type, code)
		{

		}
		public ResponseInvoiceInfo(ResponseCodes code) : base(PaygramResponseTypes.ResponseInvoiceInfo, code)
		{

		}
		public ResponseInvoiceInfo(string msg, ResponseCodes code) : base(msg, PaygramResponseTypes.ResponseInvoiceInfo, code)
		{

		}
	}
}

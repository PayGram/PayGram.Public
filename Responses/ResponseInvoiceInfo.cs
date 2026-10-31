using CurrenciesLib;
using Newtonsoft.Json;

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
		/// Written <c>null</c> in JSON when <see cref="CodesWithheld"/> (CheckUnique to a support account, someone else's open voucher);
		/// <see cref="Guid.Empty"/> when a <c>null</c> is read.
		/// </summary>
		[JsonIgnore]
		public Guid InvoiceCode { get; set; }
		/// <summary>
		/// <see cref="InvoiceCode"/> as it is written and read in JSON, under its name: the same guid, or <c>null</c> when the codes are
		/// withheld. Not public: the property everyone reads stays a <see cref="Guid"/> (a nullable one would not compile in the callers)
		/// </summary>
		[JsonProperty(nameof(InvoiceCode))]
		// the default of the Guid it stands for: a format that leaves default values out (PublicJson, the stored json) leaves out
		// an empty code as before, never a withheld one
		[System.ComponentModel.DefaultValue(typeof(Guid), "00000000-0000-0000-0000-000000000000")]
		protected Guid? InvoiceCodeInJson
		{
			get => CodesWithheld ? null : InvoiceCode;
			set { InvoiceCode = value ?? Guid.Empty; CodesWithheld = value == null; }
		}
		/// <summary>
		/// True when the codes that redeem this invoice are withheld from the caller (CheckUnique answering an account with the
		/// CanSeeTransactionInfo permission about someone else's open voucher or red envelope, TelegramBotServer#66):
		/// <see cref="InvoiceCode"/> is then written <c>null</c> (and is <see cref="Guid.Empty"/> in memory) and
		/// <see cref="FriendlyVoucherCode"/> is null. Not written in JSON: a <c>null</c> invoiceCode read back sets it.
		/// </summary>
		[JsonIgnore]
		public bool CodesWithheld { get; set; }
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
		/// Until when a payment request can be paid (30 minutes from its creation): after that it is <see cref="InvoiceStatuses.Expired"/>
		/// and a payment is refused with <see cref="ResponseCodes.InvoiceExpired"/>. Null for the invoices that do not expire
		/// (vouchers, red envelopes, deposits, withdrawals).
		/// </summary>
		public DateTime? ExpiresUtc { get; set; }
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
		/// The client-side user ID of the payer (who sent the funds), with its client in <see cref="FromClientId"/>.
		/// The parties of an invoice are given to whoever is a party of it (the payer, the receiver, the originator, their clients)
		/// and to admins; a caller that is not involved gets none of them.
		/// </summary>
		public string? FromUser { get; set; }
		/// <summary>
		/// The client-side user ID of the receiver (who received, or will receive, the funds), with its client in <see cref="ToClientId"/>.
		/// Same visibility as <see cref="FromUser"/>.
		/// </summary>
		public string? ToUser { get; set; }
		/// <summary>
		/// The client-side user ID that originally created the invoice, with its client in <see cref="OriginatingClientId"/>.
		/// Same visibility as <see cref="FromUser"/>.
		/// </summary>
		public string? OriginatingUser { get; set; }
		/// <summary>
		/// The PayGram client of <see cref="FromUser"/>: compare it with your own client id (<see cref="ResponseUserInfo.ClientId"/>) to
		/// know whether the id belongs to your user base. Null when <see cref="FromUser"/> is null.
		/// </summary>
		public int? FromClientId { get; set; }
		/// <summary>The PayGram client of <see cref="ToUser"/>, see <see cref="FromClientId"/>.</summary>
		public int? ToClientId { get; set; }
		/// <summary>The PayGram client of <see cref="OriginatingUser"/>, see <see cref="FromClientId"/>.</summary>
		public int? OriginatingClientId { get; set; }
		/// <summary>
		/// True if the invoice has been paid (funds debited from the payer).
		/// </summary>
		public bool IsPaid { get; set; }
		/// <summary>
		/// True if the invoice has been redeemed (funds credited to the receiver).
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
		/// <summary>
		/// The swap that funded this payment, in the answer to the payer of a PayInvoice with PayWithCurrency. Null (absent) for
		/// any other answer
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)] // absent when null: the wire format of the other answers is unchanged
		public PaySwapInfo? Swap { get; set; }
		public ResponseInvoiceInfo()
				: base(PaygramResponseTypes.ResponseInvoiceInfo)
		{

		}
		public ResponseInvoiceInfo(PaygramResponseTypes type) : base(type)
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

namespace PayGram.Public.Responses
{
	/// <summary>
	/// Returned by <c>TransferCredit</c> / <c>TransferCreditV2</c>.
	/// Contains the result of transferring credit from one user to another, including the generated voucher details.
	/// </summary>
	public class ResponseTransferCredit : PaygramResponse
	{
		public ResponseTransferCredit()
						: base(PaygramResponseTypes.ResponseTransferCredit)
		{
		}

		public ResponseTransferCredit(string err)
						: base(err, PaygramResponseTypes.ResponseTransferCredit)
		{
		}
		public ResponseTransferCredit(string err, ResponseCodes responseCode)
						: base(err, PaygramResponseTypes.ResponseTransferCredit, responseCode)
		{
		}
		public ResponseTransferCredit(ResponseCodes responseCode) : base(PaygramResponseTypes.ResponseTransferCredit, responseCode)
		{
		}
		/// <summary>
		/// The sender's balance in the transfer currency after the transfer has been processed.
		/// </summary>
		public decimal FromCredit { get; set; }
		/// <summary>
		/// The receiver's balance in the transfer currency after the transfer has been processed.
		/// Note: this is set to 0 before returning to external API callers for privacy.
		/// </summary>
		public decimal ToCredit { get; set; }
		/// <summary>
		/// The GUID of the invoice created for this transfer. Can be used to look up the transfer via <c>InvoiceInfo</c>.
		/// </summary>
		public Guid VoucherCode { get; set; }
		/// <summary>
		/// A human-readable code for the voucher (e.g. "ABC-1234"), easier to share than the GUID.
		/// </summary>
		public string FriendlyVoucherCode { get; set; }
		/// <summary>
		/// When the transfer happened directly between two users, this is null.
		/// If the user has created a voucher (no recipient specified), this is the URL the recipient can visit to redeem it.
		/// For Telegram clients, this is a t.me deep link.
		/// </summary>
		public string? RedeemUrl { get; set; }
		/// <summary>
		/// The principal transfer amount (before fees), in the transfer currency.
		/// </summary>
		public decimal Amount { get; set; }
		/// <summary>
		/// The fees charged for this transfer, in the transfer currency.
		/// </summary>
		public decimal Fees { get; set; }
	}
}

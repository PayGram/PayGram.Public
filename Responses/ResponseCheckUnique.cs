namespace PayGram.Public.Responses
{
	/// <summary>
	/// One of the two parties of an invoice, resolved to the telegram account known to the bot.
	/// </summary>
	public class CheckedParty
	{
		public long TelegramId { get; set; }
		public string? Username { get; set; }
		public string? FirstName { get; set; }
	}

	/// <summary>
	/// An invoice found by <c>CheckUnique</c>: the full invoice info as seen by the client (not filtered on the requesting user)
	/// plus the sender and the receiver resolved to telegram accounts when the bot knows them.
	/// </summary>
	public class CheckedInvoice
	{
		/// <summary>The invoice. A <see cref="ResponseRedEnvelopeInvoiceInfo"/> when the invoice is a red envelope: it then carries the redeemers.</summary>
		public ResponseInvoiceInfo Info { get; set; } = new();
		/// <summary>Who paid the invoice. Null when nobody did yet or the payer is not a bot user.</summary>
		public CheckedParty? From { get; set; }
		/// <summary>Who redeemed the invoice. Null when nobody did yet or the redeemer is not a bot user. For a red envelope see the redeemers in <see cref="Info"/>.</summary>
		public CheckedParty? To { get; set; }
	}

	/// <summary>
	/// The state of the operation you created with the <c>unique</c> key that was queried (TransferCredit, Swap, Withdraw...),
	/// found by <c>CheckUnique</c>. Tells you whether a call whose answer you lost (a timeout, a 551) ran, and what it produced.
	/// </summary>
	public class CheckedUniqueRequest
	{
		/// <summary>
		/// NotFound: no operation with that key. Succeeded: done, see InvoiceCode. Failed: it moved no money, see Message; resending
		/// it with the same key executes it again. Processing: not final yet, either still running or it produced something (see
		/// InvoiceCode) whose answer was not recorded: resend the very same request with the same key to get its final answer,
		/// never a new key, which would execute it a second time.
		/// </summary>
		public string Status { get; set; } = "NotFound";
		public DateTime? CreatedUtc { get; set; }
		public DateTime? LastUpdateUtc { get; set; }
		/// <summary>The voucher or invoice the operation produced, also listed in Invoices: when it succeeded, or when it is Processing with its money moved</summary>
		public Guid? InvoiceCode { get; set; }
		/// <summary>What to know about the state: the message the operation answered with when it failed, what is pending when it is Processing</summary>
		public string? Message { get; set; }
	}

	/// <summary>
	/// Returned by <c>CheckUnique</c>: the invoices matching the query, and the state of the operation created with that
	/// unique key, when the query is one. An integrator sees the invoices it is a party of (by callback data only the ones it
	/// created, the callback data being its own) and its own operations; an account with the CanSeeTransactionInfo permission
	/// (support) sees every one of the client. Success with an empty list means nothing matched.
	/// </summary>
	public class ResponseCheckUnique : PaygramResponse
	{
		/// <summary>The query as it was looked up (trimmed; an invoice id or a voucher code also with its spaces removed)</summary>
		public string Query { get; set; } = string.Empty;
		public List<CheckedInvoice> Invoices { get; set; } = new();
		/// <summary>True when more invoices carry that callback data than the ones listed (the newest): narrow the reference</summary>
		public bool MoreInvoices { get; set; }
		/// <summary>The operation created with the query as its unique key, when the query did not match an invoice id. Status NotFound when there is none.</summary>
		public CheckedUniqueRequest? UniqueRequest { get; set; }

		public ResponseCheckUnique() : base(PaygramResponseTypes.ResponseCheckUnique)
		{
		}
		public ResponseCheckUnique(string message, ResponseCodes code) : base(message, PaygramResponseTypes.ResponseCheckUnique, code)
		{
		}
	}
}

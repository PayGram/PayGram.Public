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
	/// Returned by <c>CheckUnique</c>: the invoices matching the query. Requires the CanSeeTransactionInfo permission.
	/// Success with an empty list means nothing matched.
	/// </summary>
	public class ResponseCheckUnique : PaygramResponse
	{
		/// <summary>The query as it was interpreted (spaces removed)</summary>
		public string Query { get; set; } = string.Empty;
		public List<CheckedInvoice> Invoices { get; set; } = new();

		public ResponseCheckUnique() : base(PaygramResponseTypes.ResponseCheckUnique)
		{
		}
		public ResponseCheckUnique(string message, ResponseCodes code) : base(message, PaygramResponseTypes.ResponseCheckUnique, code)
		{
		}
	}
}

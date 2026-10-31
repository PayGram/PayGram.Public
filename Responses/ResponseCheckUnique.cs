using Newtonsoft.Json;

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
	/// An invoice found by <c>CheckUnique</c>: the invoice info as the requesting user may see it
	/// plus the sender and the receiver resolved to telegram accounts when the bot knows them.
	/// </summary>
	public class CheckedInvoice
	{
		/// <summary>The invoice. A <see cref="ResponseRedEnvelopeInvoiceInfo"/> when the invoice is a red envelope: it then carries the redeemers;
		/// a <see cref="ResponseInvoiceWithdrawInfo"/> when it is a withdrawal (read by its Type, see <see cref="InvoiceInfoConverter"/>).</summary>
		[JsonConverter(typeof(InvoiceInfoConverter))]
		public ResponseInvoiceInfo Info { get; set; } = new();
		/// <summary>Who paid the invoice. Null when nobody did yet or the payer is not a bot user.</summary>
		public CheckedParty? From { get; set; }
		/// <summary>Who redeemed the invoice. Null when nobody did yet or the redeemer is not a bot user. For a red envelope see the redeemers in <see cref="Info"/>.</summary>
		public CheckedParty? To { get; set; }
	}

	/// <summary>
	/// The state of the operation you created with the <c>unique</c> key that was queried (TransferCredit, Swap, Withdraw...),
	/// found by <c>CheckUnique</c>: whether it ran, and what it produced. The lookup executes nothing and is not a step of a retry:
	/// after a 500, a 551 or no answer, resend the same request with the same key, never a new key.
	/// </summary>
	public class CheckedUniqueRequest
	{
		/// <summary>
		/// NotFound: no operation with that key. Succeeded: done, see InvoiceCode. Failed: it moved no money, see Message; resending
		/// it with the same key executes it again; a transfer whose voucher went back to its sender is Failed too, and final (nothing
		/// moved in the end, the same key only answers it again: a new transfer needs a new key).
		/// Processing: not final yet, either still running or it produced something (see InvoiceCode) whose answer was not
		/// recorded: resend the very same request with the same key to get its final answer, never a new key, which would execute
		/// it a second time. Unsettled: final, its money moved and it cannot be completed: not executed again, contact support.
		/// NotFound and Failed do not prove that no call is still on its way: resending with the same key is always safe.
		/// </summary>
		public string Status { get; set; } = "NotFound";
		public DateTime? CreatedUtc { get; set; }
		public DateTime? LastUpdateUtc { get; set; }
		/// <summary>The voucher or invoice the operation produced, also listed in Invoices: when it succeeded, or when it is Processing with its money moved.
		/// Null for a support account when it is not listed with its codes: someone else's open voucher, or an invoice not listed (see <see cref="ResponseCheckUnique"/>)</summary>
		public Guid? InvoiceCode { get; set; }
		/// <summary>What to know about the state: the message the operation answered with when it failed, what is pending when it is Processing.
		/// For a support account, an invoice id in it that is not listed with its codes reads "[withheld]" (see <see cref="ResponseCheckUnique"/>)</summary>
		public string? Message { get; set; }
	}

	/// <summary>
	/// Returned by <c>CheckUnique</c>: the invoices matching the query, and the state of the operation created with that
	/// unique key, when the query is one. An integrator sees the invoices it is a party of (by callback data only the ones it
	/// created, the callback data being its own) and its own operations; an account with the CanSeeTransactionInfo permission
	/// (support) sees every one of the client, but not the codes that redeem someone else's open voucher or red envelope: for those,
	/// <see cref="ResponseInvoiceInfo.InvoiceCode"/> is the all-zero guid (<see cref="ResponseInvoiceInfo.CodesWithheld"/> true: the code is
	/// withheld, redeeming it only answers "not found") and <see cref="ResponseInvoiceInfo.FriendlyVoucherCode"/> is null (since 2026-10-31). The codes are given for a payment request, an invoice the account issued, a voucher already
	/// redeemed (a red envelope: closed), and a withdrawal, a deposit or a swap. For such an account
	/// <see cref="CheckedUniqueRequest.InvoiceCode"/> is null whenever the produced invoice is not listed with its codes (e.g. a
	/// swap, never listed), and every invoice id in <see cref="CheckedUniqueRequest.Message"/> not listed with its codes is replaced
	/// by "[withheld]". Success with an empty list means nothing matched.
	/// </summary>
	public class ResponseCheckUnique : PaygramResponse
	{
		/// <summary>The query as it was looked up: as it was sent, an invoice id without its spaces</summary>
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

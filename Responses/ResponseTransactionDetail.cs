using CurrenciesLib;

namespace PayGram.Public.Responses
{
	/// <summary>
	/// The on-chain leg of a movement, when there is one.
	/// </summary>
	public class BlockchainInfo
	{
		public CryptoTranDirections Direction { get; set; }
		public string Hash { get; set; } = "";
		public string Network { get; set; } = "";
		public string? Token { get; set; }
		public decimal Amount { get; set; }
		/// <summary>
		/// The counterparty address: sender for a deposit, destination for a withdrawal.
		/// </summary>
		public string? Address { get; set; }
		public decimal? Fee { get; set; }
		public string? FeeCurrency { get; set; }
		public DateTime DateUtc { get; set; }
		/// <summary>
		/// A ready-to-open block-explorer URL, built from a configurable per-network template.
		/// </summary>
		public string? ExplorerUrl { get; set; }
	}

	/// <summary>
	/// Returned by <c>GetTransactionDetail</c>: the full picture of one ledger movement —
	/// the two parties, the linked invoice, the merchant (if any), the blockchain leg (if any)
	/// and any external-provider enrichment (e.g. Yapay).
	/// </summary>
	public class ResponseTransactionDetail : PaygramResponse
	{
		public Guid TransactionId { get; set; }
		public DateTime ExecutedUtc { get; set; }
		public decimal Amount { get; set; }
		public Currencies Currency { get; set; }
		/// <summary>
		/// Readable currency symbol — enums serialize as integers on the wire, this carries the name.
		/// </summary>
		public string CurrencyName => Currency.ToString();
		public decimal Commission { get; set; }

		public FundFlowNode? From { get; set; }
		public FundFlowNode? To { get; set; }

		public Guid? InvoiceCode { get; set; }
		public string? FriendlyCode { get; set; }
		public string? InvoiceType { get; set; }
		public int ProviderId { get; set; }

		/// <summary>
		/// The business name when the payment went to a merchant, else null.
		/// </summary>
		public string? MerchantName { get; set; }

		public BlockchainInfo? Blockchain { get; set; }
		public PaymentEnrichment? Enrichment { get; set; }

		public ResponseTransactionDetail() : base(PaygramResponseTypes.ResponseTransactionDetail) { }
		public ResponseTransactionDetail(string msg, ResponseCodes code) : base(msg, PaygramResponseTypes.ResponseTransactionDetail, code) { }
		public ResponseTransactionDetail(ResponseCodes code) : base(PaygramResponseTypes.ResponseTransactionDetail, code) { }
	}
}

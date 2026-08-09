using CurrenciesLib;

namespace PayGram.Public.Responses
{
	public enum FundFlowDirections
	{
		/// <summary>
		/// Money entered the center user's account (counterpart -> center)
		/// </summary>
		In = 0,
		/// <summary>
		/// Money left the center user's account (center -> counterpart)
		/// </summary>
		Out = 1,
	}

	/// <summary>
	/// One movement in the center user's fund flow, with the counterparty resolved.
	/// </summary>
	public class FundFlowEntry
	{
		public long Id { get; set; }
		public Guid TransactionId { get; set; }
		public DateTime ExecutedUtc { get; set; }
		public StatementEntryType EntryType { get; set; }
		public FundFlowDirections Direction { get; set; }
		public decimal Amount { get; set; }
		public Currencies Currency { get; set; }
		/// <summary>
		/// Readable currency symbol — enums serialize as integers on the wire, this carries the name.
		/// </summary>
		public string CurrencyName => Currency.ToString();
		public decimal Fees { get; set; }
		public FundFlowNode Counterpart { get; set; } = new();
		public Guid? InvoiceCode { get; set; }
		public string? FriendlyCode { get; set; }
		/// <summary>
		/// On-chain transaction hash for a deposit/withdrawal, when recorded.
		/// </summary>
		public string? TxHash { get; set; }
		public string? Network { get; set; }
	}

	/// <summary>
	/// Returned by <c>GetFundFlow</c>. The center user plus one hop of movements.
	/// </summary>
	public class ResponseFundFlow : PaygramResponse
	{
		public FundFlowNode Center { get; set; } = new();
		public IList<FundFlowEntry> Entries { get; set; } = new List<FundFlowEntry>();

		public ResponseFundFlow() : base(PaygramResponseTypes.ResponseFundFlow) { }
		public ResponseFundFlow(string msg, ResponseCodes code) : base(msg, PaygramResponseTypes.ResponseFundFlow, code) { }
		public ResponseFundFlow(ResponseCodes code) : base(PaygramResponseTypes.ResponseFundFlow, code) { }
	}
}

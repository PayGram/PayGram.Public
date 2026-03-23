using CurrenciesLib;

namespace PayGram.Public.Responses
{
	/// <summary>
	/// A single ledger entry in a user's statement. Mapped from a <c>DbStatement</c> row.
	/// </summary>
	public class StatementEntry
	{
		/// <summary>
		/// The unique sequential ID of this statement row. Used for cursor-based pagination.
		/// </summary>
		public long Id { get; set; }
		/// <summary>
		/// The ID of the underlying transaction that produced this entry.
		/// </summary>
		public Guid TransactionId { get; set; }
		/// <summary>
		/// The UTC timestamp when the transaction was executed (settled).
		/// </summary>
		public DateTime ExecutedUtc { get; set; }
		/// <summary>
		/// The amount of this entry in <see cref="Currency"/>. Positive for credits, negative for debits.
		/// </summary>
		public decimal Amount { get; set; }
		/// <summary>
		/// The currency of this statement entry.
		/// </summary>
		public Currencies Currency { get; set; }
		/// <summary>
		/// The UTC timestamp when the underlying transaction was created (may differ from <see cref="ExecutedUtc"/>).
		/// </summary>
		public DateTime CreatedUtc { get; set; }
		/// <summary>
		/// The fees associated with this entry, in <see cref="Currency"/>.
		/// </summary>
		public decimal Fees { get; set; }
		/// <summary>
		/// The type of ledger entry (Deposit, Withdrawal, Transfer, Swap, Fee, etc.).
		/// </summary>
		public StatementEntryType EntryType { get; set; }
		/// <summary>
		/// Whether this transaction can be recalled (reversed) by the sender.
		/// </summary>
		public bool Recallable { get; set; }
		/// <summary>
		/// The invoice associated with this entry, containing its GUID and optional friendly voucher code.
		/// </summary>
		public Invoice Invoice { get; set; }
	}
}

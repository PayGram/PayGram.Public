using CurrenciesLib;
using PayGram.Public;
namespace PayGram.Types
{
	public class ResponseStatement : PaygramResponse
	{
		public IList<StatementEntry> StatementEntries { get; set; } = new List<StatementEntry>();
		public int Results => StatementEntries.Count;
		public bool NextPageAvailable { get; set; }
		public long NextStartIdx { get; set; }
		public ResponseStatement() : base(PaygramResponseTypes.ResponseStatement)
		{

		}
	}
	public class StatementEntry
	{
		public DateTime CreatedUtc { get; set; }
		public decimal Amount { get; set; }
		public Currencies Currency { get; set; }
		public decimal Fees { get; set; }
		public StatementEntryType EntryType { get; set; }
		public Guid InvoiceCode { get; set; }
		public bool Recallable { get; set; }
	}
	public enum StatementEntryType
	{
		None,
		Deposit,
		Withdrawal,
		WithdrawalRefund,
		OutgoingTransfer,
		IncomingTransfer,
		ChangeSell,
		ChangeBuy,
		Fee,
		Other
	}
}

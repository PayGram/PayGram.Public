using CurrenciesLib;

namespace PayGram.Public.Responses
{
	public class StatementEntry
    {
        public long Id { get; set; }
        public Guid TransactionId { get; set; }
        public DateTime ExecutedUtc { get; set; }
        public decimal Amount { get; set; }
        public Currencies Currency { get; set; }
        public DateTime CreatedUtc { get; set; }
        public decimal Fees { get; set; }
        public StatementEntryType EntryType { get; set; }
        public Guid? InvoiceCode { get; set; }
        public bool Recallable { get; set; }
    }
}

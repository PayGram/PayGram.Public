using CurrenciesLib;

namespace PayGram.Public.Responses
{
    public class ResponseStatement : PaygramResponse
    {
        public IList<StatementEntry> StatementEntries { get; set; } = new List<StatementEntry>();
        public int Results => StatementEntries.Count;
        public bool NextPageAvailable { get; set; }
        //public long NextStartIdx { get; set; }
        public ResponseStatement() : base(PaygramResponseTypes.ResponseStatement)
        {

        }
    }
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
    public enum StatementEntryType
    {
        None = 0,
        Deposit = 1,
        Withdrawal = 2,
        WithdrawalRefund = 3,
        OutgoingTransfer = 4,
        OutgoingDirectTransfer = 5,
        IncomingTransfer = 6,
        IncomingDirectTransfer = 7,
        ChangeSell = 8,
        ChangeBuy = 9,
        Fee = 10,
        Other = 11
    }
}

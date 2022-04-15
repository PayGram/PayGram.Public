namespace PayGram.Public
{
    public enum InvoiceStatuses
    {
        Requested = 1,
        Completed = 2,
        Cancelled = 3,
        OnGoing = 4,
        Error = 5,
        Denied = 7,
        AcceptedPendingTransfer = 8
    }
}

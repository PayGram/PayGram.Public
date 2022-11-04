namespace PayGram.Public
{
	public class RequestCreateRedEnvelope : PaygramRequest
    {
        public int MaxNumOfRedeemers { get; set; }
        public decimal Amount { get; set; }
    }
}

namespace PayGram.Public.Responses
{
    public class ResponseTransferCredit : PaygramResponse
    {

        public ResponseTransferCredit()
                        : base((PaygramResponseTypes)Enum.Parse(typeof(PaygramResponseTypes), nameof(ResponseTransferCredit)))
        {
        }

        public ResponseTransferCredit(string err)
                        : base(err)
        {
            Type = PaygramResponseTypes.ResponseTransferCredit;
        }

        public decimal FromCredit { get; set; }
        public decimal ToCredit { get; set; }
        public Guid VoucherCode { get; set; }
        /// <summary>
        /// When the transfer happened directly between two users, it will be null.
        /// If the user has created a voucher, it will be the link that the user can use to redeem the value of this voucher
        /// </summary>
        public string? RedeemUrl { get; set; }
    }
}

using PayGram.Public;

namespace PayGram.Types
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
    }
}

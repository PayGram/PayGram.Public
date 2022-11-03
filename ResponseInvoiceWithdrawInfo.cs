using PayGram.Public;
using PayGram.Public.UserAPI;

namespace PayGram.Public
{
    public class ResponseInvoiceWithdrawInfo : ResponseInvoiceInfo
    {
        public WithdrawMethod WithdrawMethod { get; set; }
        /// <summary>
        /// Gets or sets whether the withdraw was refunded back to the user
        /// </summary>
        public bool Refunded { get; set; }

        public ResponseInvoiceWithdrawInfo()
            : base()
        {
            Type = PaygramResponseTypes.ResponseInvoiceWithdrawInfo;
        }
    }
}

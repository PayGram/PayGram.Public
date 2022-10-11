using CurrenciesLib;
using PayGram.Public;

namespace PayGram.Types
{

    public class RequestIssueInvoice : PaygramRequest
    {
        /// <summary>
        /// The currency for this invoice
        /// </summary>
        public Currencies CurrencyCode { get; set; }
        /// <summary>
        /// The amount of money, included the fees, that the user is asking to be paid
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// The data to send back to the user when this invoice is paid
        /// </summary>
        public string? CallbackData { get; set; }
        /// <summary>
        /// The type of merchant who issued this invoice
        /// </summary>
        public MerchantTypes MerchantType { get; set; }
    }
}

using CurrenciesLib;

namespace PayGram.Public.UserAPI
{
	/// <summary>
	/// A payment request of yours nobody paid within its validity: it is now <see cref="InvoiceStatuses.Expired"/> and
	/// cannot be paid any more. No money moved. Issue a new one if the customer still wants to pay.
	/// </summary>
	public class UserCallbackInvoiceExpired
	{
		public Guid InvoiceCode { get; set; }
		public string? FriendlyVoucherCode { get; set; }
		/// <summary>The amount that was requested</summary>
		public decimal Amount { get; set; }
		public Currencies Currency { get; set; }
		public DateTime CreatedUtc { get; set; }
		/// <summary>When the request stopped being payable</summary>
		public DateTime ExpiredUtc { get; set; }
	}
}

using Newtonsoft.Json;
using PayGram.Public.Responses;

namespace PayGram.Public.UserAPI
{
	public class UserCallbackInvoiceInfo : UserCallbackBalanceInfo
	{
		public string? FriendlyVoucherCode { get; set; }
		public Guid InvoiceCode { get; set; }
		// the counterpart (who paid, who redeemed, who sent) is UserIdInClientFrom / FromClientId of UserCallbackBalanceInfo
		/// <summary>
		/// To the payer of a payment funded by an automatic swap only: the swap, with the balance of its source currency after it
		/// (the Balance of this callback is the one in the invoice's currency). Absent in every other callback, the merchant's too
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public PaySwapInfo? Swap { get; set; }
	}
}

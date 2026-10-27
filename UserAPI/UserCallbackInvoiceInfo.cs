using Newtonsoft.Json;
using PayGram.Public.Responses;

namespace PayGram.Public.UserAPI
{
	public class UserCallbackInvoiceInfo : UserCallbackBalanceInfo
	{
		public string? FriendlyVoucherCode { get; set; }
		/// <summary>
		/// The code of the invoice (payment request, voucher, red envelope). Always set on this callback. The same value as
		/// <see cref="UserCallbackBalanceInfo.InvoiceCode"/> (it is stored there), kept as a non-nullable guid for compatibility
		/// </summary>
		public new Guid InvoiceCode
		{
			get => base.InvoiceCode ?? Guid.Empty;
			set => base.InvoiceCode = value;
		}
		// the counterpart (who paid, who redeemed, who sent) is UserIdInClientFrom / FromClientId of UserCallbackBalanceInfo
		/// <summary>
		/// To the payer of a payment funded by an automatic swap only: the swap, with the balance of its source currency after it
		/// (the Balance of this callback is the one in the invoice's currency). Absent in every other callback, the merchant's too
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public PaySwapInfo? Swap { get; set; }
	}
}

using Newtonsoft.Json;

namespace PayGram.Public.Responses
{
	/// <summary>
	/// Returned by <c>SwapAsync</c> / <c>SwapV2</c>.
	/// Contains the result of exchanging funds from one currency to another in the user's account.
	/// </summary>
	public class ResponseSwap : PaygramResponse
	{
		/// <summary>
		/// The amount received in the destination currency after the swap.
		/// </summary>
		public decimal ReceivedAmount { get; set; }
		/// <summary>
		/// The fees charged for the swap, expressed in the source currency.
		/// </summary>
		public decimal FeesRequestedCurrency { get; set; }
		/// <summary>
		/// The user's balance in the source currency after the swap.
		/// </summary>
		public decimal BalanceSourceAfter { get; set; }
		/// <summary>
		/// The user's balance in the destination currency after the swap.
		/// </summary>
		public decimal BalanceDestAfter { get; set; }
		/// <summary>
		/// The code (guid) of the swap: the invoice its two statement entries (the sale and the purchase) show. On a success (and its
		/// replay); absent on a simulation and on a failure. Added in 3.9.0
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Guid? InvoiceCode { get; set; }
		/// <summary>
		/// The friendly code (12 characters) of the swap, with <see cref="InvoiceCode"/>: the one its two statement entries show.
		/// Absent when <see cref="InvoiceCode"/> is. Added in 3.9.0
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string? FriendlyVoucherCode { get; set; }
		public ResponseSwap() : base(PaygramResponseTypes.ResponseSwap)
		{

		}
		public ResponseSwap(string err)
					: base(err, PaygramResponseTypes.ResponseSwap, ResponseCodes.ResponseGenericError)
		{
		}
		public ResponseSwap(ResponseCodes code)
					: base(PaygramResponseTypes.ResponseSwap, code)
		{
		}
	}
}

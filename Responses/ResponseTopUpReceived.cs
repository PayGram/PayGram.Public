using CurrenciesLib;

namespace PayGram.Public.Responses
{
	/// <summary>
	/// Returned by <c>RedeemVoucher</c> / <c>RedeemVoucherV2</c> when a voucher or red envelope is successfully redeemed.
	/// Contains the amount credited to the user's balance after fees.
	/// </summary>
	public class ResponseTopUpReceived : PaygramResponse
	{
		/// <summary>
		/// The gross amount received from the invoice, in <see cref="CurrencyCode"/>, before fees.
		/// Set from the invoice's Amount.
		/// </summary>
		public decimal Received { get; set; }
		/// <summary>
		/// The net amount credited to the user's balance after fees (Received - Fees). Computed property.
		/// </summary>
		public decimal Credited => Received - Fees;
		/// <summary>
		/// The fees deducted from this redemption, in <see cref="CurrencyCode"/>.
		/// Set from the invoice's Fees.
		/// </summary>
		public decimal Fees { get; set; }
		/// <summary>
		/// The currency in which the voucher was denominated and the credit was applied.
		/// </summary>
		public Currencies CurrencyCode { get; set; }
		/// <summary>
		/// The user's balance in <see cref="CurrencyCode"/> after the redemption has been applied.
		/// </summary>
		public decimal NewBalance { get; set; }

		public ResponseTopUpReceived()
							: base(PaygramResponseTypes.ResponseTopUpReceived)
		{

		}
		public ResponseTopUpReceived(ResponseCodes code) : base(PaygramResponseTypes.ResponseTopUpReceived, code)
		{

		}
		public ResponseTopUpReceived(string msg, ResponseCodes code) : base(msg, PaygramResponseTypes.ResponseTopUpReceived, code)
		{

		}
	}
}

using CurrenciesLib;

namespace PayGram.Public.Responses
{
	/// <summary>
	/// Base response for deposit/top-up operations. Returned by <c>DepositCredit</c> / <c>DepositCreditV2</c>.
	/// The actual runtime type is typically <see cref="ResponseTopUpCryptapi"/> which adds the deposit address.
	/// </summary>
	public class ResponseTopUp : PaygramResponse
	{
		/// <summary>
		/// The total amount the user requested to deposit, expressed in the chosen fiat/crypto currency.
		/// Set from the invoice's Amount.
		/// </summary>
		public decimal AmountToSendInChosenCurrency { get; set; }
		/// <summary>
		/// The total fees that will be deducted from the deposit, in the deposit currency.
		/// Set from the invoice's Fees.
		/// </summary>
		public decimal CoinTotalFees { get; set; }
		/// <summary>
		/// The gross coin amount before fees are deducted (CoinAmountWillReceive + CoinTotalFees). Computed property.
		/// </summary>
		public decimal CoinAmountExcFees { get { return CoinAmountWillReceive + CoinTotalFees; } }
		/// <summary>
		/// The net amount that will be credited to the user's balance after fees are deducted.
		/// Equals AmountToSendInChosenCurrency minus CoinTotalFees.
		/// </summary>
		public decimal CoinAmountWillReceive { get; set; }
		/// <summary>
		/// The effective exchange rate excluding fees (AmountToSendInChosenCurrency / CoinAmountExcFees). Computed property, 0 while the amounts are not known (an error response).
		/// </summary>
		public decimal ChangeRateExclFees => CoinAmountExcFees == 0 ? 0 : AmountToSendInChosenCurrency / CoinAmountExcFees;
		/// <summary>
		/// The effective exchange rate including fees (AmountToSendInChosenCurrency / CoinAmountWillReceive). Computed property, 0 while the amounts are not known (an error response).
		/// </summary>
		public decimal ChangeRateInclFees => CoinAmountWillReceive == 0 ? 0 : AmountToSendInChosenCurrency / CoinAmountWillReceive;

		public ResponseTopUp()
					: base(PaygramResponseTypes.ResponseTopUp)
		{

		}
		public ResponseTopUp(string err)
					: base(err, PaygramResponseTypes.ResponseTopUp, ResponseCodes.ResponseGenericError)
		{
		}
		public ResponseTopUp(ResponseCodes code)
					: base(PaygramResponseTypes.ResponseTopUp, code)
		{
		}
	}
}

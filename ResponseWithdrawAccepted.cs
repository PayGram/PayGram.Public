using PayGram.Public;

namespace PayGram.Types
{
	public class ResponseWithdrawAccepted : PaygramResponse
	{
		/// <summary>
		/// The balance after witdrawing the funds of the selected currency-account
		/// </summary>
		public decimal NewBalance { get; set; }
		/// <summary>
		/// The fee for this transfer expressed in the CurrencyCode
		/// </summary>
		public decimal ExpectedFee { get; set; }
		/// <summary>
		/// The amount that will be deducted from the source account expressed by the CurrencyCode
		/// </summary>
		public decimal WithdrawnAmount { get; set; }
		/// <summary>
		/// The amount that will be sent to the user expressed in his chosen currency, net of the PayGram and crypto transaction fees, but excluding 
		/// additional interidiary bank or beneficiary bank fees. This will likely be the received amount.
		/// </summary>
		public decimal AmountWillReceive => WithdrawnAmount = ExpectedFee;
		/// <summary>
		/// The unique identifier for the invoice that identifies the withdraw operation
		/// </summary>
		public Guid InvoiceCode { get; set; }

		public ResponseWithdrawAccepted()
									: base((PaygramResponseTypes)Enum.Parse(typeof(PaygramResponseTypes), nameof(ResponseWithdrawAccepted)))
		{

		}
	}
}
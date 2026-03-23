namespace PayGram.Public.Responses
{
	/// <summary>
	/// Returned by <c>WithdrawAsync</c> / <c>WithdrawV2</c>.
	/// Indicates that a cryptocurrency withdrawal has been accepted for processing.
	/// The withdrawal still goes through an approval process before being executed on-chain.
	/// </summary>
	public class ResponseWithdrawAccepted : PaygramResponse
	{
		/// <summary>
		/// The user's balance in the withdrawal currency after the funds have been reserved for withdrawal.
		/// </summary>
		public decimal NewBalance { get; set; }
		/// <summary>
		/// The fee for this withdrawal, expressed in the withdrawal currency.
		/// Includes PayGram fees and estimated crypto transaction fees.
		/// </summary>
		public decimal ExpectedFee { get; set; }
		/// <summary>
		/// The total amount deducted from the user's account, expressed in the withdrawal currency.
		/// This includes both the amount to be sent and the fees.
		/// </summary>
		public decimal WithdrawnAmount { get; set; }
		/// <summary>
		/// The net amount the user will receive at the destination address (WithdrawnAmount - ExpectedFee).
		/// Excludes any intermediary or beneficiary bank fees which may apply additionally.
		/// </summary>
		public decimal AmountWillReceive => WithdrawnAmount = ExpectedFee;
		/// <summary>
		/// The GUID of the invoice tracking this withdrawal. Can be used with <c>InvoiceInfo</c> to check withdrawal status.
		/// </summary>
		public Guid InvoiceCode { get; set; }

		public ResponseWithdrawAccepted()
									: base(PaygramResponseTypes.ResponseWithdrawAccepted)
		{

		}
	}
}

namespace PayGram.Public.Responses
{
	/// <summary>
	/// Returned by <c>DepositCredit</c> / <c>DepositCreditV2</c> for cryptocurrency deposits.
	/// Extends <see cref="ResponseTopUp"/> with the blockchain address where the user should send funds.
	/// </summary>
	public class ResponseTopUpCryptapi : ResponseTopUp
	{
		/// <summary>
		/// The cryptocurrency address the user must send funds to. Once funds arrive, they are credited to the user's PayGram balance.
		/// </summary>
		public string? SendToAddress { get; set; }
		public ResponseTopUpCryptapi() : base()
		{
			Type = PaygramResponseTypes.ResponseTopUpCryptapi;
		}
		public ResponseTopUpCryptapi(string sendToAddress)
		{
			SendToAddress = sendToAddress;
			Type = PaygramResponseTypes.ResponseTopUpCryptapi;
		}
	}
}

using Newtonsoft.Json;

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
		/// <summary>
		/// The code (guid) of the deposit account bound to <see cref="SendToAddress"/>: the <c>InvoiceCode</c> the deposit
		/// callbacks and statement entries of the deposits to that address carry (the same on every request that returns the
		/// same address). A deposit of another token sent to the same address is credited to the user's account for that token,
		/// with its own code. Absent when it could not be read. Added in 3.9.0
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Guid? InvoiceCode { get; set; }
		/// <summary>
		/// The friendly code (12 characters) of the same deposit account as <see cref="InvoiceCode"/>. Absent when it has none (an
		/// account created before 2023-12-20) or could not be read. Added in 3.9.0
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string? FriendlyVoucherCode { get; set; }
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

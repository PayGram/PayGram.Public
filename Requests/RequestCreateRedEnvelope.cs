using CurrenciesLib;

namespace PayGram.Public
{
	public class RequestCreateRedEnvelope : PaygramRequest
	{
		/// <summary>
		/// The currency for this invoice
		/// </summary>
		public Currencies CurrencyCode { get; set; }
		public int MaxNumOfRedeemers { get; set; }
		public decimal Amount { get; set; }
		/// <summary>
		/// The data to send back to the user when this invoice is paid
		/// </summary>
		public string? CallbackData { get; set; }
	}
}

using CurrenciesLib;

namespace PayGram.Public.Requests
{
	public class RequestTransferCredit : PaygramRequest
	{
		/// <summary>
		/// The currency of the amount that should be sent
		/// </summary>
		public Currencies CurrencyCode { get; set; }
		/// <summary>
		/// The username of the user at the client side that is receiving the money or null to create an open Invoice
		/// </summary>
		public string? ToUserCliId { get; set; }
		/// <summary>
		/// The amount to send
		/// </summary>
		public decimal Amount { get; set; }
		/// <summary>
		/// The data to send back to the user when the credit is transferred.
		/// </summary>
		public string? CallbackData { get; set; }

		public RequestTransferCredit()
		{

		}
	}
}

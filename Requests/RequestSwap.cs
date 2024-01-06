using CurrenciesLib;

namespace PayGram.Public.Requests
{
	public class RequestSwap : PaygramRequest
	{
		public Currencies FromCurrency { get; set; }
		public Currencies ToCurrency { get; set; }
		public decimal AmountToSwap { get; set; }
		public string? CallbackData { get; set; }
		public bool IsSimulation { get; set; }
	}
}

using CurrenciesLib;

namespace PayGram.Public
{
	public class RequestSimplex : PaygramRequest
	{
		public int Provider { get; set; }
		public Currencies RequestCurrency { get; set; }
		public Currencies DigitalCurrency { get; set; }
		public Currencies FiatCurrency { get; set; }
		public decimal RequestAmount { get; set; }
		public string? CallbackData { get; set; }
		public string ClientIP { get; set; }
		public string EmailAddress { get; set; }
		public string PhoneNumber { get; set; }
	}
}

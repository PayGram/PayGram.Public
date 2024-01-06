namespace PayGram.Public
{
	public class ResponseSimplexEvents
	{
		public List<Event> events { get; set; }
	}

	public class Event
	{
		public string event_id { get; set; }
		public string name { get; set; }
		public Payment payment { get; set; }
		public DateTime timestamp { get; set; }
	}

	public class Payment
	{
		public string id { get; set; }
		public string status { get; set; }
		public DateTime created_at { get; set; }
		public int partner_id { get; set; }
		public DateTime updated_at { get; set; }
		public string crypto_currency { get; set; }
		public FiatEvent fiat_total_amount { get; set; }
		public CryptoEvent crypto_total_amount { get; set; }
		public string partner_end_user_id { get; set; }
	}

	public class FiatEvent
	{
		public decimal? amount { get; set; }
		public string currency { get; set; }
	}

	public class CryptoEvent
	{
		public decimal? amount { get; set; }
		public string currency { get; set; }
	}
}

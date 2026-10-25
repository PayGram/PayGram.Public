namespace PayGram.Public
{
	public static class Constraints
	{

		public const int CALLBACKURL_MAX = 512;
		/// <summary>The longest callback data an invoice keeps (Invoices.CallbackDataToClient, looked up by an index): a longer one is refused</summary>
		public const int CALLBACK_DATA_MAX = 256;
		/// <summary>The longest PayerRef (an integrator's own id of its end user who pays) a transfer accepts: a longer one is refused</summary>
		public const int PAYER_REF_MAX = 64;

		public const int NOTIFICATION_PAGE_SIZE = 20;
		public const int NOTIFICATION_RETRY_FOR_DAYS = 15;
		public const int NOTIFICATION_RETRY_EVERY_MINUTES = 5;
	}
}

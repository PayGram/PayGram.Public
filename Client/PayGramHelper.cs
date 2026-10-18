namespace PayGram.Public.Client
{
	public class PayGramHelper
	{
		/// <summary>The production endpoint of the PayGram users api. Pass another base url to the PayGramBotClient constructor to use a different server.</summary>
		public const string PAYGRAM_BOT_API_URL = "https://pubapi.paygr.am/PayGramUsers/";
		public const string PAYGRAM_BOT_ENDPOINT = PAYGRAM_BOT_API_URL + "{0}/{1}/{2}";
		public const string TRANSFER_METHOD = "TransferCredit";
		public const string TRANSFER_V2_METHOD = "TransferCreditV2";
		public const string DEPOSIT_METHOD = "DepositCredit";
		public const string UPDATES_METHOD = "GetUpdates";
		public const string CANCEL_PENDING_CALLBACKS_METHOD = "CancelPendingCallbacks";
		public const string INVOICE_INFO_METHOD = "InvoiceInfo";
		public const string EXCHANGE_RATES_METHOD = "GetExchangeRates";
		public const string CONVERT_METHOD = "Convert";
		public const string SWAP_METHOD = "Swap";
		public const string ISSUEINVOICE_METHOD = "IssueInvoice";
		public const string ISSUEINVOICE_V2_METHOD = "IssueInvoiceV2";
		public const string SWAP_V2_METHOD = "SwapV2";
		public const string WITHDRAW_V2_METHOD = "WithdrawV2";
		public const string REDEEM_VOUCHER_V2_METHOD = "RedeemVoucherV2";
		public const string PAY_VOUCHER_V2_METHOD = "PayVoucherV2";
		public const string GET_STATEMENT_V2_METHOD = "GetStatementV2";

		// tokens are used in the PayGramUsersController as query string parameters
		public const string INVOICEID_TOKEN_NAME = "invguid";
		public const string TO_TOKEN_NAME = "to";
		public const string CURRENCY_SYMBOL_TOKEN_NAME = "cursym";
		public const string NETWORK_SYMBOL_TOKEN_NAME = "network";
		public const string CURRENCY_SYMBOL_DEST_TOKEN_NAME = "cursymdest";
		public const string AMT_TOKEN_NAME = "amt";
		public const string METHOD_TOKEN_NAME = "method";
		public const string API_TOKEN_NAME = "apitoken";
		public const string CALLBACKDATA_TOKEN_NAME = "cd";
		public const string UNIQUE_TOKEN_NAME = "unique";
	}
}

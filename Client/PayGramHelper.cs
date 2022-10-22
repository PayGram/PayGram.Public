using System.Globalization;
using Telegram.Messaging.Types;
using Utilities.String.Extentions;

namespace PayGram.Public.Client
{
	public class PayGramHelper
	{
		public const string PAYGRAM_BOT_API_URL = "https://api.paygr.am:8443/PayGramUsers/";
		public const string PAYGRAM_BOT_ENDPOINT = PAYGRAM_BOT_API_URL + "{0}/{1}/{2}";
		public const string TRANSFER_METHOD = "TransferCredit";
		public const string DEPOSIT_METHOD = "DepositCredit";
		public const string UPDATES_METHOD = "GetUpdates";
		public const string INVOICE_INFO_METHOD = "InvoiceInfo";
		public const string EXCHANGE_RATES_METHOD = "GetExchangeRates";
		public const string CONVERT_METHOD = "Convert";
		public const string SWAP_METHOD = "Swap";

		// tokens are used in the PayGramUsersController as query string parameters
		public const string INVOICEID_TOKEN_NAME = "invguid";
		public const string TO_TOKEN_NAME = "to";
		public const string CURRENCY_SYMBOL_TOKEN_NAME = "cursym";
		public const string NETWORK_SYMBOL_TOKEN_NAME = "network";
		//public const string PAY_CURRENCY_SYMBOL_TOKEN_NAME = "paycursym";
		public const string CURRENCY_SYMBOL_DEST_TOKEN_NAME = "cursymdest";
		public const string AMT_TOKEN_NAME = "amt";
		public const string METHOD_TOKEN_NAME = "method";
		public const string API_TOKEN_NAME = "apitoken";
		public const string CALLBACKDATA_TOKEN_NAME = "cd";
		public const string UNIQUE_TOKEN_NAME = "unique";

		public const string ACTION_TAG = "a";
		public const string CODE_TAG = "c";
		public const string TO_TAG = "t";
		public const string AMOUNT_TAG = "m";
		//public const string CALLBACK_TAG = "cb";
		public const string CALLBACKDATA_TAG = "d";
		public const string CURRENCY_TAG = "y";

		public const string VOUCHER_PARAM = "v";
		public const string PAY_PARAM = "p";
		public const string INVOICE_PARAM = "i";
#if RELEASE
        public const string PAYGRAM_BOTUSERNAME = "opgmbot";

#else
		public const string PAYGRAM_BOTUSERNAME = "OfficialPayGram_Bot";
#endif

		/// <summary>
		/// Makes a link to the PayGram bot specifing the shown label and eventually the parameters that follow the start
		/// </summary>
		/// <param name="label">What the user will see</param>
		/// <param name="query">The parameters that follow the start. Should not include start=. If it is not a base64 string, it will be converted to base64</param>
		/// <returns>The text containing the url</returns>
		public static string PayGramHyperLink(string? label = "PayGram", string? query = null)
		{
			if (label == null) label = "";
			if (query == null) query = "";
			else
			{
				var original = query.Base64Decode();
				if (original == null)//string must be encoded
					query = query.Base64Encode();
				query = $"?start={query}";
			}
			return $"<a href=\"https://t.me/{PAYGRAM_BOTUSERNAME}{query}\">{label}</a>";
		}
		/// <summary>
		/// Makes a link to the PayGram bot specifing the shown label and eventually the parameters that follow the start
		/// </summary> 
		/// <param name="query">The parameters that follow the start. Should not include start=. If it is not a base64 string, it will be converted to base64</param>
		/// <returns>The text containing the url</returns>
		public static string PayGramOnlyLink(string query = null)
		{
			if (query == null) query = "";
			else
			{
				var original = query.Base64Decode();
				if (original == null)//string must be encoded
					query = query.Base64Encode();
				query = $"?start={query}";
			}
			return $"https://t.me/{PAYGRAM_BOTUSERNAME}{query}";
		}

		/// <summary>
		/// Creates a payment url
		/// </summary>
		/// <param name="amount">usd amount must be positive or null  will be returned</param>
		/// <param name="toTid">The telegram id of the user that will receive the payment</param>
		/// <param name="label">The text to display on the url</param>
		/// <param name="callbackData">The data that you will receive when the users has completed the payment</param>
		/// <returns>A payment url or null  if the value is &lt;=0</returns>
		public static string? MakePaymentUrl(double amount, long toTid, string label, string callbackData)
		{
			if (amount <= 0) return null;

			var query = $"{ACTION_TAG}={PAY_PARAM}&{TO_TAG}={toTid:X}&{AMOUNT_TAG}={amount.ToString("0.00", CultureInfo.InvariantCulture)}&{CALLBACKDATA_TAG}={callbackData}";

			return PayGramHyperLink(label, query);
		}

		/// <summary>
		/// Creates a payment url
		/// </summary>
		/// <param name="amount">usd amount must be positive or null  will be returned</param>
		/// <param name="toTid">The telegram id of the user that will receive the payment</param>
		/// <param name="callbackData">The data that you will receive when the users has completed the payment</param>
		/// <returns>A payment url or null  if the value is &lt;=0</returns>
		public static string? MakePaymentUrl(double amount, long toTid, string callbackData)
		{
			if (amount <= 0) return null;

			var query = $"{ACTION_TAG}={PAY_PARAM}&{TO_TAG}={toTid:X}&{AMOUNT_TAG}={amount.ToString("0.00", CultureInfo.InvariantCulture)}&{CALLBACKDATA_TAG}={callbackData}";
			return PayGramOnlyLink(query);
		}
		/// <summary>
		/// Creates a link to redeem an invoice
		/// </summary>
		/// <param name="g"></param>
		/// <returns></returns>
		public static string MakeVoucherLink(Guid g, string botName)
		{
			var cmd = TelegramCommand.EscapeCommandValue($"{PayGramHelper.ACTION_TAG}{TelegramCommand.PARAMS_EQUAL_SEP}{PayGramHelper.VOUCHER_PARAM}{TelegramCommand.PARAMS_AND_SEP}{PayGramHelper.CODE_TAG}{TelegramCommand.PARAMS_EQUAL_SEP}{g}");
			return $"https://t.me/{botName}?{TelegramCommand.START}{TelegramCommand.PARAMS_EQUAL_SEP}{cmd}";
		}
		/// <summary>
		/// Creates a link to pay an invoice
		/// </summary>
		/// <param name="g"></param>
		/// <returns></returns>
		public static string MakeInvoiceLink(Guid g, string botName)
		{
			var cmd = TelegramCommand.EscapeCommandValue($"{PayGramHelper.ACTION_TAG}{TelegramCommand.PARAMS_EQUAL_SEP}{PayGramHelper.INVOICE_PARAM}{TelegramCommand.PARAMS_AND_SEP}{PayGramHelper.CODE_TAG}{TelegramCommand.PARAMS_EQUAL_SEP}{g}");
			return $"https://t.me/{botName}?{TelegramCommand.START}{TelegramCommand.PARAMS_EQUAL_SEP}{cmd}";
		}
	}
}

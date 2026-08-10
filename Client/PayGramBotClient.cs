using CurrenciesLib;
using CurrenciesLib.Cryptos;
using log4net;
using Newtonsoft.Json;
using PayGram.Public.Requests;
using PayGram.Public.Responses;
using System.Globalization;
using System.Text;

namespace PayGram.Public.Client
{
	public class PayGramBotClient
	{
		static private readonly ILog log = LogManager.GetLogger(typeof(PayGramBotClient));
		static readonly HttpClient _httpClient = new();

		public string Token { get; set; }

		public PayGramBotClient(string token)
		{
			Token = token;
		}

		/// <summary>
		/// Sends the money directly from ones account to someone else's account
		/// </summary>
		/// <param name="toTid">The id of the person to send the funds to</param>
		/// <param name="amount">The amount to send</param>
		/// <param name="curSym">The currency symbol, any of the <see cref="CurrenciesLib.Currencies"/> .ToString()</param>
		/// <param name="unique">The unique code for this request</param>
		/// <returns>True if it succeded, false otherwise</returns>
		public async Task<bool> TransferMoneyAsync(long toTid, decimal amount, Currencies curSym, string unique, string callbackData)
		{
			if (amount <= 0) return false;
			var response = await ExecuteMethodAsync(Token, PayGramHelper.TRANSFER_METHOD, $"{PayGramHelper.CURRENCY_SYMBOL_TOKEN_NAME}={curSym}&{PayGramHelper.AMT_TOKEN_NAME}={amount.ToString(CultureInfo.InvariantCulture)}&{PayGramHelper.TO_TOKEN_NAME}={toTid}&{PayGramHelper.UNIQUE_TOKEN_NAME}={unique}&{PayGramHelper.CALLBACKDATA_TOKEN_NAME}={callbackData}", null);
			log.Debug($"transferring {amount} to tid {toTid}, response: {response}");
			if (response == null)
			{
				log.Error($"error transferring {amount} to tid {toTid}, PayGram bot returned null");
				return false;
			}
			try
			{
				var res = JsonConvert.DeserializeObject<PaygramResponse>(response);
				return res?.Success ?? false;
			}
			catch (Exception e)
			{
				log.Error($"error transferring {amount} to tid {toTid}", e);
			}
			return false;
		}
		public async Task<Guid> IssueInvoiceAsync(decimal amount, Currencies curSym, string callBackdata)
		{
			/*string token
		    , [FromQuery] decimal amt
		    , [FromQuery] Currencies cursym
		    , [FromQuery] string? callbackData*/

			if (amount <= 0) return Guid.Empty;
			var response = await ExecuteMethodAsync(Token, PayGramHelper.ISSUEINVOICE_METHOD, $"{PayGramHelper.CURRENCY_SYMBOL_TOKEN_NAME}={curSym}&{PayGramHelper.AMT_TOKEN_NAME}={amount.ToString(CultureInfo.InvariantCulture)}&{PayGramHelper.CALLBACKDATA_TOKEN_NAME}={callBackdata}", null);
			log.Debug($"IssueInvoiceAsync {amount} {curSym}, response: {response}");
			if (response == null)
			{
				log.Error($"error IssueInvoiceAsync {amount} {curSym}, PayGram bot returned null");
				return Guid.Empty;
			}
			try
			{
				var res = JsonConvert.DeserializeObject<ResponseIssueInvoice>(response);
				return res?.Success == true ? res.InvoiceCode : Guid.Empty;
			}
			catch (Exception e)
			{
				log.Error($"error IssueInvoiceAsync {amount} {curSym} - {response}", e);
			}
			return Guid.Empty;

		}
		/// <summary>
		/// Asks PayGram to generate a crypto currency address where to send funds. when funds arrive, it will be sent to paygram
		/// </summary>
		/// <param name="amount">The amount to deposit</param>
		/// <param name="amountCurrency">The crypto currency symbol </param>
		/// <param name="callbackData">The data to receive on a callback when the user has deposited</param>
		/// <returns>ResponseTopUp or ResponseTopUpCryptapi</returns> 
		public async Task<ResponseTopUp> DepositCreditAsync(double amount, Currencies amountCurrency, CryptoNetworks network, string callbackData)
		{
			if (amount <= 0) return new ResponseTopUp() { ResponseCode = ResponseCodes.ResponseGenericError, Message = "Amount cannot be negative" };
			var response = await ExecuteMethodAsync(Token, PayGramHelper.DEPOSIT_METHOD, $"{PayGramHelper.CURRENCY_SYMBOL_TOKEN_NAME}={amountCurrency}&{PayGramHelper.NETWORK_SYMBOL_TOKEN_NAME}={network}&{PayGramHelper.AMT_TOKEN_NAME}={amount.ToString(CultureInfo.InvariantCulture)}&{PayGramHelper.CALLBACKDATA_TOKEN_NAME}={callbackData}", null);
			log.Debug($"DepositMoneyAsync {amount} {amountCurrency}, response: {response}");
			if (string.IsNullOrWhiteSpace(response))
			{
				log.Error($"error DepositCreditAsync {amount} {amountCurrency}, PayGram bot returned null");
				return new ResponseTopUp() { ResponseCode = ResponseCodes.ResponseGenericError, Message = "Unexpected error" };
			}
			try
			{
				var res = JsonConvert.DeserializeObject<ResponseTopUp>(response);
				return res.Type == PaygramResponseTypes.ResponseTopUpCryptapi ? JsonConvert.DeserializeObject<ResponseTopUpCryptapi>(response) : res;
			}
			catch (Exception e)
			{
				log.Error($"error DepositCreditAsync {amount} {amountCurrency}", e);
				return new ResponseTopUp() { ResponseCode = ResponseCodes.ResponseGenericError, Message = "Unexpected error" };
			}
		}
		/// <summary>
		/// Converts an amount from srcCurr to destCurr
		/// </summary>
		/// <param name="srcCurr"></param>
		/// <param name="destCurr"></param>
		/// <param name="amount"></param>
		/// <returns>The converted amount or decimal.MinValue in case of error</returns>
		public async Task<decimal> Convert(string srcCurr, string destCurr, decimal amount)
		{
			string response = await ExecuteMethodAsync(Token, PayGramHelper.CONVERT_METHOD, $"{PayGramHelper.TAG_AMOUNT}={amount}&{PayGramHelper.CURRENCY_SYMBOL_TOKEN_NAME}={srcCurr}&{PayGramHelper.CURRENCY_SYMBOL_DEST_TOKEN_NAME}={destCurr}", null);
			if (response == null)
				return decimal.MinValue;
			decimal result;
			if (decimal.TryParse(response, out result))
				return result;
			return decimal.MinValue;
		}
		public async Task<ResponseGetUpdates?> GetUpdatesAsync()
		{
			var response = await ExecuteMethodAsync(Token, PayGramHelper.UPDATES_METHOD, null, null);
			if (response == null)
				return new ResponseGetUpdates() { ResponseCode = ResponseCodes.ResponseGenericError };

			return JsonConvert.DeserializeObject<ResponseGetUpdates>(response);
		}
		/// <summary>
		/// Cancels all the pending callbacks of this client's user by marking them as Failed on the
		/// PayGram server: they will not be pushed again, so they are not picked up on the next pass.
		/// They remain retrievable through <see cref="GetUpdatesAsync"/> until they age out of the
		/// retention window.
		/// WARNING: use this only while debugging a callback endpoint (e.g. a misconfigured endpoint that
		/// PayGram cannot reach, which left updates piling up). It must NOT be used in production, because
		/// cancelled callbacks may carry transaction events (payments, deposits, withdrawals) that would
		/// otherwise be lost; any notification lost this way can still be recovered with a GetUpdatesAsync call.
		/// </summary>
		/// <returns>A <see cref="ResponseCancelPendingCallbacks"/> with the number of cancelled callbacks. Does not return null.</returns>
		public async Task<ResponseCancelPendingCallbacks?> CancelPendingCallbacksAsync()
		{
			var response = await ExecuteMethodAsync(Token, PayGramHelper.CANCEL_PENDING_CALLBACKS_METHOD, null, null);
			if (response == null)
				return new ResponseCancelPendingCallbacks() { ResponseCode = ResponseCodes.ResponseGenericError };

			return JsonConvert.DeserializeObject<ResponseCancelPendingCallbacks>(response);
		}
		public async Task<ResponseGetExchangeRates?> GetExchangeRatesAsync()
		{
			var response = await ExecuteMethodAsync(Token, PayGramHelper.EXCHANGE_RATES_METHOD, null, null);
			if (response == null)
				return new ResponseGetExchangeRates() { ResponseCode = ResponseCodes.ResponseGenericError };

			return JsonConvert.DeserializeObject<ResponseGetExchangeRates>(response);
		}
		/// <summary>
		/// Gets the information about the passed invoice
		/// </summary>
		/// <param name="invoiceId">The invoice id whose info is needed</param>
		/// <returns>A ResponseInvoiceInfo or ResponseInvoiceWithdrawInfo depending on the type of the invoice. Does not return null.</returns>
		public async Task<ResponseInvoiceInfo?> GetInvoiceInfo(Guid invoiceId)
		{
			var response = await ExecuteMethodAsync(Token, PayGramHelper.INVOICE_INFO_METHOD, $"{PayGramHelper.INVOICEID_TOKEN_NAME}={invoiceId}", null);
			if (string.IsNullOrWhiteSpace(response))
				return new ResponseInvoiceInfo() { ResponseCode = ResponseCodes.ResponseGenericError };

			var resp = JsonConvert.DeserializeObject<ResponseInvoiceInfo>(response);

			if (resp == null)
			{
				log.Warn($"{invoiceId} info returned null");
				return new ResponseInvoiceInfo() { ResponseCode = ResponseCodes.ResponseGenericError, Message = "Unexpected error" };
			}

			if (resp.Type == PaygramResponseTypes.ResponseInvoiceWithdrawInfo)
				return JsonConvert.DeserializeObject<ResponseInvoiceWithdrawInfo>(response);
			else
				return resp;
		}
		/// <summary>
		/// Swaps an amount from one currency to another for the authenticated user. Set
		/// <see cref="SwapRequest.Simulate"/> to true to only get a quote without executing.
		/// Requires a unique idempotency key to prevent duplicate swaps.
		/// </summary>
		/// <returns>A <see cref="ResponseSwap"/>. Does not return null.</returns>
		public async Task<ResponseSwap?> SwapAsync(SwapRequest req)
		{
			var response = await ExecuteMethodAsync(Token, PayGramHelper.SWAP_V2_METHOD, null, req);
			if (response == null)
				return new ResponseSwap() { ResponseCode = ResponseCodes.ResponseGenericError };

			return JsonConvert.DeserializeObject<ResponseSwap>(response);
		}
		/// <summary>
		/// Requests a cryptocurrency withdrawal for the authenticated user to an external address.
		/// Requires a unique idempotency key to prevent duplicate withdrawals.
		/// </summary>
		/// <returns>A <see cref="ResponseWithdrawAccepted"/>. Does not return null.</returns>
		public async Task<ResponseWithdrawAccepted?> WithdrawAsync(WithdrawRequest req)
		{
			var response = await ExecuteMethodAsync(Token, PayGramHelper.WITHDRAW_V2_METHOD, null, req);
			if (response == null)
				return new ResponseWithdrawAccepted() { ResponseCode = ResponseCodes.ResponseGenericError };

			return JsonConvert.DeserializeObject<ResponseWithdrawAccepted>(response);
		}
		/// <summary>
		/// Redeems a voucher into the authenticated user's balance.
		/// </summary>
		/// <returns>A <see cref="ResponseTopUpReceived"/>. Does not return null.</returns>
		public async Task<ResponseTopUpReceived?> RedeemVoucherAsync(RedeemVoucherRequest req)
		{
			var response = await ExecuteMethodAsync(Token, PayGramHelper.REDEEM_VOUCHER_V2_METHOD, null, req);
			if (response == null)
				return new ResponseTopUpReceived() { ResponseCode = ResponseCodes.ResponseGenericError };

			return JsonConvert.DeserializeObject<ResponseTopUpReceived>(response);
		}
		/// <summary>
		/// Pays (funds) a voucher from the authenticated user's balance.
		/// </summary>
		/// <returns>A <see cref="ResponseInvoiceInfo"/>. Does not return null.</returns>
		public async Task<ResponseInvoiceInfo?> PayVoucherAsync(PayVoucherRequest req)
		{
			var response = await ExecuteMethodAsync(Token, PayGramHelper.PAY_VOUCHER_V2_METHOD, null, req);
			if (response == null)
				return new ResponseInvoiceInfo() { ResponseCode = ResponseCodes.ResponseGenericError };

			return JsonConvert.DeserializeObject<ResponseInvoiceInfo>(response);
		}
		/// <summary>
		/// Retrieves the authenticated user's account statement (transaction history) for a time range, paged.
		/// </summary>
		/// <returns>A <see cref="ResponseStatement"/>. Does not return null.</returns>
		public async Task<ResponseStatement?> GetStatementAsync(GetStatementRequest req)
		{
			var response = await ExecuteMethodAsync(Token, PayGramHelper.GET_STATEMENT_V2_METHOD, null, req);
			if (response == null)
				return new ResponseStatement() { ResponseCode = ResponseCodes.ResponseGenericError };

			return JsonConvert.DeserializeObject<ResponseStatement>(response);
		}
		/// <summary>
		/// Sends a request to the PayGram bot server. Returns null in case of errors with the communication with the server.
		/// </summary>
		/// <param name="rq">the query string</param>
		/// <param name="method">the body request if any</param>
		/// <returns></returns>
		async static Task<string?> ExecuteMethodAsync(string token, string method, string otherParams, object? request)
		{
			if (otherParams == null)
				otherParams = "";
			if (otherParams != "" && otherParams.StartsWith("?") == false)
				otherParams = $"?{otherParams}";

			string url = string.Format(PayGramHelper.PAYGRAM_BOT_ENDPOINT, token, method, otherParams);

			string reqContent = request != null ? JsonConvert.SerializeObject(request) : "{}";

			//log.Debug($"Requesting: {url}, {reqContent}");

			using (HttpRequestMessage m = new(HttpMethod.Post, url))
			{
				using (HttpContent cont = new StringContent(reqContent, Encoding.UTF8, "application/json"))
				{
					m.Content = cont;
					try
					{
						using (HttpResponseMessage res = await _httpClient.SendAsync(m))
						{
							if (res.IsSuccessStatusCode == false)
							{
								log.Error($"httpError:{res.StatusCode}, url:{url}");
								return null;
							}

							string resContent = await res.Content.ReadAsStringAsync();

							return resContent;
						}
					}
					catch (Exception ex)
					{
						log.Error($"Error sending the request to the server. {url} {reqContent}", ex);
						return null;
					}
				}
			}
		}

	}
}

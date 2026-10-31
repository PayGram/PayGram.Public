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
		/// <summary>The base url of the users api, ending with a slash. Defaults to <see cref="PayGramHelper.PAYGRAM_BOT_API_URL"/>.</summary>
		public string ApiUrl { get; }

		/// <param name="token">The api token generated in the bot (Settings, Generate API Token)</param>
		/// <param name="apiUrl">The base url of the users api, null for the production server</param>
		public PayGramBotClient(string token, string? apiUrl = null)
		{
			Token = token;
			ApiUrl = string.IsNullOrWhiteSpace(apiUrl) ? PayGramHelper.PAYGRAM_BOT_API_URL : apiUrl.TrimEnd('/') + "/";
		}

		/// <summary>
		/// Sends the money directly from ones account to someone else's account
		/// </summary>
		/// <param name="toTid">The id of the person to send the funds to</param>
		/// <param name="amount">The amount to send</param>
		/// <param name="curSym">The currency symbol, any of the <see cref="CurrenciesLib.Currencies"/> .ToString()</param>
		/// <param name="unique">The unique code for this request</param>
		/// <returns>True if it succeded, false otherwise: also when its outcome could not be confirmed (it may have gone through),
		/// which a bool cannot tell. Use <see cref="TransferCreditAsync"/></returns>
		[Obsolete("Use TransferCreditAsync: its answer tells a failure (500, 554) from an outcome not confirmed (551, resend with the same key), which a bool cannot.")]
		public async Task<bool> TransferMoneyAsync(long toTid, decimal amount, Currencies curSym, string unique, string callbackData)
		{
			if (amount <= 0) return false;
			// through the V2 call: the key and the callback data travel in the JSON body, never cut by a # or a & of a query string
			// an empty or blank callback data reached PayGram as null through the query string: sent so, a request resent from an older
			// version is the same request
			var res = await TransferCreditAsync(new TransferCreditRequest { To = toTid, Amt = amount, CurSym = curSym, Unique = unique, Cd = string.IsNullOrWhiteSpace(callbackData) ? null : callbackData });
			if (res.ResponseCode == ResponseCodes.ResponseUniqueRequestViolation)
				log.Warn($"transfer {unique} of {amount} {curSym} to {toTid} not final (551), resend it with the same key, never a new one: {res.Message}");
			else if (res.ResponseCode == ResponseCodes.UniqueKeyReusedForOtherRequest)
				log.Warn($"transfer {unique} of {amount} {curSym} to {toTid} not executed (560): the key is already registered for a different request. Check that one with CheckUniqueAsync before sending a new key: {res.Message}");
			return res.Success;
		}
		public async Task<Guid> IssueInvoiceAsync(decimal amount, Currencies curSym, string callBackdata)
		{
			/*string token
		    , [FromQuery] decimal amt
		    , [FromQuery] Currencies cursym
		    , [FromQuery] string? callbackData*/

			if (amount <= 0) return Guid.Empty;
			var response = await ExecuteMethodAsync(Token, PayGramHelper.ISSUEINVOICE_METHOD, $"{PayGramHelper.CURRENCY_SYMBOL_TOKEN_NAME}={curSym}&{PayGramHelper.AMT_TOKEN_NAME}={amount.ToString(CultureInfo.InvariantCulture)}&{PayGramHelper.CALLBACKDATA_TOKEN_NAME}={Uri.EscapeDataString(callBackdata ?? "")}", null);
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
		/// Creates an invoice that other users pay to the authenticated user. Share <see cref="ResponseIssueInvoice.PayUrl"/>
		/// with the payer; when the invoice is paid a callback carrying <see cref="IssueInvoiceRequest.Cd"/> is sent.
		/// </summary>
		/// <returns>A <see cref="ResponseIssueInvoice"/>. Does not return null.</returns>
		public async Task<ResponseIssueInvoice> IssueInvoiceAsync(IssueInvoiceRequest req)
		{
			var response = await ExecuteMethodAsync(Token, PayGramHelper.ISSUEINVOICE_V2_METHOD, null, req);
			return Read(response, () => new ResponseIssueInvoice(ResponseCodes.ResponseGenericError));
		}
		/// <summary>
		/// Moves funds out of the authenticated user's balance. With <see cref="TransferCreditRequest.To"/> set the funds go
		/// straight to that user; left null, a voucher is created and <see cref="ResponseTransferCredit.RedeemUrl"/> is the
		/// link whoever receives it opens to redeem it (a cashout).
		/// Requires a unique idempotency key: retrying with the same key never transfers twice.
		/// </summary>
		/// <returns>A <see cref="ResponseTransferCredit"/>. Does not return null.</returns>
		public async Task<ResponseTransferCredit> TransferCreditAsync(TransferCreditRequest req)
			=> MoneyAnswer<ResponseTransferCredit>(await ExecuteAsync(Token, PayGramHelper.TRANSFER_V2_METHOD, null, req));

		/// <summary>
		/// The message of an operation that moves money (TransferCredit, Swap, Withdraw, PayVoucher, RedeemVoucher) when PayGram's answer did not
		/// arrive (a timeout, a connection error, a server or proxy error) or could not be read. It may have run, so it is not a
		/// failure: the answer is <see cref="ResponseCodes.ResponseUniqueRequestViolation"/> (551), as for a request still in
		/// progress. Resend the same request with the same unique key, never a new key, which could run it a second time.
		/// </summary>
		public const string NOT_CONFIRMED = "PayGram's answer did not arrive: the operation may have gone through. Resend the same request (with the same unique key, where it has one), never with a new key: a resend never runs it twice. A RedeemVoucher is resent too (it never redeems twice): if the resend fails, check with CheckUnique by the voucher code whether it was redeemed into your account (API reference 4.2), and credit a voucher code at most once.";

		/// <summary>
		/// The answer of an operation that moves money: what PayGram answered (a JSON with its responseCode); a failure of this attempt
		/// when PayGram refused the call before it ran (<see cref="REFUSED_BEFORE_RUNNING"/>: fix the call and send it again with
		/// the same key); otherwise (no answer, a 5xx, any other
		/// 4xx, which may come from a proxy on the resend of a call that ran, an answer that is not PayGram's) 551
		/// <see cref="NOT_CONFIRMED"/>
		/// </summary>
		static T MoneyAnswer<T>((string? Body, int? Status) answer) where T : PaygramResponse, new()
		{
			if (answer.Status is int status && REFUSED_BEFORE_RUNNING.Contains(status))
				return new T { ResponseCode = ResponseCodes.ResponseGenericError, Message = $"PayGram refused this call (HTTP {status}): this attempt was not executed: fix the call and send it again with the same key." };
			if (answer.Status is >= 200 and < 300 && string.IsNullOrWhiteSpace(answer.Body) == false)
			{
				try
				{
					// PayGram's answer carries its responseCode: without it (an empty object, a proxy's page) it says nothing
					if (Newtonsoft.Json.Linq.JObject.Parse(answer.Body).GetValue("responseCode", StringComparison.OrdinalIgnoreCase) != null
						&& JsonConvert.DeserializeObject<T>(answer.Body) is T read)
						return read;
				}
				catch (JsonException) { /* an error page, a cut answer: not read */ }
			}
			return new T { ResponseCode = ResponseCodes.ResponseUniqueRequestViolation, Message = NOT_CONFIRMED };
		}

		/// <summary>
		/// The HTTP statuses PayGram's API answers before a call runs (an invalid body, a wrong url or method, a body too large or
		/// of another type): this attempt was not executed. Any other status says nothing about it
		/// </summary>
		static readonly HashSet<int> REFUSED_BEFORE_RUNNING = new() { 400, 404, 405, 413, 415 };

		/// <summary>
		/// The answer of a call that moves no money: what PayGram answered (a JSON with its responseCode), or <paramref name="failed"/>
		/// when no answer came, it could not be read or it is not PayGram's (never thrown)
		/// </summary>
		static T Read<T>(string? response, Func<T> failed, params JsonConverter[] converters) where T : class?
		{
			if (string.IsNullOrWhiteSpace(response)) return failed();
			try
			{
				if (Newtonsoft.Json.Linq.JObject.Parse(response).GetValue("responseCode", StringComparison.OrdinalIgnoreCase) == null) return failed();
				return JsonConvert.DeserializeObject<T>(response, converters) ?? failed();
			}
			catch (JsonException) { return failed(); }
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
			var response = await ExecuteMethodAsync(Token, PayGramHelper.DEPOSIT_METHOD, $"{PayGramHelper.CURRENCY_SYMBOL_TOKEN_NAME}={amountCurrency}&{PayGramHelper.NETWORK_SYMBOL_TOKEN_NAME}={network}&{PayGramHelper.AMT_TOKEN_NAME}={amount.ToString(CultureInfo.InvariantCulture)}&{PayGramHelper.CALLBACKDATA_TOKEN_NAME}={Uri.EscapeDataString(callbackData ?? "")}", null);
			log.Debug($"DepositMoneyAsync {amount} {amountCurrency}, response: {response}");
			var res = Read(response, () => new ResponseTopUp() { ResponseCode = ResponseCodes.ResponseGenericError, Message = "No answer that could be read" });
			// the crypto deposit carries its address: read again into its own type
			return res.Type == PaygramResponseTypes.ResponseTopUpCryptapi ? Read<ResponseTopUpCryptapi?>(response, () => null) ?? res : res;
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
			string response = await ExecuteMethodAsync(Token, PayGramHelper.CONVERT_METHOD, $"{PayGramHelper.AMT_TOKEN_NAME}={amount.ToString(CultureInfo.InvariantCulture)}&{PayGramHelper.CURRENCY_SYMBOL_TOKEN_NAME}={srcCurr}&{PayGramHelper.CURRENCY_SYMBOL_DEST_TOKEN_NAME}={destCurr}", null);
			if (response == null)
				return decimal.MinValue;
			// the server answers with a ResponseConvert, not with a bare number
			var res = Read<ResponseConvert?>(response, () => null);
			return res?.Success == true ? res.Result : decimal.MinValue;
		}
		public async Task<ResponseGetUpdates?> GetUpdatesAsync()
		{
			var response = await ExecuteMethodAsync(Token, PayGramHelper.UPDATES_METHOD, null, null);
			return Read(response, () => new ResponseGetUpdates() { ResponseCode = ResponseCodes.ResponseGenericError });
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
			return Read(response, () => new ResponseCancelPendingCallbacks() { ResponseCode = ResponseCodes.ResponseGenericError });
		}
		public async Task<ResponseGetExchangeRates?> GetExchangeRatesAsync()
		{
			var response = await ExecuteMethodAsync(Token, PayGramHelper.EXCHANGE_RATES_METHOD, null, null);
			return Read(response, () => new ResponseGetExchangeRates() { ResponseCode = ResponseCodes.ResponseGenericError });
		}
		/// <summary>
		/// Gets the information about the passed invoice
		/// </summary>
		/// <param name="invoiceId">The invoice id whose info is needed</param>
		/// <returns>A ResponseInvoiceInfo, a ResponseInvoiceWithdrawInfo or a ResponseRedEnvelopeInvoiceInfo depending on the type of the invoice. Does not return null.</returns>
		public async Task<ResponseInvoiceInfo?> GetInvoiceInfo(Guid invoiceId)
		{
			var response = await ExecuteMethodAsync(Token, PayGramHelper.INVOICE_INFO_METHOD, $"{PayGramHelper.INVOICEID_TOKEN_NAME}={invoiceId}", null);
			// read into its real type: a withdrawal keeps its method, a red envelope its redeemers
			return Read(response, () => new ResponseInvoiceInfo() { ResponseCode = ResponseCodes.ResponseGenericError, Message = "No answer that could be read" }, new InvoiceInfoConverter());
		}
		/// <summary>
		/// Swaps an amount from one currency to another for the authenticated user. Set
		/// <see cref="SwapRequest.Simulate"/> to true to only get a quote without executing.
		/// Requires a unique idempotency key to prevent duplicate swaps.
		/// </summary>
		/// <returns>A <see cref="ResponseSwap"/>. Does not return null.</returns>
		public async Task<ResponseSwap?> SwapAsync(SwapRequest req)
		{
			var answer = await ExecuteAsync(Token, PayGramHelper.SWAP_V2_METHOD, null, req);
			// a quote moves nothing: no answer is a plain failure
			if (req.Simulate)
				return Read(answer.Status is >= 200 and < 300 ? answer.Body : null, () => new ResponseSwap() { ResponseCode = ResponseCodes.ResponseGenericError });
			return MoneyAnswer<ResponseSwap>(answer);
		}
		/// <summary>
		/// Requests a cryptocurrency withdrawal for the authenticated user to an external address.
		/// Requires a unique idempotency key to prevent duplicate withdrawals.
		/// </summary>
		/// <returns>A <see cref="ResponseWithdrawAccepted"/>. Does not return null.</returns>
		public async Task<ResponseWithdrawAccepted?> WithdrawAsync(WithdrawRequest req)
			=> MoneyAnswer<ResponseWithdrawAccepted>(await ExecuteAsync(Token, PayGramHelper.WITHDRAW_V2_METHOD, null, req));
		/// <summary>
		/// Redeems a voucher into the authenticated user's balance. It has no key, but a resend never redeems twice: after a 551
		/// resend it; if the resend fails, check with <see cref="CheckUniqueAsync"/> by the voucher code whether it was redeemed
		/// into your account, and credit a voucher code at most once (see the API reference, 4.2).
		/// </summary>
		/// <returns>A <see cref="ResponseTopUpReceived"/>. Does not return null.</returns>
		public async Task<ResponseTopUpReceived?> RedeemVoucherAsync(RedeemVoucherRequest req)
		{
			// it moves money: an answer lost is not confirmed (resend it), never a plain failure
			return MoneyAnswer<ResponseTopUpReceived>(await ExecuteAsync(Token, PayGramHelper.REDEEM_VOUCHER_V2_METHOD, null, req));
		}
		/// <summary>
		/// Looks a reference up without executing anything: an invoice id, a voucher code, your callback data or your unique key
		/// (see <see cref="CheckedUniqueRequest.Status"/>). It is not a step of a retry: after a 500, a 551 or no answer, resend the
		/// same request with the same key, never a new key. Use it when you no longer want the operation (an order cancelled) and
		/// only want to know whether it went through, when you lost the request data and cannot resend it identically, or for an
		/// operation sent before version 2.6 (look it up by invoice id or callback data: its key is not recognised) or whose answer
		/// was lost with an SDK before 3.3. Whatever it says, resending the same request with the same key stays safe: it runs at
		/// most once. Unsettled: its money moved and it cannot be completed, never send it again, contact support.
		/// </summary>
		/// <returns>A <see cref="ResponseCheckUnique"/>. Does not return null.</returns>
		public async Task<ResponseCheckUnique> CheckUniqueAsync(CheckUniqueRequest req)
		{
			var response = await ExecuteMethodAsync(Token, PayGramHelper.CHECK_UNIQUE_METHOD, null, req);
			return Read(response, () => new ResponseCheckUnique("PayGram did not answer, or its answer could not be read: retry", ResponseCodes.ResponseGenericError));
		}
		/// <summary>
		/// Pays (funds) a voucher from the authenticated user's balance.
		/// </summary>
		/// <returns>A <see cref="ResponseInvoiceInfo"/>. Does not return null.</returns>
		public async Task<ResponseInvoiceInfo?> PayVoucherAsync(PayVoucherRequest req)
			=> MoneyAnswer<ResponseInvoiceInfo>(await ExecuteAsync(Token, PayGramHelper.PAY_VOUCHER_V2_METHOD, null, req));
		/// <summary>
		/// Retrieves the authenticated user's account statement (transaction history) for a time range, paged.
		/// </summary>
		/// <returns>A <see cref="ResponseStatement"/>. Does not return null.</returns>
		public async Task<ResponseStatement?> GetStatementAsync(GetStatementRequest req)
		{
			var response = await ExecuteMethodAsync(Token, PayGramHelper.GET_STATEMENT_V2_METHOD, null, req);
			return Read(response, () => new ResponseStatement() { ResponseCode = ResponseCodes.ResponseGenericError });
		}
		/// <summary>
		/// Sends a request to the PayGram bot server. Returns null in case of errors with the communication with the server.
		/// </summary>
		async Task<string?> ExecuteMethodAsync(string token, string method, string? otherParams, object? request)
		{
			var answer = await ExecuteAsync(token, method, otherParams, request);
			return answer.Status is >= 200 and < 300 ? answer.Body : null;
		}

		/// <summary>
		/// Sends a request to the PayGram bot server: the body of its answer and its HTTP status; the status is null when no
		/// answer came (a timeout, a connection error)
		/// </summary>
		async Task<(string? Body, int? Status)> ExecuteAsync(string token, string method, string? otherParams, object? request)
		{
			if (otherParams == null)
				otherParams = "";
			if (otherParams != "" && otherParams.StartsWith("?") == false)
				otherParams = $"?{otherParams}";

			string url = $"{ApiUrl}{token}/{method}/{otherParams}";
			// the API token moves funds: never written in a log
			// the token is masked like in the PayGram logs: its last 4 characters
			string logUrl = $"{ApiUrl}{(string.IsNullOrEmpty(token) ? "-" : token.Length < 12 ? "…" : "…" + token[^4..])}/{method}/{otherParams}";

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
								log.Error($"httpError:{res.StatusCode}, url:{logUrl}");
								return (null, (int)res.StatusCode);
							}

							string resContent = await res.Content.ReadAsStringAsync();

							return (resContent, (int)res.StatusCode);
						}
					}
					catch (Exception ex)
					{
						log.Error($"Error sending the request to the server. {logUrl} {reqContent}", ex);
						return (null, null);
					}
				}
			}
		}

	}
}

using System.Collections.Concurrent;
using CurrenciesLib;
using Newtonsoft.Json;
using PayGram.Public;
using PayGram.Public.Client;
using PayGram.Public.Requests;
using PayGram.Public.UserAPI;

// PayGram sample: create invoices, issue vouchers (cashout) and receive the payment callbacks.
// Fill PayGram:ApiToken and PayGram:SignSeed in appsettings.json, then `dotnet run`.

var builder = WebApplication.CreateBuilder(args);
var settings = builder.Configuration.GetSection("PayGram").Get<PayGramSettings>() ?? new PayGramSettings();
if (string.IsNullOrWhiteSpace(settings.ApiToken) || string.IsNullOrWhiteSpace(settings.SignSeed))
{
	Console.Error.WriteLine("Set PayGram:ApiToken and PayGram:SignSeed in appsettings.json (see the README).");
	return 1;
}

var paygram = new PayGramBotClient(settings.ApiToken, settings.ApiUrl);
// the seed is a guid and the server signs with its lowercase text
string signSeed = settings.SignSeed.Trim().ToLowerInvariant();

// ids of the callbacks already handled. Delivery is at-least-once: a real app keeps them in its database
var handledCallbacks = new ConcurrentDictionary<int, bool>();

var app = builder.Build();
var log = app.Logger;

app.MapGet("/", () => Results.Text("""
	PayGram sample is running.
	  POST /invoices          {"amount": 1.5, "currency": "USDT"}   create an invoice, returns the link to pay it
	  POST /cashouts          {"amount": 1.5, "currency": "USDT", "id": "your-cashout-id"}   issue a voucher, returns the link to redeem it
	  POST /paygram/callback  the url to set in the bot (Settings > Set Callback URL), through a public https tunnel
	"""));

// 1. Create an invoice: the customer opens PayUrl in Telegram and pays it.
app.MapPost("/invoices", async (CreateRequest req) =>
{
	if (Enum.TryParse(req.Currency, true, out Currencies currency) == false)
		return Results.BadRequest($"Unknown currency {req.Currency}");

	// your order id: it comes back in the callback as CallbackData, so you know which order was paid
	string orderId = $"order-{Guid.NewGuid():N}";
	var res = await paygram.IssueInvoiceAsync(new IssueInvoiceRequest { Amt = req.Amount, CurSym = currency, Cd = orderId });
	if (res.Success == false)
		return Results.Problem($"{res.ResponseCode}: {res.Message}", statusCode: StatusCodes.Status502BadGateway);

	log.LogInformation("Invoice {InvoiceCode} created for {OrderId}: {Amount} {Currency}", res.InvoiceCode, orderId, res.Amount, res.CurrencyCode);
	return Results.Ok(new { orderId, res.InvoiceCode, res.PayUrl });
});

// 2. Cashout: a voucher is funded from your balance; whoever opens RedeemUrl receives the money.
app.MapPost("/cashouts", async (CreateRequest req) =>
{
	if (Enum.TryParse(req.Currency, true, out Currencies currency) == false)
		return Results.BadRequest($"Unknown currency {req.Currency}");

	// your id of this cashout, created once and stored with it (here sent by the caller): the same id is the same key on every
	// retry of this cashout, a new one is a new cashout. Required: made up here, a retry would be a second cashout
	if (string.IsNullOrWhiteSpace(req.Id))
		return Results.BadRequest("id is required: your id of this cashout, the same on every retry of it");
	string cashoutId = req.Id;
	var res = await paygram.TransferCreditAsync(new TransferCreditRequest
	{
		To = null,          // no recipient: a voucher is created
		Amt = req.Amount,
		CurSym = currency,
		// idempotency key: if the response is lost, retry with the SAME key and the voucher is never funded twice
		Unique = cashoutId,
		Cd = cashoutId,
	});
	// 560: this id was already used for a different cashout (another amount or currency). This call did nothing; the first
	// cashout with this id may have gone through: look it up (CheckUniqueAsync) before using a new id, never resend this one
	if (res.ResponseCode == ResponseCodes.UniqueKeyReusedForOtherRequest)
		return Results.Conflict(new { cashoutId, error = res.Message });
	// 551: not final (the answer was lost, or it is still running): it may have gone through, retry with the same id, never a new one
	if (res.ResponseCode == ResponseCodes.ResponseUniqueRequestViolation)
		return Results.Accepted(value: new { cashoutId, status = "not final (551): retry with the same id; if it keeps coming for minutes, contact support, never a new id", res.Message });
	// any other failure: retry with the same id once the cause is fixed, never a new one; a 500 replayed (ReplayedResponse true)
	// is final: do not resend it, contact support
	if (res.Success == false)
		return Results.Problem($"{res.ResponseCode}: {res.Message} (retry with the same id, never a new one; if replayedResponse is true it is final: contact support)", statusCode: StatusCodes.Status502BadGateway);

	log.LogInformation("Voucher {Voucher} issued: {Amount} {Currency}, balance left {Balance}", res.FriendlyVoucherCode, res.Amount, currency, res.FromCredit);
	return Results.Ok(new { cashoutId, res.VoucherCode, res.FriendlyVoucherCode, res.RedeemUrl });
});

// 3. Callbacks: PayGram POSTs every event of your account here.
app.MapPost("/paygram/callback", async (HttpRequest http) =>
{
	// the signature covers the exact bytes received: read the raw body before parsing it
	using var reader = new StreamReader(http.Body);
	string body = await reader.ReadToEndAsync();

	if (CallbackSignature.Validate(signSeed, http.Headers[CallbackSignature.TIMESTAMP_HEADER], http.Headers[CallbackSignature.SIGNATURE_HEADER], body) == false)
	{
		log.LogWarning("Callback rejected: invalid signature");
		return Results.Unauthorized();
	}

	var cb = JsonConvert.DeserializeObject<UserCallbackInfo>(body);
	if (cb == null)
		return Results.BadRequest();

	// a 2xx tells PayGram the callback was delivered; a duplicate is acknowledged without being processed again
	if (handledCallbacks.TryAdd(cb.Id, true) == false)
		return Results.Ok();

	switch (cb.CallbackType)
	{
		case UserCallbackTypes.InvoiceInfoCredited:
			var paid = cb.InvoiceInfo;
			log.LogInformation("PAID: invoice {InvoiceCode} of {OrderId}: received {Amount} {Currency} (fees {Fees}), balance {Balance}",
				paid.InvoiceCode, cb.CallbackData, paid.TransactionAmount, paid.Currency, paid.Fees, paid.Balance);
			// here: mark the order cb.CallbackData as paid
			break;
		case UserCallbackTypes.MoneySent:
			var sent = cb.MoneySentInfo;
			log.LogInformation("Voucher {Voucher} of {CashoutId} funded: {Amount} {Currency}, balance {Balance}",
				sent.FriendlyVoucherCode, cb.CallbackData, sent.TransactionAmount, sent.Currency, sent.Balance);
			break;
		case UserCallbackTypes.InvoiceInfoDebited:
			log.LogInformation("Debited: invoice {InvoiceCode} ({CallbackData}), {Amount} {Currency}",
				cb.InvoiceInfo.InvoiceCode, cb.CallbackData, cb.InvoiceInfo.TransactionAmount, cb.InvoiceInfo.Currency);
			break;
		default:
			log.LogInformation("Callback {Id} of type {Type}: {Body}", cb.Id, cb.CallbackType, body);
			break;
	}
	return Results.Ok();
});

app.Run();
return 0;

record CreateRequest(decimal Amount, string Currency = "USDT", string? Id = null);

class PayGramSettings
{
	public string ApiToken { get; set; } = "";
	public string SignSeed { get; set; } = "";
	public string? ApiUrl { get; set; }
}

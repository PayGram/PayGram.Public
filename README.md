# PayGram.Public

The .NET library for the **PayGram users API**, the HTTP API behind the PayGram Telegram bot. You can use it to:

- **get paid**: create invoices your customers pay from Telegram;
- **pay out**: issue vouchers (cashout) or transfer funds to a PayGram user;
- **be notified**: receive signed callbacks for every event on your account.

It also has deposits, withdrawals, swaps, statements and exchange rates.

The library targets `net10.0`. You don't need .NET to use the API: everything below is plain HTTPS + JSON. The [Node.js sample](samples/nodejs) shows how to call it with no library at all.

- [1. Get your keys](#1-get-your-keys)
- [2. Install](#2-install)
- [3. Quick start](#3-quick-start)
- [4. Responses, errors and idempotency](#4-responses-errors-and-idempotency)
- [5. Receiving updates: use the webhook, not polling](#5-receiving-updates-use-the-webhook-not-polling)
- [6. Verifying the callback signature](#6-verifying-the-callback-signature)
- [7. Sample projects](#7-sample-projects)
- [8. API reference](#8-api-reference)

---

## 1. Get your keys

You manage everything from the PayGram bot in Telegram, under **Settings**:

| What | Where | Used for |
|---|---|---|
| **API Token** | Settings → *Generate API Token* | Authenticates every API call. It is part of the request URL. |
| **Sign Seed** | Shown with the API token, and in *Set Callback URL* | The key that signs the callbacks PayGram sends you. |
| **Callback URL** | Settings → *Set Callback URL* | Your HTTPS endpoint. PayGram POSTs your account's events to it. |

> ⚠️ **The API token can move the funds of your account.** Keep it on your server only. Never put it in a web page, a mobile app or a git repository. If it leaks, generate a new one from the bot.

## 2. Install

The library isn't on NuGet yet. Clone it next to [CurrenciesLib](https://github.com/PayGram/CurrenciesLib), which it references as a sibling folder:

```bash
git clone https://github.com/PayGram/CurrenciesLib.git
git clone https://github.com/PayGram/PayGram.Public.git
```

Then reference the project from yours:

```xml
<ItemGroup>
  <ProjectReference Include="..\PayGram.Public\PayGram.Public.csproj" />
</ItemGroup>
```

## 3. Quick start

```csharp
using CurrenciesLib;
using PayGram.Public.Client;
using PayGram.Public.Requests;
using PayGram.Public.UserAPI;

var paygram = new PayGramBotClient("YOUR-API-TOKEN"); // production by default

// Get paid: create an invoice and send PayUrl to your customer
var invoice = await paygram.IssueInvoiceAsync(new IssueInvoiceRequest
{
    Amt = 10m,
    CurSym = Currencies.USDT,
    Cd = "order-1234",          // your reference, returned in the payment callback
});
if (invoice.Success)
    Console.WriteLine($"Pay here: {invoice.PayUrl}");

// Pay out: issue a voucher (cashout) and give RedeemUrl to the person to pay
var voucher = await paygram.TransferCreditAsync(new TransferCreditRequest
{
    To = null,                  // no recipient = create a voucher
    Amt = 5m,
    CurSym = Currencies.USDT,
    Unique = "cashout-5678",    // idempotency key, see below
    Cd = "cashout-5678",
});
if (voucher.Success)
    Console.WriteLine($"Redeem here: {voucher.RedeemUrl} (code {voucher.FriendlyVoucherCode})");
```

- **Invoice flow.** Your customer opens `PayUrl` in Telegram and pays. A payment request **expires 30 minutes after its creation**: after that it can no longer be paid, so create a new one for a customer who comes back later. Your callback URL then receives an `InvoiceInfoCredited` callback, whose `callbackData` is your `Cd`. **Fulfil the order on that callback**, not when the customer comes back to your site.
- **Voucher flow.** The amount leaves your balance as soon as the voucher is created; you receive a `MoneySent` callback. Whoever opens `RedeemUrl` gets the funds on their PayGram account.

## 4. Responses, errors and idempotency

Every response has the same envelope:

| Field | Meaning |
|---|---|
| `ResponseCode` | `200` means OK. Any value **≥ 400** is an error (`Success` is `false`). |
| `Message` | A human-readable explanation of the error. |
| `ReplayedResponse` | `true` when the response is the stored result of an earlier request with the same idempotency key. |

| Code | Name | What to do |
|---|---|---|
| 200 | `ResponseOK` | Done. |
| 500 | `ResponseGenericError` | Read `Message` (invalid token, invalid parameters…). |
| 550 | `ResponseServerUpdating` | The server is being updated. Retry in a minute. |
| 551 | `ResponseUniqueRequestViolation` | That idempotency key was already used, or is still being processed. |
| 552 | `AuthError` | Your token isn't allowed to do this. |
| 553 | `NotFound` | The user or invoice doesn't exist. |
| 554 | `NotEnoughFunds` | Your balance is too low. |

The library methods never return `null` for a communication error: they return a response with `ResponseGenericError`. Always check `Success`.

**Idempotency.** Every call that moves money (`TransferCredit`, `Swap`, `Withdraw`) takes a `Unique` key (keep it under 50 ASCII characters). Generate the key **before** the call and store it with your operation. If the call times out or you lose the response, retry with the **same** key: the operation runs at most once.

**Amounts** are decimals. In JavaScript, don't use floating-point arithmetic on balances; use integer minor units or a decimal library.

## 5. Receiving updates: use the webhook, not polling

There are two ways to learn that something happened on your account, for example an invoice was paid, a voucher was redeemed or a deposit arrived:

1. **Webhook (recommended):** set a **Callback URL** in the bot, and PayGram POSTs each event to it as it happens.
2. **Polling:** call `GetUpdatesAsync()` periodically, and PayGram returns the events not delivered yet.

**Use the webhook.** Polling is only a fallback, for example while you can't expose an HTTPS endpoint yet.

| | Webhook | Polling |
|---|---|---|
| Latency | Immediate | Up to your polling interval |
| Requests | One per event | Constant, even when nothing happens |
| Rate limit | Doesn't count against your API quota | Uses your quota: currently 300 requests/minute per token, shared with your real calls |
| Missed events | PayGram retries for you (see below) | Events are marked delivered as soon as they are returned, so a response lost in transit can lose them |
| Authenticity | HMAC-signed by PayGram | Only as trusted as your API call |

Push and polling read **the same queue**: an event delivered by one isn't returned by the other. Pick one mechanism, and don't poll while your webhook is working.

### How callbacks are delivered

- **Request format.** Each event is an HTTPS `POST` with a JSON body. Field names are camelCase and enums are numbers. The request carries the signature headers described in [section 6](#6-verifying-the-callback-signature).
- **Acknowledge with a 2xx.** Any 2xx status marks the event as delivered. Anything else, a timeout or a connection error makes PayGram **retry every 5 minutes for up to 15 days**.
- **Events arrive in order.** While an event of your account is failing, the later ones wait behind it. A broken endpoint delays all your notifications, so monitor it.
- **Delivery is at least once.** The same event can arrive twice, for example when your 200 was lost. Every callback has an `id`: store the ids you've processed and ignore duplicates. Still answer them with 200.
- **Answer quickly.** Verify the signature, store the event, reply 200, then do the slow work, such as emails or fulfilment, in the background.

`CancelPendingCallbacksAsync()` drops every pending push, for example after a misconfigured URL piled up a backlog. It's meant for development: in production you would lose payment notifications.

### Callback types

The body carries the type in `callbackType` (a number; `UserCallbackInfo.CallbackType` in C#). It is also given by which object is not null:

| Type | Object | When |
|---|---|---|
| `InvoiceInfoCredited` (2) | `invoiceInfo`, with `transactionAmount > 0` | Money came in on an invoice: one of your invoices was paid. `callbackData` is the `Cd` of your invoice. |
| `InvoiceInfoDebited` (5) | `invoiceInfo`, with `transactionAmount ≤ 0` | Money left on an invoice, for example you paid an invoice. |
| `MoneySent` (4) | `moneySentInfo` | You sent a transfer or funded a voucher. Includes `voucherCode` and `redeemUrl`. |
| `BalanceInfo` (0) | `balanceInfo` | Your balance changed for another reason. |
| `WithdrawInfo` (1) | `withdrawInfo` | A withdrawal changed status. |
| `ReportReady` (6) | `reportInfo` | A statement export you booked is ready. |

Example body:

```json
{
  "id": 918273,
  "dateUtc": "2026-10-18T12:00:00Z",
  "timestamp": 1792324800,
  "callbackData": "order-1234",
  "userCliId": "123456789",
  "invoiceInfo": {
    "invoiceCode": "8a6f0d2e-5b1c-4c1e-9d55-0b6e0a1f2c3d",
    "friendlyVoucherCode": null,
    "transactionAmount": 9.9,
    "fees": 0.1,
    "currency": 5,
    "balance": 152.4
  },
  "balanceInfo": null,
  "moneySentInfo": null,
  "withdrawInfo": null,
  "reportInfo": null,
  "callbackType": 2,
  "hash": "…deprecated, ignore…"
}
```

`currency` is the `Currencies` enum number: 1 BTC, 2 ETH, 5 USDT, 11 PHPT, 16 BNB, 17 TRX, 18 USDC… (see `CurrenciesLib`).

## 6. Verifying the callback signature

**Always** verify the signature before trusting a callback: your callback URL is public, and anyone can POST to it.

PayGram adds two headers:

| Header | Value |
|---|---|
| `X-PayGram-Timestamp` | Unix time, in seconds, when the request was sent |
| `X-PayGram-Signature` | `HEX( HMAC-SHA256( key = Sign Seed, message = "{timestamp}.{raw body}" ) )`, in uppercase |

The rules:

- **Sign the raw body.** Compute the HMAC over the exact bytes you received, **before** parsing the JSON. Re-serializing changes the bytes and breaks the check.
- **The key is your Sign Seed as text, in lowercase** (it's a GUID).
- **Reject old requests.** Refuse a timestamp more than **600 seconds** away from your clock, to block replays. Keep your server clock synced (NTP).
- **Compare in constant time.**

In C# all of this is one call:

```csharp
// ASP.NET Core minimal API
app.MapPost("/paygram/callback", async (HttpRequest http) =>
{
    using var reader = new StreamReader(http.Body);
    string body = await reader.ReadToEndAsync();

    if (!CallbackSignature.Validate(signSeed.ToLowerInvariant(),
            http.Headers[CallbackSignature.TIMESTAMP_HEADER],
            http.Headers[CallbackSignature.SIGNATURE_HEADER],
            body))
        return Results.Unauthorized();

    var cb = JsonConvert.DeserializeObject<UserCallbackInfo>(body);
    // ... dedupe on cb.Id, store, then reply
    return Results.Ok();
});
```

In Node.js:

```js
import crypto from "node:crypto";

function verify(signSeed, headers, rawBody /* Buffer */) {
  const ts = headers["x-paygram-timestamp"], sig = headers["x-paygram-signature"];
  if (!ts || !sig || Math.abs(Date.now() / 1000 - Number(ts)) > 600) return false;
  const expected = crypto.createHmac("sha256", signSeed.toLowerCase())
    .update(`${ts}.`).update(rawBody).digest("hex").toUpperCase();
  return expected.length === sig.length &&
    crypto.timingSafeEqual(Buffer.from(expected), Buffer.from(sig.toUpperCase()));
}
```

The `hash` field in the body is **deprecated**. It doesn't cover the payload and can be replayed, so use the headers.

## 7. Sample projects

Two ready-to-run samples do the same three things:

- `POST /invoices` creates an invoice and returns its `payUrl`.
- `POST /cashouts` issues a voucher and returns its `redeemUrl`.
- `POST /paygram/callback` receives the callbacks: it verifies the signature, ignores duplicates and logs payments.

| | Folder | Requires | Configure | Run |
|---|---|---|---|---|
| C# | [`samples/csharp/PayGram.Sample`](samples/csharp/PayGram.Sample) | .NET 10 SDK, and CurrenciesLib cloned next to this repo ([section 2](#2-install)) | `appsettings.json` → `PayGram:ApiToken`, `PayGram:SignSeed` | `dotnet run` (listens on http://localhost:5080) |
| Node.js | [`samples/nodejs`](samples/nodejs) | Node 18+, no npm packages | `config.json` → `apiToken`, `signSeed` | `npm start` (listens on http://localhost:5081) |

Instead of editing the files, you can pass the keys as environment variables:
- C#: `PayGram__ApiToken`, `PayGram__SignSeed`.
- Node: `PAYGRAM_API_TOKEN`, `PAYGRAM_SIGN_SEED`.

This keeps them out of git. **Don't commit real keys.**

### Try it

```bash
# create an invoice of 1 USDT, open the returned payUrl in Telegram and pay it
curl -X POST http://localhost:5080/invoices -H "Content-Type: application/json" -d '{"amount": 1, "currency": "USDT"}'

# issue a 1 USDT voucher, open redeemUrl from another Telegram account to redeem it
curl -X POST http://localhost:5080/cashouts -H "Content-Type: application/json" -d '{"amount": 1, "currency": "USDT"}'
```

(Use port 5081 for the Node sample. In PowerShell, call `curl.exe` and escape the inner quotes, or use `Invoke-RestMethod`.)

### Receiving the callbacks on your PC

PayGram must reach your endpoint over the internet. While developing, expose the local port through an HTTPS tunnel:

```bash
cloudflared tunnel --url http://localhost:5080
# or
ngrok http 5080
```

Then, in the bot, go to **Settings → Set Callback URL** and enter `https://<your-tunnel-host>/paygram/callback`. Pay one of your invoices: the sample prints a `PAID:` line with your order id.

In production, use your own HTTPS domain. If the endpoint was unreachable for a while, you don't have to do anything: PayGram keeps retrying, as described in [section 5](#5-receiving-updates-use-the-webhook-not-polling).

## 8. API reference

The production base URL is `https://pubapi.paygr.am/PayGramUsers/`, and every call is `POST {base}{apiToken}/{Method}` with a JSON body. Request field names are case-insensitive. Enums can be sent as names (`"USDT"`) or numbers. Responses are camelCase, with enums as numbers.

| `PayGramBotClient` method | API method | Body | Returns |
|---|---|---|---|
| `IssueInvoiceAsync(IssueInvoiceRequest)` | `IssueInvoiceV2` | `amt`, `curSym`, `cd` | `ResponseIssueInvoice`, with `payUrl` |
| `TransferCreditAsync(TransferCreditRequest)` | `TransferCreditV2` | `to` (Telegram id, or null for a voucher), `amt`, `curSym`, `unique`, `cd` | `ResponseTransferCredit`, with `redeemUrl` |
| `GetInvoiceInfo(Guid)` | `InvoiceInfo` | query `invguid` | `ResponseInvoiceInfo` |
| `PayVoucherAsync(PayVoucherRequest)` | `PayVoucherV2` | | `ResponseInvoiceInfo` |
| `RedeemVoucherAsync(RedeemVoucherRequest)` | `RedeemVoucherV2` | | `ResponseTopUpReceived` |
| `DepositCreditAsync(...)` | `DepositCredit` | query `amt`, `cursym`, `network`, `cd` | `ResponseTopUp` (crypto deposit address) |
| `WithdrawAsync(WithdrawRequest)` | `WithdrawV2` | | `ResponseWithdrawAccepted` |
| `SwapAsync(SwapRequest)` | `SwapV2` | | `ResponseSwap` |
| `GetStatementAsync(GetStatementRequest)` | `GetStatementV2` | | `ResponseStatement` (each entry carries `balanceAfter`) |
| `GetExchangeRatesAsync()` | `GetExchangeRates` | | `ResponseGetExchangeRates` |
| `Convert(src, dest, amount)` | `Convert` | | the converted amount (a quote: nothing is moved) |
| `GetUpdatesAsync()` | `GetUpdates` | | `ResponseGetUpdates` (polling fallback, see [section 5](#5-receiving-updates-use-the-webhook-not-polling)) |
| `CancelPendingCallbacksAsync()` | `CancelPendingCallbacks` | | development only |

`CurrenciesManager` is a hosted service that keeps the `CurrenciesLib` conversion cache fed with PayGram's rates:

```csharp
services.AddSingleton(new PayGramBotClient(apiToken));
services.AddHostedService<CurrenciesManager>();
```

To point the client at another server, for example a test environment, pass its base URL: `new PayGramBotClient(apiToken, "https://test.example/PayGramUsers/")`.

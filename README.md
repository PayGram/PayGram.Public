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
using PayGram.Public;
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
else if (voucher.ResponseCode == ResponseCodes.ResponseUniqueRequestViolation)
    // not final: resend it with the same Unique; only "already registered for a different request" needs a new key
    Console.WriteLine($"Not final (551): {voucher.Message}");
```

- **Invoice flow.** Your customer opens `PayUrl` in Telegram and pays. A payment request **expires 30 minutes after its creation** (`ExpiresUtc` in the response): after that it is `Expired`, a payment is refused with `ResponseCodes.InvoiceExpired` (555) and you receive an `InvoiceExpired` callback, so create a new one for a customer who comes back later. Your callback URL then receives an `InvoiceInfoCredited` callback, whose `callbackData` is your `Cd`. **Fulfil the order on that callback**, not when the customer comes back to your site.
- **Voucher flow.** The amount leaves your balance as soon as the voucher is created; you receive a `MoneySent` callback. Whoever opens `RedeemUrl` gets the funds on their PayGram account.

## 4. Responses, errors and idempotency

Every response has the same envelope:

| Field | Meaning |
|---|---|
| `ResponseCode` | `200` means OK. Any value **≥ 400** is an error (`Success` is `false`). |
| `Message` | A human-readable explanation of the error. |
| `ReplayedResponse` | `true` when the response is the stored result of an earlier request with the same idempotency key, or a final failure closed by the server (see below). |

| Code | Name | What to do |
|---|---|---|
| 200 | `ResponseOK` | Done. |
| 500 | `ResponseGenericError` | Read `Message` (invalid token, invalid parameters…). For an operation with an idempotency key, resend the same request with the same key, never a new key (see **Idempotency** below). With `ReplayedResponse` true it is final: do not resend it, contact support. |
| 550 | `ResponseServerUpdating` | The server is being updated. Retry in a minute. |
| 551 | `ResponseUniqueRequestViolation` | Not final: the request with that idempotency key is still running, or its outcome could not be confirmed (it may have gone through). Resend the same request with the same key a few seconds later, never a new key. The only exception: a 551 whose message says the key is already registered for a different request needs a new key. |
| 552 | `AuthError` | Your token isn't allowed to do this. |
| 553 | `NotFound` | The user or invoice doesn't exist. Codes that don't exist are limited: after 10 within an hour, every lookup by code (`InvoiceInfo`, `PayVoucher`, `RedeemVoucher`) is refused with 500 for an hour, for all the tokens of the account. Don't retry a code answered 553. |
| 554 | `NotEnoughFunds` | Your balance is too low. |
| 557 | `OutgoingSuspended` | The account is under review: its outgoing operations (withdrawals, transfers and vouchers, payments, red envelopes) are suspended. Final: nothing moved, do not resend. Contact support. |

The library methods never return `null` for a communication error, nor throw on an answer they cannot read. For a call that moves money (`TransferCreditAsync`, `SwapAsync`, `WithdrawAsync`, `PayVoucherAsync`, `RedeemVoucherAsync`) a lost or unreadable answer is 551: the operation may have gone through, resend the same request with the same key (`RedeemVoucherAsync` has no key: resend the same request, see below). Only an HTTP 400, 404, 405, 413 or 415 means PayGram refused the call before it ran (`ResponseGenericError`): that attempt was not executed: fix the call and send it again with the same key. For the other calls a lost or unreadable answer is `ResponseGenericError`. Always check `Success`.

**Every 551 is not final**, whatever its message: resend the same request with the same key (`RedeemVoucherAsync`: see below). The only exception is a 551 whose message says the key is *already registered for a different request*: that one needs a new key. If the same 551 keeps coming for more than a few minutes, stop resending and contact support; never use a new key.

**`CheckUniqueAsync` is not a step of a retry**: it only looks a key up and executes nothing. Use it when you no longer want the operation (for example the order was cancelled) and only want to know whether it went through, when you lost the request data and cannot resend it identically, or for an operation sent before version 2.6 (its key is not recognised: look it up by invoice id or callback data) or whose answer was lost with an SDK before 3.3 (see below).

`RedeemVoucherAsync` has no key, but a resend never redeems twice. Before redeeming a code for a customer, look it up with `CheckUniqueAsync`: accept only a voucher not yet redeemed (`Info.IsRedeemed` false), or a red envelope you did not create and have no entry in. After a 551 or no answer, resend it. If the resend fails (any 500), look the code up again. A voucher was redeemed into your account when `Info.IsRedeemed` is true, `Info.ToUser` is your Telegram id and `Info.ToClientId` your client id, and its sender (`Info.FromUser` with `Info.FromClientId`) is there and is not you (that would be a recall). A red envelope (`ResponseRedEnvelopeInvoiceInfo`) when its `Redeemers` have an entry with your Telegram id and client id and you are not its creator (`Info.FromUser`). Right after a 551 it may still be running: if it is not redeemed yet, check again a minute later. Credit a voucher code at most once on your side.

**Upgrading to 3.3.**
- A lost answer of a call that moves money is now 551, not `ResponseGenericError`: resend with the same key, never a new one.
- `TransferMoneyAsync` is obsolete (its `bool` cannot say "not confirmed"): use `TransferCreditAsync`. It now sends the key and the callback data in the body, so a value containing `+`, `&`, `#` or `%` reaches PayGram whole, unlike before. A transfer whose answer was lost before 3.3: check it with `CheckUniqueAsync` (or the statement) first, never resend it with a new key. If its key or callback data contained `+`, `&`, `#` or `%`, 3.2 sent them altered: neither a `NotFound` nor a 551 "different request" proves it did not run, and for such a key a resend through 3.3 is a new request too: resend it only if the statement shows it did not go through.

**Idempotency.** Every call that moves money (`TransferCredit`, `Swap`, `Withdraw`) takes a `Unique` key, at most 64 characters. Generate the key **before** the call and store it with your operation. After a 500, a 551 or no answer at all, resend the **same** request with the **same** key: the operation runs at most once. You get its answer (`ReplayedResponse` true), or, if it failed without moving money, it runs again. The exception is a transfer whose voucher went back to its sender before it reached the receiver: final, the same key only answers it again, a new transfer needs a new key. The same key with other parameters is refused with 551. Never retry with a new key: that would be a second operation. The only 551 that needs a new key is the one whose message says the key is already registered for a different request; a 500 with `ReplayedResponse` true is final: do not resend it, contact support. `CheckUniqueAsync` only looks a key up (see above and the API reference, section 3.4). Keys stay valid when you regenerate your API token.

**Payer reference (optional, 3.4).** When you send a transfer on behalf of one of your own users, put your id of that user in `PayerRef` (`payerRef` in JSON), at most 64 characters: an opaque reference such as your internal user number, never a name, an e-mail or a phone number. It is **write-only**: PayGram stores it with the transfer for its compliance checks (many of your users sending to the same PayGram user), only PayGram's admins see it, and it is never returned in any response or callback, to you included. It is never used as an identity or an authorization, and a request without it works as before. It is part of the request: a resend with the same `Unique` must carry the same `PayerRef`.

**Account under review (557, 3.4).** When PayGram's compliance suspends a user's outgoing operations, Withdraw, TransferCredit (and vouchers), PayVoucher and red envelopes answer `OutgoingSuspended` (557): final, nothing moved; do not resend it, with the same key or a new one. The user contacts support. The suspension is a state of the account that lasts until a PayGram admin releases it: retrying never lifts it. Receiving, deposits, swaps and the balance keep working.

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
| `InvoiceInfoCredited` (2) | `invoiceInfo`, with `transactionAmount > 0` | Money came in on an invoice: one of your invoices was paid, or a voucher you redeemed was credited. `callbackData` is the `Cd` of your invoice. `userIdInClientFrom` + `fromClientId`: who paid (or who sent the voucher). |
| `InvoiceInfoDebited` (5) | `invoiceInfo`, with `transactionAmount ≤ 0` | Money left on an invoice: you paid an invoice, or a voucher you sent was redeemed. `userIdInClientFrom` + `fromClientId`: whom you paid, or who redeemed. |
| `MoneySent` (4) | `moneySentInfo` | You sent a transfer or funded a voucher. Includes `voucherCode` and `redeemUrl`; for a direct transfer `userCliToId` + `toClientId`: who received it. |
| `BalanceInfo` (0) | `balanceInfo` | Your balance changed for another reason, for example you received a direct transfer: `userIdInClientFrom` + `fromClientId` say who sent it. |
| `WithdrawInfo` (1) | `withdrawInfo` | A withdrawal changed status. |
| `ReportReady` (6) | `reportInfo` | A statement export you booked is ready. |
| `InvoiceExpired` (7) | `expiredInfo` | A payment request you issued expired unpaid (30 minutes after its creation): status `Expired` (10), it cannot be paid any more, no money moved. `callbackData` is the `Cd` of your invoice; issue a new one if the customer still wants to pay. |

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
    "balance": 152.4,
    "userIdInClientFrom": "987654321",
    "fromClientId": 2
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

### Who is on the other side

Every callback about a movement names the **counterpart**, as the integration that owns that user knows it, together with the PayGram client it belongs to:

| Callback | Counterpart fields | Meaning |
|---|---|---|
| credited invoice | `userIdInClientFrom`, `fromClientId` | who paid your invoice; for a voucher you redeemed, who sent it |
| debited invoice | `userIdInClientFrom`, `fromClientId` | who redeemed your voucher (yourself on a recall); for an invoice you paid, whom you paid |
| money sent | `userCliToId`, `toClientId` | who received your direct transfer (null for a voucher) |
| balance | `userIdInClientFrom`, `fromClientId` | who sent you a direct transfer |

The client id tells you whose user base the id comes from: when it equals your own client id (`clientId` in `GetUserInfo`), the user id is one of your users, for example a Telegram id for users of the PayGram bot; otherwise the user belongs to another PayGram integration and the id lives in that integration's namespace. Use the pair to match payments and cashouts to your own records and to spot anomalies, for example the same payer across many accounts. The counterpart is disclosed only to the two sides of a movement, never to someone who merely holds an invoice code: `InvoiceInfo` returns the parties (`fromUser`, `toUser`, `originatingUser`, each with its `...ClientId`) only to a caller involved in the invoice.

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

# issue a 1 USDT voucher, open redeemUrl from another Telegram account to redeem it. "id" is your id of the cashout: a retry
# with the same id never pays twice. A 202 means not final: retry with the same id, and if it keeps coming for minutes
# contact support, never a new id. A 409: that id was already used for a different cashout. A 502: retry with the same id
# (once the cause is fixed), never a new one; a 500 replayed is final: contact support
curl -X POST http://localhost:5080/cashouts -H "Content-Type: application/json" -d '{"amount": 1, "currency": "USDT", "id": "cashout-1"}'
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
| `TransferCreditAsync(TransferCreditRequest)` | `TransferCreditV2` | `to` (Telegram id, or null for a voucher), `amt`, `curSym`, `unique`, `cd`, optional `payerRef` | `ResponseTransferCredit`, with `redeemUrl` |
| `GetInvoiceInfo(Guid)` | `InvoiceInfo` | query `invguid` | `ResponseInvoiceInfo` |
| `CheckUniqueAsync(CheckUniqueRequest)` | `CheckUnique` | `query`: invoice id, voucher code, callback data or unique key | `ResponseCheckUnique` |
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

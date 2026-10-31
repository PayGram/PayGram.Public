// PayGram sample: create invoices, issue vouchers (cashout) and receive the payment callbacks.
// Fill apiToken and signSeed in config.json (or PAYGRAM_API_TOKEN / PAYGRAM_SIGN_SEED), then `npm start`.
import http from "node:http";
import crypto from "node:crypto";
import { readFileSync } from "node:fs";
import { PayGramClient, verifyCallback, callbackType, CURRENCIES } from "./paygram.js";

const config = JSON.parse(readFileSync(new URL("./config.json", import.meta.url), "utf8"));
const apiToken = process.env.PAYGRAM_API_TOKEN || config.apiToken;
const signSeed = process.env.PAYGRAM_SIGN_SEED || config.signSeed;
const port = Number(process.env.PORT || config.port || 5081);
if (!apiToken || !signSeed) {
  console.error("Set apiToken and signSeed in config.json (see the README).");
  process.exit(1);
}

const paygram = new PayGramClient(apiToken, config.apiUrl);

// ids of the callbacks already handled. Delivery is at-least-once: a real app keeps them in its database
const handledCallbacks = new Set();

function readBody(req) {
  return new Promise((resolve, reject) => {
    const chunks = [];
    req.on("data", (c) => chunks.push(c));
    req.on("end", () => resolve(Buffer.concat(chunks)));
    req.on("error", reject);
  });
}

function send(res, status, body) {
  res.writeHead(status, { "Content-Type": typeof body === "string" ? "text/plain" : "application/json" });
  res.end(typeof body === "string" ? body : JSON.stringify(body));
}

// 1. Create an invoice: the customer opens payUrl in Telegram and pays it.
async function createInvoice({ amount, currency = "USDT" }) {
  // your order id: it comes back in the callback as callbackData, so you know which order was paid
  const orderId = `order-${crypto.randomUUID()}`;
  const r = await paygram.issueInvoice({ amount, currency, cd: orderId });
  if (!r.success) return [502, { error: `${r.responseCode}: ${r.message}` }];
  console.log(`Invoice ${r.invoiceCode} created for ${orderId}: ${r.amount} ${CURRENCIES[r.currencyCode]}`);
  return [200, { orderId, invoiceCode: r.invoiceCode, payUrl: r.payUrl }];
}

// 2. Cashout: a voucher is funded from your balance; whoever opens redeemUrl receives the money.
async function createCashout({ amount, currency = "USDT", id }) {
  // your id of this cashout, created once and stored with it (here sent by the caller): the same id is the same key on every
  // retry of this cashout, a new one is a new cashout. Required: made up here, a retry would be a second cashout
  if (!id) return [400, { error: "id is required: your id of this cashout, the same on every retry of it" }];
  const cashoutId = String(id);
  // unique is the idempotency key: if the response is lost, retry with the SAME value and the voucher is never funded twice
  const r = await paygram.transferCredit({ to: null, amount, currency, unique: cashoutId, cd: cashoutId });
  // 560: this id was already used for a different cashout (another amount or currency). This call did nothing; the first
  // cashout with this id may have gone through: look it up (checkUnique) before using a new id, never resend this one
  if (r.responseCode === 560) return [409, { cashoutId, error: r.message }];
  // 551: not final (the answer was lost, or it is still running): it may have gone through, retry with the same id
  if (r.responseCode === 551) return [202, { cashoutId, status: "not final (551): retry with the same id; if it keeps coming for minutes, contact support, never a new id", message: r.message }];
  // any other failure: retry with the same id once the cause is fixed, never a new one; a 500 replayed (replayedResponse true)
  // is final: do not resend it, contact support
  if (!r.success) return [502, { error: `${r.responseCode}: ${r.message} (retry with the same id, never a new one; if replayedResponse is true it is final: contact support)` }];
  console.log(`Voucher ${r.friendlyVoucherCode} issued: ${r.amount} ${currency}, balance left ${r.fromCredit}`);
  return [200, { cashoutId, voucherCode: r.voucherCode, friendlyVoucherCode: r.friendlyVoucherCode, redeemUrl: r.redeemUrl }];
}

// 3. Callbacks: PayGram POSTs every event of your account here.
function handleCallback(headers, rawBody) {
  // the signature covers the exact bytes received: verify before parsing
  if (!verifyCallback(signSeed, headers, rawBody)) {
    console.warn("Callback rejected: invalid signature");
    return 401;
  }
  const cb = JSON.parse(rawBody.toString("utf8"));

  // a 2xx tells PayGram the callback was delivered; a duplicate is acknowledged without being processed again
  if (handledCallbacks.has(cb.id)) return 200;
  handledCallbacks.add(cb.id);

  switch (callbackType(cb)) {
    case "InvoiceInfoCredited": {
      const i = cb.invoiceInfo;
      console.log(`PAID: invoice ${i.invoiceCode} of ${cb.callbackData}: received ${i.transactionAmount} ${CURRENCIES[i.currency]} (fees ${i.fees}), balance ${i.balance}`);
      // here: mark the order cb.callbackData as paid
      break;
    }
    case "MoneySent": {
      const m = cb.moneySentInfo;
      console.log(`Voucher ${m.friendlyVoucherCode} of ${cb.callbackData} funded: ${m.transactionAmount} ${CURRENCIES[m.currency]}, balance ${m.balance}`);
      break;
    }
    case "InvoiceInfoDebited": {
      const i = cb.invoiceInfo;
      console.log(`Debited: invoice ${i.invoiceCode} (${cb.callbackData}), ${i.transactionAmount} ${CURRENCIES[i.currency]}`);
      break;
    }
    default:
      console.log(`Callback ${cb.id} of type ${callbackType(cb)}: ${rawBody}`);
  }
  return 200;
}

const help = `PayGram sample is running.
  POST /invoices          {"amount": 1.5, "currency": "USDT"}   create an invoice, returns the link to pay it
  POST /cashouts          {"amount": 1.5, "currency": "USDT", "id": "your-cashout-id"}   issue a voucher, returns the link to redeem it
  POST /paygram/callback  the url to set in the bot (Settings > Set Callback URL), through a public https tunnel
`;

http.createServer(async (req, res) => {
  try {
    const url = new URL(req.url, "http://localhost");
    if (req.method === "GET" && url.pathname === "/") return send(res, 200, help);
    if (req.method !== "POST") return send(res, 404, "not found");

    const body = await readBody(req);
    if (url.pathname === "/paygram/callback") {
      res.writeHead(handleCallback(req.headers, body));
      return res.end();
    }
    const input = body.length ? JSON.parse(body.toString("utf8")) : {};
    if (!(Number(input.amount) > 0)) return send(res, 400, { error: "amount must be > 0" });
    if (url.pathname === "/invoices") return send(res, ...(await createInvoice(input)));
    if (url.pathname === "/cashouts") return send(res, ...(await createCashout(input)));
    send(res, 404, "not found");
  } catch (e) {
    console.error(e);
    send(res, 500, { error: String(e.message ?? e) });
  }
}).listen(port, () => console.log(`Listening on http://localhost:${port}`));

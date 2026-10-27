// Minimal PayGram users-api client: no dependencies, Node 18+ (global fetch).
import crypto from "node:crypto";

export const SIGNATURE_HEADER = "x-paygram-signature";
export const TIMESTAMP_HEADER = "x-paygram-timestamp";
const MAX_AGE_SECONDS = 600;

// Currencies enum of the api: responses and callbacks carry the number, requests accept the name.
export const CURRENCIES = {
  0: "UNKNOWN", 1: "BTC", 2: "ETH", 3: "EUR", 4: "USD", 5: "USDT", 7: "AED", 8: "LTC", 9: "XMR", 10: "XRP",
  11: "PHPT", 12: "CNY", 13: "PHP", 14: "VND", 15: "GBP", 16: "BNB", 17: "TRX", 18: "USDC", 19: "FTN",
};

// Callback types, derived the same way as UserCallbackInfo.CallbackType in the C# library.
export function callbackType(cb) {
  if (cb.balanceInfo) return "BalanceInfo";
  if (cb.withdrawInfo) return "WithdrawInfo";
  if (cb.invoiceInfo) return cb.invoiceInfo.transactionAmount > 0 ? "InvoiceInfoCredited" : "InvoiceInfoDebited";
  if (cb.moneySentInfo) return "MoneySent";
  if (cb.reportInfo) return "ReportReady";
  return "CallbackInfo";
}

// The answer of a call that moves money when PayGram's answer is lost or unreadable: it may have gone through. Resend the same
// request with the same unique key (it runs at most once): never a new key, which could run it twice.
export const NOT_CONFIRMED = "PayGram's answer did not arrive: the operation may have gone through. Resend the same request (with the same unique key, where it has one), never with a new key: a resend never runs it twice. A RedeemVoucher is resent too (it never redeems twice): a resend refused with refusalReason AlreadyRedeemedByYou went through the first time: credit its originalCredited only if you have not credited that voucher code yet (from the success or a callback); after any other failure check with CheckUnique by the voucher code whether it was redeemed into your account (API reference 4.2). Credit a voucher code at most once.";
const notConfirmed = () => ({ responseCode: 551, success: false, message: NOT_CONFIRMED });

export class PayGramClient {
  constructor(apiToken, apiUrl = "https://pubapi.paygr.am/PayGramUsers/") {
    this.apiToken = apiToken;
    this.apiUrl = apiUrl.replace(/\/+$/, "") + "/";
  }

  #post(method, body) {
    return fetch(`${this.apiUrl}${this.apiToken}/${method}`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(body ?? {}),
    });
  }

  // a call that moves no money: never throws, a lost or unreadable answer is a plain failure (500)
  async #call(method, body) {
    const failed = (why) => ({ responseCode: 500, success: false, message: `PayGram ${method}: ${why}` });
    let res;
    try { res = await this.#post(method, body); } catch { return failed("no answer"); }
    const json = res.ok ? await res.json().catch(() => null) : null;
    // every response carries responseCode: without it, it is not PayGram's answer; below 400 means success
    if (json == null || typeof json.responseCode !== "number") return failed(`no answer that could be read (HTTP ${res.status})`);
    json.success = json.responseCode < 400;
    return json;
  }

  // a call that moves money: never throws. PayGram refused it before it ran (400, 404, 405, 413, 415): this attempt was not
  // executed (fix the call and send it again with the same key). Anything else (no answer, a 5xx, any other 4xx,
  // which may come from a proxy on the resend of a call that ran, an answer that is not PayGram's) is 551 NOT_CONFIRMED
  async #money(method, body) {
    let res;
    try { res = await this.#post(method, body); } catch { return notConfirmed(); }
    if (!res.ok) {
      if ([400, 404, 405, 413, 415].includes(res.status))
        return { responseCode: 500, success: false, message: `PayGram refused this call (HTTP ${res.status}): this attempt was not executed: fix the call and send it again with the same key.` };
      return notConfirmed();
    }
    let json;
    try { json = await res.json(); } catch { return notConfirmed(); }
    if (json == null || typeof json.responseCode !== "number") return notConfirmed();
    json.success = json.responseCode < 400;
    return json;
  }

  /** Creates an invoice paid to you. cd comes back in the payment callback as callbackData. */
  issueInvoice({ amount, currency, cd }) {
    return this.#call("IssueInvoiceV2", { amt: amount, curSym: currency, cd });
  }

  /**
   * Without `to` creates a voucher (cashout): the response carries redeemUrl.
   * `unique` is the idempotency key: retry with the same value and the transfer never happens twice.
   * Never throws: responseCode 551 means not final (resend with the same key; its message says when the key is already used
   * for a different request).
   */
  transferCredit({ to = null, amount, currency, unique, cd }) {
    return this.#money("TransferCreditV2", { to, amt: amount, curSym: currency, unique, cd });
  }

  /**
   * What happened to an operation: by its unique key, an invoice id, a voucher code or your callback data. A lookup that
   * executes nothing, not a step of a retry (after a 500, a 551 or no answer, resend the same request with the same key).
   */
  checkUnique(query) {
    return this.#call("CheckUnique", { query });
  }

  invoiceInfo(invoiceCode) {
    return this.#call("InvoiceInfoV2", { invGuid: invoiceCode });
  }
}

/**
 * Validates the hmac of a callback: hex(hmac-sha256(signSeed, `${timestamp}.${rawBody}`)), uppercase.
 * rawBody must be the exact bytes received, before any JSON parsing.
 */
export function verifyCallback(signSeed, headers, rawBody) {
  const ts = headers[TIMESTAMP_HEADER];
  const sig = headers[SIGNATURE_HEADER];
  if (!ts || !sig || !/^\d+$/.test(ts)) return false;
  if (Math.abs(Math.floor(Date.now() / 1000) - Number(ts)) > MAX_AGE_SECONDS) return false;

  // the seed is a guid and the server signs with its lowercase text
  const key = signSeed.trim().toLowerCase();
  const expected = crypto.createHmac("sha256", key).update(`${ts}.`).update(rawBody).digest("hex").toUpperCase();
  const a = Buffer.from(expected);
  const b = Buffer.from(String(sig).toUpperCase());
  return a.length === b.length && crypto.timingSafeEqual(a, b);
}

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

export class PayGramClient {
  constructor(apiToken, apiUrl = "https://pubapi.paygr.am/PayGramUsers/") {
    this.apiToken = apiToken;
    this.apiUrl = apiUrl.replace(/\/+$/, "") + "/";
  }

  async #call(method, body) {
    const res = await fetch(`${this.apiUrl}${this.apiToken}/${method}`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(body ?? {}),
    });
    if (!res.ok) throw new Error(`PayGram ${method}: HTTP ${res.status}`);
    const json = await res.json();
    // every response carries responseCode: below 400 means success
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
   */
  transferCredit({ to = null, amount, currency, unique, cd }) {
    return this.#call("TransferCreditV2", { to, amt: amount, curSym: currency, unique, cd });
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

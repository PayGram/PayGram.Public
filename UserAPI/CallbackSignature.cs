using System.Security.Cryptography;
using System.Text;

namespace PayGram.Public.UserAPI
{
	/// <summary>
	/// Hmac signature attached by the PayGram server to the callbacks it sends and verified by the receiver.
	/// The signature covers "{timestamp}.{raw json body}", so neither the payload nor the timestamp can be
	/// forged or reused on a different body. The key is the client token: a secret the two parties already
	/// share (it is the same value the client embeds in the api url when calling the PayGram server).
	/// The timestamp is generated at send time, not at event time, so it stays fresh even when the
	/// notificator retries a days-old notification.
	/// </summary>
	public static class CallbackSignature
	{
		public const string SIGNATURE_HEADER = "X-PayGram-Signature";
		public const string TIMESTAMP_HEADER = "X-PayGram-Timestamp";
		/// <summary>Maximum accepted difference, in seconds, between the timestamp header and the receiver clock</summary>
		public const int MAX_AGE_SECONDS = 600;

		/// <summary>
		/// Computes the uppercase-hex hmac-sha256 of "{unixTimestamp}.{body}" with the given key
		/// </summary>
		public static string Compute(string key, long unixTimestamp, string body)
		{
			using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
			return Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes($"{unixTimestamp}.{body}")));
		}

		/// <summary>
		/// Validates the signature headers against the raw request body.
		/// Constant-time comparison: the response time must not reveal how many leading characters matched.
		/// </summary>
		/// <param name="key">The shared secret (the receiver's client token)</param>
		/// <param name="timestampHeader">The value of <see cref="TIMESTAMP_HEADER"/></param>
		/// <param name="signatureHeader">The value of <see cref="SIGNATURE_HEADER"/></param>
		/// <param name="body">The raw request body, exactly as received</param>
		public static bool Validate(string key, string? timestampHeader, string? signatureHeader, string body)
		{
			if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(timestampHeader) || string.IsNullOrWhiteSpace(signatureHeader))
				return false;
			if (long.TryParse(timestampHeader, out long ts) == false)
				return false;
			if (Math.Abs(DateTimeOffset.UtcNow.ToUnixTimeSeconds() - ts) > MAX_AGE_SECONDS)
				return false;

			string expected = Compute(key, ts, body);
			return CryptographicOperations.FixedTimeEquals(
				Encoding.UTF8.GetBytes(expected),
				Encoding.UTF8.GetBytes(signatureHeader.ToUpperInvariant()));
		}
	}
}

using CurrenciesLib;
using Newtonsoft.Json;

namespace PayGram.Public.Responses
{
	/// <summary>
	/// Returned by <c>GetUserInfo</c>.
	/// Contains the user's profile, wallet balances, and callback configuration.
	/// Populated from <c>DbUser</c> via <c>MakeResponseUserInfo</c>.
	/// </summary>
	public class ResponseUserInfo : PaygramResponse
	{
		/// <summary>
		/// Convenience property: the balance of the user's first (default) wallet. Not serialized to JSON.
		/// </summary>
		[JsonIgnore]
		public decimal Balance => Balances?.FirstOrDefault()?.Balance ?? 0;
		/// <summary>
		/// Convenience property: the currency of the user's first (default) wallet. Not serialized to JSON.
		/// </summary>
		[JsonIgnore]
		public Currencies Currency => Balances?.FirstOrDefault()?.Currency ?? Currencies.UNKNOWN;
		/// <summary>
		/// All wallet balances for this user. Each entry represents one currency wallet,
		/// populated from <c>DbUser.Wallets</c>.
		/// </summary>
		public List<BalanceInfo> Balances { get; set; } = new List<BalanceInfo>();
		/// <summary>
		/// The UTC date and time when this user first registered in the PayGram system.
		/// </summary>
		public DateTime JoinedOn { get; set; }
		/// <summary>
		/// The user's identifier in the calling client's system (e.g. Telegram user ID).
		/// </summary>
		public string UserIdInClient { get; set; }
		/// <summary>
		/// The PayGram client the user belongs to (your integration). The counterpart of a movement is reported with its own
		/// client id (<c>FromClientId</c>, <c>ToClientId</c>, ...): when it equals this one, its user id is in your user base.
		/// </summary>
		public int ClientId { get; set; }
		/// <summary>
		/// Not returned any more (issue PayGram#19, 2026-10-26): always <see cref="Guid.Empty"/> and left out of the JSON. A user's
		/// PayGram id is internal (an admin's is its AdminKey); identify users by <see cref="UserIdInClient"/> and <see cref="ClientId"/>.
		/// Kept for the binary compatibility of the consumers.
		/// </summary>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public Guid UID { get; set; }
		/// <summary>
		/// The URL that PayGram will POST callback notifications to for this user's events.
		/// </summary>
		public string CallbackUrl { get; set; }
		/// <summary>
		/// The secret seed used to compute SHA-256 HMAC signatures on callback payloads,
		/// so the client can verify that callbacks originate from PayGram.
		/// <para>Returned by <c>GetUserInfo</c> to the account's owner (its own API token). Left out of the JSON only when empty.</para>
		/// </summary>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public Guid SignSeed { get; set; }
		public ResponseUserInfo()
								: base(PaygramResponseTypes.ResponseUserInfo)
		{

		}

		public ResponseUserInfo(ResponseCodes code) : base(PaygramResponseTypes.ResponseUserInfo, code)
		{

		}
	}
}

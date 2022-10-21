using CurrenciesLib;
using Newtonsoft.Json;
using PayGram.Public;

namespace PayGram.Types
{
	public class ResponseUserInfo : PaygramResponse
	{
		/// <summary>
		/// The balance of the first wallet of this user
		/// </summary>
		[JsonIgnore]
		public decimal Balance => Balances?.FirstOrDefault()?.Balance ?? 0;
		/// <summary>
		/// The currency of the first wallet of this user
		/// </summary>
		[JsonIgnore]
		public Currencies Currency => Balances?.FirstOrDefault()?.Currency ?? Currencies.UNKNOWN;
		public List<BalanceInfo> Balances { get; set; } = new List<BalanceInfo>();
		/// <summary>
		/// The UTC date and time when this user joined
		/// </summary>
		public DateTime JoinedOn { get; set; }
		/// <summary>
		/// The id in the client's system
		/// </summary>
		public string UserIdInClient { get; set; }
		/// <summary>
		/// The Unique Identifier in the Paygram System
		/// </summary>
		public Guid UID { get; set; }
		/// <summary>
		/// The Callback Url to call when an update is made to this user
		/// </summary>
		public string CallbackUrl { get; set; }
		/// <summary>
		/// The SignSeed used to sign callbacks with the SHA256 alg
		/// </summary>
		public Guid SignSeed { get; set; }
		public ResponseUserInfo()
								: base((PaygramResponseTypes)Enum.Parse(typeof(PaygramResponseTypes), nameof(ResponseUserInfo)))
		{

		}
	}
}

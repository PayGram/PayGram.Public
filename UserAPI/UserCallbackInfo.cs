using Newtonsoft.Json;
using System.Security.Cryptography;

namespace PayGram.Public.UserAPI
{
	public class UserCallbackInfo
	{
		/// <summary>
		/// When sent to the User it is the unique identifier for this notification
		/// </summary>
		public int Id { get; set; }
		public DateTime DateUtc { get; set; }
		/// <summary>
		/// Unix representation of DateUtc in seconds
		/// </summary>
		public long Timestamp => ((DateTimeOffset)DateUtc).ToUnixTimeSeconds();
		public string CallbackData { get; set; }
		/// <summary>
		/// The user id at the client side that got updated
		/// </summary>
		public string UserCliId { get; set; }

		public UserCallbackBalanceInfo BalanceInfo { get; set; }
		public UserCallbackMoneySent MoneySentInfo { get; set; }
		public UserCallbackWithdraw WithdrawInfo { get; set; }
		public UserCallbackInvoiceInfo InvoiceInfo { get; set; }
		public string Hash { get; set; }
		public UserCallbackTypes CallbackType => BalanceInfo != null ? UserCallbackTypes.BalanceInfo
			: WithdrawInfo != null ? UserCallbackTypes.WithdrawInfo
			: InvoiceInfo != null ? (InvoiceInfo.TransactionAmount>0? UserCallbackTypes.InvoiceInfoCredited : UserCallbackTypes.InvoiceInfoDebited)
			: MoneySentInfo != null ? UserCallbackTypes.MoneySent
			: UserCallbackTypes.CallbackInfo;


		public UserCallbackInfo()
		{
			DateUtc = DateTime.UtcNow;
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented,
													new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate });
		}
	}
}

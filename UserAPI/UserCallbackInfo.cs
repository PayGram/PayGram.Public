using Newtonsoft.Json;
using System;

namespace PayGram.Public.UserAPI
{
	public class UserCallbackInfo
	{
		/// <summary>
		/// When sent to the User it is the unique identifier for this notification
		/// </summary>
		public int Id { get; set; }
		public DateTime DateUtc { get; set; }
		public string CallbackData { get; set; }
		/// <summary>
		/// The user id for at the client side that got updated
		/// </summary>
		public string UserCliId { get; set; }

		public UserCallbackBalanceInfo BalanceInfo { get; set; }
		public UserCallbackMoneySent MoneySentInfo { get; set; }
		public UserCallbackWithdraw WithdrawInfo { get; set; }
		public UserCallbackInvoiceInfo InvoiceInfo { get; set; }


		public UserCallbackTypes CallbackType => BalanceInfo != null ? UserCallbackTypes.BalanceInfo
			: WithdrawInfo != null ? UserCallbackTypes.WithdrawInfo
			: InvoiceInfo != null ? UserCallbackTypes.InvoiceInfo
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

using Newtonsoft.Json;

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
		public UserCallbackReportInfo ReportInfo { get; set; }
		/// <summary>
		/// DEPRECATED, verify the callback through the hmac headers instead (see <see cref="CallbackSignature"/>):
		/// they cover the whole body ("{timestamp}.{raw json body}", key = your SignSeed), while this legacy field
		/// is only sha256(SignSeed + Timestamp) - it does not authenticate the payload and is replayable.
		/// It is still populated for backward compatibility and will be removed in a future release.
		/// Note that it is computed when the notification is created: if the SignSeed is rotated while a
		/// notification is still being retried, this field carries the old seed while the headers use the new one.
		/// </summary>
		[Obsolete("Verify callbacks through the CallbackSignature hmac headers instead. This field does not authenticate the payload and will be removed in a future release.")]
		public string Hash { get; set; }
		public UserCallbackTypes CallbackType => BalanceInfo != null ? UserCallbackTypes.BalanceInfo
			: WithdrawInfo != null ? UserCallbackTypes.WithdrawInfo
			: InvoiceInfo != null ? (InvoiceInfo.TransactionAmount > 0 ? UserCallbackTypes.InvoiceInfoCredited : UserCallbackTypes.InvoiceInfoDebited)
			: MoneySentInfo != null ? UserCallbackTypes.MoneySent
			: ReportInfo != null ? UserCallbackTypes.ReportReady
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

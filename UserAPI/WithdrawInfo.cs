using Newtonsoft.Json;

namespace PayGram.Public.UserAPI
{
	public class WithdrawInfo
	{
		/// <summary>
		/// Removed from the callback in 3.9.0: it was an internal database id. Never sent, never read: always 0. Identify the
		/// withdrawal by <see cref="WithdrawId"/> (the same as the callback's <c>InvoiceCode</c>) or by the callback's
		/// <c>FriendlyVoucherCode</c>
		/// </summary>
		[JsonIgnore]
		[Obsolete("Removed in 3.9.0 (an internal database id, no longer sent): use WithdrawId or the callback's InvoiceCode / FriendlyVoucherCode")]
		public long InternalId { get; set; }
		public WithdrawStatuses Status { get; set; }
		/// <summary>
		/// The amount to send in the requested currency
		/// </summary>
		public decimal Amount { get; set; }
		public decimal Fees { get; set; }
		public string UserId { get; set; }
		public decimal BalanceBeforeWithdraw { get; set; }
		public Guid WithdrawId { get; set; }
		public WithdrawMethod WithdrawMethod { get; set; }
	}
}

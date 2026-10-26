using CurrenciesLib;
using Newtonsoft.Json;

namespace PayGram.Public.Requests
{
	/// <summary>
	/// Request body for <c>SwapV2</c>.
	/// Exchanges funds from one currency to another in the user's account at the current rate.
	/// </summary>
	public class SwapRequest
	{
		/// <summary>Amount to swap (must be &gt; 0).</summary>
		public decimal Amt { get; set; }
		/// <summary>Source currency to swap from.</summary>
		public Currencies CurSym { get; set; }
		/// <summary>Destination currency to swap to.</summary>
		public Currencies CurSymDest { get; set; }
		/// <summary>Client-generated idempotency key (max 64 bytes). Prevents duplicate swaps.</summary>
		public string Unique { get; set; } = string.Empty;
		/// <summary>Optional callback data returned in notifications related to this swap.</summary>
		public string? Cd { get; set; }
		/// <summary>True to see the preview of the conversion, false to execute it.</summary>
		public bool Simulate { get; set; }
		/// <summary>
		/// Optional, to execute: the least the swap must give in <see cref="CurSymDest"/>, e.g. the amount the simulation showed.
		/// If the rate got worse and the swap would give less, it is refused with 558 (QuoteChanged) and nothing moves; simulate
		/// again and send the new amount with a new unique key. Absent: the swap runs at the rate of its moment
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)] // absent when null: the wire format of a call without it is unchanged
		public decimal? MinRcv { get; set; }
	}
}

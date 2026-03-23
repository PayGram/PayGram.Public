using CurrenciesLib;

namespace PayGram.Public.Requests
{
	/// <summary>
	/// Request body for <c>ConvertV2</c>.
	/// Read-only currency conversion quote -- no funds are moved. Use <see cref="SwapRequest"/> to exchange funds.
	/// </summary>
	public class ConvertRequest
	{
		/// <summary>Amount to convert.</summary>
		public decimal Amt { get; set; }
		/// <summary>Source currency.</summary>
		public Currencies CurSym { get; set; }
		/// <summary>Destination currency.</summary>
		public Currencies CurSymDest { get; set; }
	}
}

namespace PayGram.Public.Responses
{
	/// <summary>
	/// Extra information about a payment gathered from an external provider (e.g. Yapay via MCP).
	/// Kept deliberately open (a bag of labelled fields) so a new provider can be added without
	/// changing the fund-flow API surface. <see cref="Available"/> is false when no provider could
	/// enrich the transaction — the UI then simply omits the provider panel.
	/// </summary>
	public class PaymentEnrichment
	{
		public bool Available { get; set; }
		/// <summary>
		/// The provider that supplied the info (e.g. "Yapay"), null when none did.
		/// </summary>
		public string? Provider { get; set; }
		/// <summary>
		/// Labelled fields to display, in insertion order (e.g. "Merchant reference" -> "...").
		/// </summary>
		public Dictionary<string, string> Fields { get; set; } = new();
		/// <summary>
		/// Optional human-readable note (e.g. "Provider not configured", or an error explanation).
		/// </summary>
		public string? Note { get; set; }
	}
}

namespace PayGram.Public.Responses
{
	public enum FundFlowNodeTypes
	{
		/// <summary>
		/// A user of the same client (identified by UserCliId) — expandable in the graph
		/// </summary>
		User = 0,
		/// <summary>
		/// A user of a different client: its UserCliId lives in another namespace, not expandable
		/// </summary>
		ExternalUser = 1,
		/// <summary>
		/// An on-chain address (deposit sender or withdrawal destination)
		/// </summary>
		OnChain = 2,
		/// <summary>
		/// A bank account (fiat withdrawal destination)
		/// </summary>
		Bank = 3,
		/// <summary>
		/// A merchant/business
		/// </summary>
		Merchant = 4,
		/// <summary>
		/// The platform itself (ROOT / system: fees, swaps, refunds)
		/// </summary>
		System = 5,
		Unknown = 6,
	}

	/// <summary>
	/// One end of a fund-flow movement.
	/// </summary>
	public class FundFlowNode
	{
		/// <summary>
		/// Stable graph key: the UserCliId for a user, "ext:&lt;cliId&gt;" for an external user,
		/// the address for on-chain/bank, "system" for the platform.
		/// </summary>
		public string Id { get; set; } = "";
		public FundFlowNodeTypes Type { get; set; }
		/// <summary>
		/// Display label (UserCliId, address, or "System"). The Telegram bot may rewrite a
		/// UserCliId to include the username, as it does for reports.
		/// </summary>
		public string Label { get; set; } = "";
		/// <summary>
		/// Set only for <see cref="FundFlowNodeTypes.User"/>, so the caller can expand this node.
		/// </summary>
		public string? UserCliId { get; set; }
	}
}

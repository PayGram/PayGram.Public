using CurrenciesLib;

namespace PayGram.Public.UserAPI
{
	public class P2PGroup
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Link { get; set; }
		public P2PGroupType P2PGroupType { get; set; } = P2PGroupType.None;
		public bool Status { get; set; }
		public string? Notes { get; set; }
		public decimal FixedFee { get; set; }
		public double PercFee { get; set; }
		public double ChangeRate { get; set; }
		public decimal MaxBuy { get; set; }
		public decimal MinBuy { get; set; }
		public string OwnerIdInClient { get; set; }
		/// <summary>
		/// The currency owned by the owner of this group
		/// </summary>
		public Currencies OwnerCurrency { get; set; }
		/// <summary>
		/// The currency owned by the trader
		/// </summary>
		public Currencies TraderCurrency { get; set; }
	}
}
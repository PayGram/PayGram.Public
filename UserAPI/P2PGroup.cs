using System.ComponentModel;
using CurrenciesLib;
using Newtonsoft.Json;
using System.Web;

namespace PayGram.Public.UserAPI
{
	public class P2PGroup
	{
		public int Id { get; set; }
		public string Name { get; set; }
		[JsonIgnore]
		public string? NameHtmlEncoded => HttpUtility.HtmlEncode(Name);
		public string? Link { get; set; }
		public P2PGroupType P2PGroupType { get; set; } = P2PGroupType.None;
		public bool Status { get; set; }
		public string? Notes { get; set; }
		[JsonIgnore]
		public string? NotesHtmlEncoded => HttpUtility.HtmlEncode(Notes);
		public decimal FixedFee { get; set; }
		public double PercFee { get; set; }
		public double ChangeRate { get; set; }
		public decimal MaxBuy { get; set; }
		public decimal MinBuy { get; set; }
		public string OwnerIdInClient { get; set; }
		/// <summary>
		/// True when the owner is a user of the requesting client: ads are listed across clients, so <see cref="OwnerIdInClient"/> of a
		/// foreign ad lives in another namespace and must not be matched against local users. Defaults to true for ads a client creates.
		/// </summary>
		// the default is true: IgnoreAndPopulate must drop true (not false) when storing, and a missing field reads back as true
		[DefaultValue(true)]
		public bool OwnerBelongsToClient { get; set; } = true;
		/// <summary>
		/// The currency owned by the owner of this group
		/// </summary>
		public Currencies OwnerCurrency { get; set; }
		/// <summary>
		/// The currency owned by the trader
		/// </summary>
		public Currencies TraderCurrency { get; set; }
		public bool IsApproved { get; set; }
		public object UpdatedUTC { get; set; }
	}
}
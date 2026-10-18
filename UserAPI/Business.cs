using System.ComponentModel;
using Newtonsoft.Json;
using System.Web;

namespace PayGram.Public.UserAPI
{
	public class Business
	{
		public long Id { get; set; }
		public string Name { get; set; }
		[JsonIgnore]
		public string? NameHtmlEncoded => HttpUtility.HtmlEncode(Name);
		public string OwnerIdInClient { get; set; }
		/// <summary>
		/// True when the owner is a user of the requesting client: businesses are listed across clients, so <see cref="OwnerIdInClient"/>
		/// of a foreign business lives in another namespace and must not be matched against local users. Defaults to true for businesses a client creates.
		/// </summary>
		// the default is true: IgnoreAndPopulate must drop true (not false) when storing, and a missing field reads back as true
		[DefaultValue(true)]
		public bool OwnerBelongsToClient { get; set; } = true;
		public bool IsOnline { get; set; }
		public string? LocationUrl { get; set; }
		public string? ImageUrl { get; set; }
		[JsonIgnore]
		public string? DescriptionHtmlEncoded => HttpUtility.HtmlEncode(Description);
		public string? Description { get; set; }
		public bool IsApproved { get; set; }
		public bool Deleted { get; set; }
		public DateTime UpdatedUTC { get; set; }
		public DateTime CreatedUTC { get; set; }
	}
}
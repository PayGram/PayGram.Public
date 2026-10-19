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
		/// The PayGram client of the owner. Businesses are listed across clients: compare it with your own client id
		/// (<c>ResponseUserInfo.ClientId</c>) before matching <see cref="OwnerIdInClient"/> against your users. Null for the records a
		/// client builds about its own users.
		/// </summary>
		public int? OwnerClientId { get; set; }
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
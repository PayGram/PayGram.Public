namespace PayGram.Public.UserAPI
{
	public class Business
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string OwnerIdInClient { get; set; }
		public bool IsOnline { get; set; }
		public string? LocationUrl { get; set; }
		public string? ImageUrl { get; set; }
		public string? Description { get; set; }
		public bool IsApproved { get; set; }
        public bool Deleted { get; set; }
    }
}
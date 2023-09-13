namespace PayGram.Public
{
	public class RequestUpdateBusiness : PaygramRequest
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public string? ImageUrl { get; set; }
		public string? LocationUrl { get; set; }
		public bool IsOnline { get; set; }
		public bool Approved { get; set; }
	}
}

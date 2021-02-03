using Newtonsoft.Json;
using System.Text;

namespace PayGram.Public.UserAPI
{
	public class GeoAddress
	{
		public string BuildingNumber { get; set; }
		public string Street1 { get; set; }
		public string Street2 { get; set; }
		public string City { get; set; }
		public string Province { get; set; }
		public string ZIP_Postal_Code { get; set; }
		public string Country { get; set; }

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented,
													new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate });
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			if (string.IsNullOrWhiteSpace(BuildingNumber) == false)
			{
				sb.AppendLine("Building Number:");
				sb.AppendLine(BuildingNumber);
			}
			if (string.IsNullOrWhiteSpace(Street1) == false)
			{
				sb.AppendLine("Street1:");
				sb.AppendLine(Street1);
			}
			if (string.IsNullOrWhiteSpace(Street2) == false)
			{
				sb.AppendLine("Street2:");
				sb.AppendLine(Street2);
			}
			if (string.IsNullOrWhiteSpace(City) == false)
			{
				sb.AppendLine("City:");
				sb.AppendLine(City);
			}
			if (string.IsNullOrWhiteSpace(Province) == false)
			{
				sb.AppendLine("Province:");
				sb.AppendLine(Province);
			}
			if (string.IsNullOrWhiteSpace(ZIP_Postal_Code) == false)
			{
				sb.AppendLine("ZIP/Post Code:");
				sb.AppendLine(ZIP_Postal_Code);
			}
			if (string.IsNullOrWhiteSpace(Country) == false)
			{
				sb.AppendLine("Country:");
				sb.AppendLine(Country);
			}
			return sb.ToString();
		}
	}
}
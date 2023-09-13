using Newtonsoft.Json;

namespace PayGram.Public
{
	public class PaygramResponse : IPaygramResponse
	{
		[JsonIgnore]
		public readonly static PaygramResponse ServerUpdating = new("Error processing the request, the server is being updated.") { Success = false };
		[JsonIgnore]
		public readonly static PaygramResponse ResponseError = new("Error processing the request.") { Success = false };
		[JsonIgnore]
		public readonly static PaygramResponse ResponseOk = new("Request processed succesfully.") { Success = true };

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string Message { get; set; }

		public PaygramResponseTypes Type { get; set; }

		public bool Success { get; set; }

		/// <summary>
		/// Success = false
		/// Type = Unknown
		/// </summary>
		public PaygramResponse()
		{
		}

		/// <summary>
		/// Success = false
		/// </summary>
		/// <param name="errorMessage"></param>
		public PaygramResponse(string errorMessage)
		{
			Message = errorMessage;
			Success = false;
		}

		/// <summary>
		/// Success = true
		/// </summary>
		/// <param name="type"></param>
		public PaygramResponse(PaygramResponseTypes type)
		{
			Type = type;
			Success = true;
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented,
													new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate });
		}
	}
}

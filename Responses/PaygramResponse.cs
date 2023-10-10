using Newtonsoft.Json;

namespace PayGram.Public
{
	public class PaygramResponse : IPaygramResponse
	{
		[JsonIgnore]
		public readonly static PaygramResponse ServerUpdating = new("Error processing the request, the server is being updated.", ResponseCodes.ResponseServerUpdating);
		[JsonIgnore]
		public readonly static PaygramResponse ResponseError = new("Error processing the request.", ResponseCodes.ResponseGenericError);
		[JsonIgnore]
		public readonly static PaygramResponse ResponseOk = new("Request processed succesfully.", ResponseCodes.ResponseOK);
		[JsonIgnore]
		public readonly static PaygramResponse ResponseUniqueViolation = new("The request was alredy processed or it's being processed.", ResponseCodes.ResponseUniqueRequestViolation);

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string Message { get; set; }

		public PaygramResponseTypes Type { get; set; }
		public ResponseCodes ResponseCode { get; set; }
		public bool Success { get; set; }

		/// <summary>
		/// Success = false
		/// Type = Unknown
		/// </summary>
		public PaygramResponse() : this(PaygramResponseTypes.Unknown, ResponseCodes.ResponseGenericError)
		{

		}
		public PaygramResponse(string err) : this(err, PaygramResponseTypes.Unknown, ResponseCodes.ResponseGenericError)
		{
		}
		/// <summary>
		/// Success = true
		/// </summary>
		/// <param name="type"></param>
		public PaygramResponse(PaygramResponseTypes type) : this(type.ToString(), type, ResponseCodes.ResponseOK)
		{
		}
		public PaygramResponse(ResponseCodes code) : this(code.ToString(), PaygramResponseTypes.Unknown, code)
		{
		}
		/// <summary>
		/// Success = false
		/// </summary>
		/// <param name="errorMessage"></param>
		public PaygramResponse(string errorMessage, PaygramResponseTypes type) : this(errorMessage, type, ResponseCodes.ResponseGenericError)
		{
		}
		public PaygramResponse(string errorMessage, ResponseCodes responseCode) : this(errorMessage, PaygramResponseTypes.Unknown, responseCode)
		{
		}
		public PaygramResponse(PaygramResponseTypes type, ResponseCodes responseCode) : this(responseCode.ToString(), type, responseCode)
		{
		}
		/// <summary>
		/// Creates a new paygram response with the specified message and type
		/// </summary>
		/// <param name="message">The message to return</param>
		/// <param name="type">The type of the response</param>
		/// <param name="responseCode">If this is greater than or equal to 500, success will be set to false, otherwise true</param>
		public PaygramResponse(string message, PaygramResponseTypes type, ResponseCodes responseCode)
		{
			Message = message;
			Type = type;
			ResponseCode = responseCode;
			Success = (int)responseCode < 400;
		}
		/// <summary>
		/// Creates a new paygram response with the specified message and type
		/// </summary>
		/// <param name="type">The type of the response</param>
		/// <param name="responseCode">If this is greater than or equal to 500, success will be set to false, otherwise true</param>
		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented,
													new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate });
		}
	}
}

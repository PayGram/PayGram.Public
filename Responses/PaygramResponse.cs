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
		public bool Success => (int)ResponseCode < 400;//{ get; set; }

		/// <summary>
		/// True when this response was not produced by executing the request, but replayed from the stored
		/// outcome of the original request that carried the same ClientUnique (idempotent replay), or when it is a
		/// final failure the server closed (its money moved, or a transfer went back to its sender): the same key
		/// only answers it again (a 500 with this true is final: do not resend it, contact support). Retrying a request
		/// with the same ClientUnique after a 500, a 551 or a lost response is safe: the operation is never executed twice.
		/// </summary>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public bool ReplayedResponse { get; set; }

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
			//Success = (int)responseCode < 400;
		}
		/// <summary>
		/// Creates a new paygram response with the specified message and type
		/// </summary>
		/// <param name="type">The type of the response</param>
		/// <param name="responseCode">If this is greater than or equal to 500, success will be set to false, otherwise true</param>
		public string ToJson()
		{
			return PublicJson.ToStoredJson(this);
		}
	}
}

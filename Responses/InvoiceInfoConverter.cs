using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PayGram.Public.Responses
{
	/// <summary>
	/// Reads an invoice info into its real type by the Type discriminator every response carries: a red envelope keeps its
	/// redeemers (<see cref="ResponseRedEnvelopeInvoiceInfo"/>), a withdrawal its method (<see cref="ResponseInvoiceWithdrawInfo"/>).
	/// Any other value of Type, or none, reads as a plain <see cref="ResponseInvoiceInfo"/>. Writing is left to the serializer, which
	/// already writes the runtime type whole. Safe on a property, on the items of a list and among the serializer's converters:
	/// the object is populated, so the converter never runs again on the value it is reading.
	/// </summary>
	public sealed class InvoiceInfoConverter : JsonConverter<ResponseInvoiceInfo>
	{
		public override bool CanWrite => false;

		public override ResponseInvoiceInfo? ReadJson(JsonReader reader, Type objectType, ResponseInvoiceInfo? existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null) return null;
			// the amounts as decimals: read as doubles they would lose digits (12345678.12345678 -> 12345678.1234568)
			var floats = reader.FloatParseHandling;
			reader.FloatParseHandling = FloatParseHandling.Decimal;
			JObject obj;
			try { obj = JObject.Load(reader); }
			finally { reader.FloatParseHandling = floats; }
			// the discriminator as a number, or as the name of the type
			var typeToken = obj.GetValue("type", StringComparison.OrdinalIgnoreCase);
			var type = PaygramResponseTypes.ResponseInvoiceInfo;
			if (typeToken?.Type == JTokenType.Integer) type = (PaygramResponseTypes)typeToken.Value<int>();
			else if (typeToken?.Type == JTokenType.String && Enum.TryParse<PaygramResponseTypes>(typeToken.Value<string>(), true, out var named)) type = named;
			ResponseInvoiceInfo info = type switch
			{
				PaygramResponseTypes.ResponseRedEnvelopeInvoiceInfo => new ResponseRedEnvelopeInvoiceInfo(),
				PaygramResponseTypes.ResponseInvoiceWithdrawInfo => new ResponseInvoiceWithdrawInfo(),
				_ => new ResponseInvoiceInfo(),
			};
			using (var fields = obj.CreateReader())
				serializer.Populate(fields, info);
			return info;
		}

		public override void WriteJson(JsonWriter writer, ResponseInvoiceInfo? value, JsonSerializer serializer)
			=> throw new NotSupportedException("written by the serializer: CanWrite is false");
	}
}

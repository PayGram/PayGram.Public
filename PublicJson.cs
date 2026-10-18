using Newtonsoft.Json;

namespace PayGram.Public
{
	/// <summary>
	/// The json format of the ToJson methods of the public types: indented, default values left out
	/// (DefaultValueHandling.IgnoreAndPopulate). It is what PayGram stores (requests, responses, callbacks, withdraw methods)
	/// and what the requests sent to the PayGram api look like, so it must not change.
	/// It mirrors JsonDefaults.Storage of the internal Utilities library, which this public sdk does not depend on.
	/// </summary>
	internal static class PublicJson
	{
		internal static string ToStoredJson(object value)
			=> JsonConvert.SerializeObject(value, Formatting.Indented,
				new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate });
	}
}

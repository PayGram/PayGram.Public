using Newtonsoft.Json;

namespace PayGram.Types
{
    public class PaygramRequest : IPaygramRequest
    {

        public Int64 RequestId { get; set; }
        /// <summary>
        /// The UserId at the client side that originated this request
        /// </summary>
        public string UserCliId { get; set; }


        public PaygramRequest()
        {
            RequestId = 0;
        }

        public PaygramRequest(Int64 id)
        {
            this.RequestId = id;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented,
                                                    new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate });
        }
    }
}

using PayGram.Public;

namespace PayGram.Types
{
    public class ResponseUserInfo : PaygramResponse
    {
        /// <summary>
        /// The PG$ balance
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// The UTC date and time when this user joined
        /// </summary>
        public DateTime JoinedOn { get; set; }
        /// <summary>
        /// The id in the client's system
        /// </summary>
        public string UserIdInClient { get; set; }
        /// <summary>
        /// The Unique Identifier in the Paygram System
        /// </summary>
        public Guid UID { get; set; }
        /// <summary>
        /// The Callback Url to call when an update is made to this user
        /// </summary>
        public string CallbackUrl { get; set; }

        public ResponseUserInfo()
                                : base((PaygramResponseTypes)Enum.Parse(typeof(PaygramResponseTypes), nameof(ResponseUserInfo)))
        {

        }
    }
}

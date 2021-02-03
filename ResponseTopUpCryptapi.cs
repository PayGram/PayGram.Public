using PayGram.Public;
namespace PayGram.Types
{
	public class ResponseTopUpCryptapi : ResponseTopUp
	{
		public string SendToAddress { get; set; }
		public ResponseTopUpCryptapi() : base()
		{
			Type = PaygramResponseTypes.ResponseTopUpCryptapi;
		}
		public ResponseTopUpCryptapi(string sendToAddress)
		{
			this.SendToAddress = sendToAddress;
			Type = PaygramResponseTypes.ResponseTopUpCryptapi;
		}
	}
}

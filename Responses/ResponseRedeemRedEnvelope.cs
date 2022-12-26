namespace PayGram.Public.Responses
{
	public class ResponseRedeemRedEnvelope : ResponseTopUpReceived
	{
		public ResponseRedeemRedEnvelope()
		{
			Type = PaygramResponseTypes.ResponseRedeemRedEnvelope;
		}
	}
}

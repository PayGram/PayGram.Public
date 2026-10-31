namespace PayGram.Public.Responses
{
	public class ResponseCreateRedEnvelope : PaygramResponse
	{
		public Guid InvoiceCode { get; set; }
		public string? FriendlyVoucherCode { get; set; }
		public decimal Fees { get; set; }
		/// <summary>
		/// The link to take a share of the envelope. Filled only by the PayGram bot API (the one
		/// <see cref="PayGram.Public.Client.PayGramBotClient"/> calls), with a link to the PayGram Telegram bot. The PayGram API
		/// leaves it empty for its own integrators on purpose: it does not depend on Telegram, and where an envelope is redeemed
		/// from is the integrator's business (redeem the code with RedeemVoucher).
		/// </summary>
		public string RedeemUrl { get; set; }
		public decimal Amount { get; set; }

		public ResponseCreateRedEnvelope() : base(PaygramResponseTypes.ResponseCreateRedEnvelope)
		{

		}
		public ResponseCreateRedEnvelope(string error) : base(PaygramResponseTypes.ResponseCreateRedEnvelope, ResponseCodes.ResponseGenericError)
		{
			Message = error;
		}
	}
}

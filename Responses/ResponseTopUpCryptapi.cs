﻿namespace PayGram.Public.Responses
{
	public class ResponseTopUpCryptapi : ResponseTopUp
	{
		public string? SendToAddress { get; set; }
		public ResponseTopUpCryptapi() : base()
		{
			Type = PaygramResponseTypes.ResponseTopUpCryptapi;
		}
		public ResponseTopUpCryptapi(string sendToAddress)
		{
			SendToAddress = sendToAddress;
			Type = PaygramResponseTypes.ResponseTopUpCryptapi;
		}
	}
}

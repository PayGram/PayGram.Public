using CurrenciesLib;
using PayGram.Public.UserAPI;
using System;
using System.Collections.Generic; 

namespace PayGram.Public
{
	public class ResponseGetUpdates : PaygramResponse
	{
		public List<UserCallbackInfo> Updates { get; set; }

		public ResponseGetUpdates()
			: base(PaygramResponseTypes.ResponseGetExchangeRates)
		{
			Updates = new List<UserCallbackInfo>();
		}
	}
}

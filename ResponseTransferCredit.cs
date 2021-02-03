using PayGram.Public;
using System;

namespace PayGram.Types
{
	public class ResponseTransferCredit : PaygramResponse
	{

		public ResponseTransferCredit()
						: base((PaygramResponseTypes)Enum.Parse(typeof(PaygramResponseTypes), nameof(ResponseTransferCredit)))
		{
		}

		public decimal FromCredit { get; set; }
		public decimal ToCredit { get; set; }
		public Guid VoucherCode { get; set; }
	}
}

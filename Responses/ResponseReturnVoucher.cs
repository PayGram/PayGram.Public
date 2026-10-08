using CurrenciesLib;
using System;

namespace PayGram.Public.Responses
{
	/// <summary>
	/// Admin-only: the outcome of closing an open voucher or red envelope on behalf of its sender,
	/// the money going back to the sender's balance (the same path as a recall by the sender).
	/// </summary>
	public class ResponseReturnVoucher : PaygramResponse
	{
		public Guid InvoiceCode { get; set; }
		public InvoiceTypes InvoiceType { get; set; }
		/// <summary>What went back to the sender</summary>
		public decimal Returned { get; set; }
		public Currencies Currency { get; set; }
		/// <summary>The sender's balance in that currency afterwards</summary>
		public decimal NewBalance { get; set; }

		public ResponseReturnVoucher() : base(PaygramResponseTypes.ResponseReturnVoucher) { }
		public ResponseReturnVoucher(ResponseCodes code) : base(PaygramResponseTypes.ResponseReturnVoucher, code) { }
		public ResponseReturnVoucher(string msg, ResponseCodes code) : base(msg, PaygramResponseTypes.ResponseReturnVoucher, code) { }
	}
}

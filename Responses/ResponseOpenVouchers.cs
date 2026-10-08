using CurrenciesLib;
using System;
using System.Collections.Generic;

namespace PayGram.Public.Responses
{
	/// <summary>A voucher or red envelope sent by a user and still open: the money is parked on the system until someone redeems it or it is returned</summary>
	public class OpenVoucher
	{
		public Guid InvoiceCode { get; set; }
		public string? FriendlyVoucherCode { get; set; }
		/// <summary>Voucher, General or RedEnvInvoice</summary>
		public InvoiceTypes InvoiceType { get; set; }
		public DateTime CreatedUtc { get; set; }
		/// <summary>The amount the sender put in</summary>
		public decimal Amount { get; set; }
		public decimal Fees { get; set; }
		/// <summary>What is still unclaimed: the whole net amount for a voucher, the unredeemed part for a red envelope</summary>
		public decimal Remaining { get; set; }
		public Currencies Currency { get; set; }
		public string? CallbackData { get; set; }
		/// <summary>Red envelopes: how many redeemed so far and the maximum</summary>
		public int Redeemers { get; set; }
		public int MaxRedeemers { get; set; }
	}

	/// <summary>Admin-only: the open vouchers and red envelopes of a user of the calling client</summary>
	public class ResponseOpenVouchers : PaygramResponse
	{
		public List<OpenVoucher> Vouchers { get; set; } = new();

		public ResponseOpenVouchers() : base(PaygramResponseTypes.ResponseOpenVouchers) { }
		public ResponseOpenVouchers(ResponseCodes code) : base(PaygramResponseTypes.ResponseOpenVouchers, code) { }
		public ResponseOpenVouchers(string msg, ResponseCodes code) : base(msg, PaygramResponseTypes.ResponseOpenVouchers, code) { }
	}
}

namespace PayGram.Public.Requests
{
	/// <summary>
	/// Request body for <c>InvoiceInfoV2</c>.
	/// Looks up details of an invoice by its GUID or human-readable voucher code.
	/// </summary>
	public class InvoiceInfoRequest
	{
		/// <summary>The invoice GUID. Use <see cref="Guid.Empty"/> when looking up by voucher code instead.</summary>
		public Guid InvGuid { get; set; }
		/// <summary>Human-readable voucher code (e.g. "ABC-1234"). Optional if InvGuid is provided.</summary>
		public string? VoucherCode { get; set; }
	}
}

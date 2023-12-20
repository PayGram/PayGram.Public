using System.Reflection.PortableExecutable;
using System.Text;

namespace PayGram.Public
{
	public record Invoice(Guid Guid, string? FriendlyVoucherCode)
	{
		/// <summary>
		/// The length of a friendly voucher code without spaces
		/// </summary>
		public const int FRIENDLY_CODE_LEN = 12;
		public string FriendlyFormatted => GetFriendlyFormat(FriendlyVoucherCode);
		static public string? GetFriendlyFormat(string? code)
		{
			if (code == null) return null;
			code = code.Replace(" ", "");
			if (string.IsNullOrEmpty(code) || code.Length < FRIENDLY_CODE_LEN) return code;
			StringBuilder sb = new();
			sb.Append(code.Take(3).ToArray());
			sb.Append(" ");
			sb.Append(code.Skip(3).Take(3).ToArray());
			sb.Append(" ");
			sb.Append(code.Skip(6).Take(2).ToArray());
			sb.Append(" ");
			sb.Append(code.Skip(8).Take(2).ToArray());
			sb.Append(" ");
			sb.Append(code.Skip(10).Take(code.Length - 10).ToArray());
			return sb.ToString().ToUpper();
		}
	}
}

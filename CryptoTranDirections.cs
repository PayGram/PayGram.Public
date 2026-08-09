namespace PayGram.Public
{
	/// <summary>
	/// Direction of an on-chain transaction recorded in CryptoTransactions.
	/// In = a deposit into the platform, Out = a withdrawal out of the platform.
	/// </summary>
	public enum CryptoTranDirections
	{
		In = 0,
		Out = 1,
	}
}

namespace PayGram.Public
{
	public enum P2PGroupType
	{
		None = 0,
		/// <summary>
		/// Trader will buy a crypto from the seller. 
		/// Seller will sell a crypto to the trader for fiat
		/// </summary>
		Buy = 1,
		/// <summary>
		/// Trader will sell a crypto to the buyer.
		/// Buyer will buy a crypto from the trader paying fiat
		/// </summary>
		Sell = 2,
	}

}

namespace PayGram.Public.Responses
{
	/// <summary>
	/// Classifies each <see cref="StatementEntry"/> by the kind of ledger event it represents.
	/// </summary>
	public enum StatementEntryType
	{
		None = 0,
		/// <summary>Crypto or fiat deposit credited to the user's wallet.</summary>
		Deposit = 1,
		/// <summary>Crypto or fiat withdrawal debited from the user's wallet.</summary>
		Withdrawal = 2,
		/// <summary>Refund of a previously failed or cancelled withdrawal.</summary>
		WithdrawalRefund = 3,
		/// <summary>Outgoing voucher/invoice transfer (sender side, redeemed by recipient).</summary>
		OutgoingTransfer = 4,
		/// <summary>Outgoing direct (peer-to-peer) transfer (sender side, instantly settled).</summary>
		OutgoingDirectTransfer = 5,
		/// <summary>Incoming voucher/invoice transfer (recipient side).</summary>
		IncomingTransfer = 6,
		/// <summary>Incoming direct (peer-to-peer) transfer (recipient side).</summary>
		IncomingDirectTransfer = 7,
		/// <summary>Sell side of a currency swap or conversion.</summary>
		ChangeSell = 8,
		/// <summary>Buy side of a currency swap or conversion.</summary>
		ChangeBuy = 9,
		/// <summary>Fee entry (PayGram platform fee, network fee, etc.).</summary>
		Fee = 10,
		/// <summary>Any other entry type not covered above.</summary>
		Other = 11
	}
}

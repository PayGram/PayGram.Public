namespace PayGram.Public
{
	public enum PaygramResponseTypes
	{
		Unknown = 0,
		ResponseClientInfo = 3,
		ResponseSetCallbackApi = 4,
		ResponseCreateClient = 5,
		ResponseInvoiceInfo = 6,
		ResponseTopUp = 7,
		ResponseTopUpReceived = 8,
		ResponseTransferCredit = 9,
		/// <summary>The answer of UpdateWithdraw, removed (a withdraw cannot be cancelled or changed): kept so the value is never reused</summary>
		ResponseWithdrawUpdated = 10,
		ResponseUserInfo = 11,
		ResponseWithdrawAccepted = 12,
		ResponseInvoiceWithdrawInfo = 13,
		ResponseIssueInvoice = 14,
		ResponseTopUpCryptapi = 15,
		ResponseCirculatingCoins = 16,
		ResponseGetExchangeRates = 17,
		ResponseGetUpdates = 18,
		ResponseConvert = 19,
		ResponseSwap = 20,
		ResponseStatement = 21,
		ResponseCreateRedEnvelope = 22,
		//ResponseSimplex = 23,
		ResponseRedeemRedEnvelope = 24,
		ResponseManageRedEnvelope = 25,
		ResponseRedEnvelopeInvoiceInfo = 26,
		ResponseP2PGroupList = 27,
		ResponseSetP2PGroup = 28,
		ResponseSetBusiness = 29,
		ResponseGetBusiness = 30,
		ResponseGetInvoices = 31,
		ResponseSetRate = 32,
		ResponseWhitelistAddresses = 33,
		ResponseReportInfo = 34,
		ResponseCancelPendingCallbacks = 35,
		ResponseBlockUser = 36,
		ResponseFundFlow = 37,
		ResponseTransactionDetail = 38,
		ResponseUserProfile = 39,
		ResponseOpenVouchers = 40,
		ResponseReturnVoucher = 41,
		ResponseCheckUnique = 42,
		ResponseReturnAllVouchers = 43,
		ResponseUniqueRequest = 44,
		ResponseCheckReference = 45,
		/// <summary>Admin: a page of withdrawals with the queue summary (AdminListWithdrawals)</summary>
		ResponseAdminWithdrawals = 46,
		/// <summary>Admin: one withdrawal with its events, rule checks and context (AdminGetWithdrawal)</summary>
		ResponseAdminWithdrawal = 47,
		/// <summary>Admin: the outcome of an action on a withdrawal (approve, reject, mark sent, release)</summary>
		ResponseWithdrawAction = 48,
		/// <summary>Admin: the rules of the withdrawal gate (AdminGetWithdrawRules / AdminSetWithdrawRules)</summary>
		ResponseWithdrawRules = 49,
		/// <summary>Admin: what a code, hash, address or key is (AdminSearch, the global search of the admin panel)</summary>
		ResponseAdminSearch = 50,
		/// <summary>Admin: the outcome of a ban or unban of a user (AdminSetUserBanned, the admin panel only)</summary>
		ResponseAdminSetUserBanned = 51,
		/// <summary>Admin: a page of the compliance alerts of the deposit watch (AdminListComplianceAlerts, the admin panel only)</summary>
		ResponseAdminComplianceAlerts = 52,
		/// <summary>Admin: the outcome of the close of a compliance alert, with or without the unfreeze of its user (AdminCloseComplianceAlert, the admin panel only)</summary>
		ResponseAdminComplianceAlertAction = 53,
		/// <summary>Admin: the outgoing freeze of a user after a change (AdminSetOutgoingFreeze, the admin panel only)</summary>
		ResponseAdminOutgoingFreeze = 54,
		/// <summary>Admin: the users whose outgoing operations are frozen, a page of them (AdminListFrozenUsers, the admin panel only)</summary>
		ResponseAdminFrozenUsers = 55,
		/// <summary>Admin: one compliance alert by its id (AdminGetComplianceAlert, the admin panel only)</summary>
		ResponseAdminComplianceAlert = 56,
		/// <summary>The currencies a payer can pay a payment request with, through an automatic swap (QuotePayInvoice)</summary>
		ResponsePayQuote = 57,
	}
}

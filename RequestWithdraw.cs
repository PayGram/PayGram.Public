﻿using CurrenciesLib;
using PayGram.Public.UserAPI;

namespace PayGram.Types
{
    public class RequestWithdraw : PaygramRequest
    {
        /// <summary>
        /// The provider that will handle this transaction. Currently only <seealso cref="PayProvider.DEFAULT_INTERNAL_TRANSFER_PROVIDERID"/> is supported for withdraw.
        /// </summary>
        public int Provider { get; set; }
        /// <summary>
        /// The currency that will be used to pay for the withdraw
        /// </summary>
        public Currencies CurrencyCode { get; set; }
        /// <summary>
        /// The amount to withdraw from the currency-account
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// The data to send back to the client or to the user when the Withdraw is executed
        /// </summary>
        public string CallbackData { get; set; }
        /// <summary>
        /// Represents where the funds should be transferred
        /// </summary>
        public WithdrawMethod WithdrawMethod { get; set; }

        public RequestWithdraw()
        {
        }
        public override string ToString()
        {
            return $"{Provider}:{CurrencyCode}:{Amount}:{CallbackData}:{WithdrawMethod}";
        }
    }
}
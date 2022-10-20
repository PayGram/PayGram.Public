using CurrenciesLib;
using CurrenciesLib.Cryptos;
using CurrenciesLib.Fiats;
using Newtonsoft.Json;
using System.Text;

namespace PayGram.Public.UserAPI
{
    public class WithdrawMethod
    {
        /// <summary>
        ///  Optional. The Full name of the bank account holder. 
        /// </summary>
        public string? BeneficiaryAccountFullname { get; set; }
        /// <summary>
        /// The currency that the user wants to use to pay and wants to receive. See <see cref="Currencies"/>.
        /// </summary>
        public Currencies CurrencyCode { get; set; }
        /// <summary>
        /// When the withdraw method is a crypto, this will be the netowrk
        /// </summary>
        public CryptoNetworks Network { get; set; }
        /// <summary>
        /// Optional. The cryptocurrency address where the funds should be transferred to 
        /// </summary>
        public string? CryptoAddress { get; set; }
        /// <summary>
        /// Optional. If the transfer is made to a bank account which has the IBAN number, this field is mandatory.
        /// </summary>
        public string? BankIban { get; set; }
        /// <summary>
        /// Optional. If the transfer is made to a bank account which has the Bic or the SWIFT code, this field is mandatory.
        /// </summary>
        public string? BicSwift { get; set; }
        /// <summary>
        /// Optional. If the transfer is made to a bank account which doesn't have the IBAN number, this field is mandatory.
        /// </summary>
        public string? BankAccount { get; set; }
        /// <summary>
        /// Optional. If the transfer is made to a bank account which has the Routing Code (USA banks), this field is mandatory.
        /// </summary>
        public string? BankRoutingNumber { get; set; }
        /// <summary>
        /// Optional. If the transfer is made to a bank account which has the Sort Code (UK banks), this field is mandatory.
        /// </summary>
        public string? BankSortCode { get; set; }
        /// <summary>
        /// Optional, but adviced when the transfer is made to a bank account.
        /// </summary>
        public GeoAddress? BankAddress { get; set; }
        /// <summary>
        /// Optional, but adviced when the transfer is made to a bank account.
        /// </summary>
        public GeoAddress? BeneficiaryAddress { get; set; }

        public bool IsValidCrypto
        {
            get
            {
                var c = Crypto.GetBySymbol(CurrencyCode); //Crypto is right, because it's a subset of Currencies
                return c != null && c.CurrencyId != Currencies.UNKNOWN && c.CurrencyType == CurrencyTypes.Crypto && string.IsNullOrWhiteSpace(CryptoAddress) == false;
            }
        }

        public bool IsValidBankAccount
        {
            get
            {
                var f = FiatCurrency.GetBySymbol(CurrencyCode);
                return (string.IsNullOrWhiteSpace(BankAccount) == false || string.IsNullOrWhiteSpace(BankIban) == false)
                        && string.IsNullOrWhiteSpace(BeneficiaryAccountFullname) == false
                        && f != null && f.CurrencyId != Currencies.UNKNOWN && f.CurrencyType == CurrencyTypes.Fiat
                        && ((string.IsNullOrWhiteSpace(BankIban) == false && string.IsNullOrWhiteSpace(BicSwift) == false)
                            || (string.IsNullOrWhiteSpace(BankAccount) == false && string.IsNullOrWhiteSpace(BankSortCode) == false)
                            || (string.IsNullOrWhiteSpace(BankAccount) == false && string.IsNullOrWhiteSpace(BankRoutingNumber) == false)
                            );
            }
        }

        public bool IsValid => IsValidCrypto || IsValidBankAccount;

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented,
                                                    new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate });
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            if (IsValidCrypto)
            {
                sb.AppendLine($"Currency: {CurrencyCode} {Network}");
                sb.AppendLine("Address:");
                sb.AppendLine(CryptoAddress);
            }
            else //if (IsValidBankAccount)
            {
                sb.AppendLine("Currency:");
                sb.AppendLine(CurrencyCode.ToString());
                if (string.IsNullOrWhiteSpace(BeneficiaryAccountFullname) == false)
                {
                    sb.AppendLine("Beneficiary full name:");
                    sb.AppendLine(BeneficiaryAccountFullname);
                }
                if (string.IsNullOrWhiteSpace(BankIban) == false)
                {
                    sb.AppendLine("IBAN:");
                    sb.AppendLine(BankIban);
                }
                if (string.IsNullOrWhiteSpace(BankAccount) == false)
                {
                    sb.AppendLine("Bank Account:");
                    sb.AppendLine(BankAccount);
                }
                if (string.IsNullOrWhiteSpace(BicSwift) == false)
                {
                    sb.AppendLine("Bic/Swift:");
                    sb.AppendLine(BicSwift);
                }
                if (string.IsNullOrWhiteSpace(BankRoutingNumber) == false)
                {
                    sb.AppendLine("Routing Number:");
                    sb.AppendLine(BankRoutingNumber);
                }
                if (string.IsNullOrWhiteSpace(BankSortCode) == false)
                {
                    sb.AppendLine("Sort Code:");
                    sb.AppendLine(BankSortCode);
                }
                if (BankAddress != null)
                {
                    sb.AppendLine("Bank Address:");
                    sb.AppendLine(BankAddress.ToString());
                }
                if (BeneficiaryAddress != null)
                {
                    sb.AppendLine("Beneficiary address:");
                    sb.AppendLine(BeneficiaryAddress.ToString());
                }
            }
            return sb.ToString();
        }
    }
}
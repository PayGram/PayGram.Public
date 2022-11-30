using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PayGram.Public
{
    public class ResponseSimplex : PaygramResponse
    {
        public string PaymentUrl { get; set; }
        public string PaymentID { get; set; }
        public SimplexQuoteResponse SimplexQuoteResponse { get; set; }
        public SimplexPaymentResponse SimplexPaymentResponse { get; set; }
        public ResponseSimplex() : base(PaygramResponseTypes.ResponseSimplex)
        {

        }
    }
}

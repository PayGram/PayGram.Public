namespace PayGram.Public.Client
{
    public class ResponseConvert : PaygramResponse
    {
        public decimal Result { get; set; }

        public ResponseConvert(decimal result) : base()
        {
            Result = result;
            Success = true;
            Type = PaygramResponseTypes.ResponseConvert;
        }
    }
}

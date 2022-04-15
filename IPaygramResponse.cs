namespace PayGram.Public
{
    public interface IPaygramResponse
    {
        PaygramResponseTypes Type { get; set; }
        bool Success { get; set; }
        string Message { get; set; }
    }
}

namespace CorridaAPI.Model.Authentication
{
    public class TokenModel
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}

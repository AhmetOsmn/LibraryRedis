namespace Entities.DTOs
{
    public class TokenOptionsDTO
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Token { get; set; }
        public int AccessTokenExpiration { get; set; }
    }
}

namespace ElideusDotNetFramework.Core
{
    public class TokenData
    {
        public required string Token { get; set; }

        public required DateTime ExpirationDateTime { get; set; }
    }
}

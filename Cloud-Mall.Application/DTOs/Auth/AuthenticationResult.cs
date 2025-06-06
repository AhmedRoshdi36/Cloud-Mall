namespace Cloud_Mall.Application.DTOs.Auth
{
    public class AuthenticationResult
    {
        public bool Succeeded { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}

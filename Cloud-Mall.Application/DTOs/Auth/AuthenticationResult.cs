namespace Cloud_Mall.Application.DTOs.Auth
{
    public class AuthenticationResult
    {
        public bool Succeeded { get; set; }
        public string Token { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public IEnumerable<string> Errors { get; set; }= new List<string>();
    }
}

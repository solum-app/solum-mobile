namespace Solum.Models
{
    public class RegisterBinding
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string CidadeId { get; set; }
    }

    public class LoginBinding
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public static string GrantType { get; } = "password";
        public bool IsValid { get { return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password); } }
    }

    public class RefreshTokenBinding
    {
        public string RefreshToken { get; set; }
        public static string GrantType { get; } = "refresh_token";
    }
}
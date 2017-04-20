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
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
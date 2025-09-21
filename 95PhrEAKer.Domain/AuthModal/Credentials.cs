using System.ComponentModel.DataAnnotations;
namespace _95PhrEAKer.Domain.AuthModal
{
    public class Credentials
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

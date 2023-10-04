using System.ComponentModel.DataAnnotations;

namespace WealthBridge.Core.Models
{
    public class AuthenticateRequest
    {
        public string Token { get; set; }
    }

    public class SignUpRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }

        public string Phone { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WealthBridge.Model
{
    public class TokenModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace TimonIdentityServer.Tos
{
    public class RefreshTokenTo
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}



using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    public class LogoutRequestDTO
    {
        [Required(ErrorMessage = "SessionId is required.")]
        public string SessionId { get; set; } = null!;
    }
}

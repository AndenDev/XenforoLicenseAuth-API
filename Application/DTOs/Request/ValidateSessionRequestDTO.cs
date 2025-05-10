
using System.ComponentModel.DataAnnotations;


namespace Application.DTOs.Request
{
    public class ValidateSessionRequestDTO
    {
        [Required(ErrorMessage = "SessionId is required.")]
        public string SessionId { get; set; } = null!;

        [Required(ErrorMessage = "IP Address is required.")]
        [RegularExpression(@"^\d{1,3}(\.\d{1,3}){3}$", ErrorMessage = "Invalid IP address format.")]
        public string IPAddress { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "IP Address is required.")]
        [RegularExpression(@"^\d{1,3}(\.\d{1,3}){3}$", ErrorMessage = "Invalid IP address format.")]
        public string IPAddress { get; set; }
    }
}
    
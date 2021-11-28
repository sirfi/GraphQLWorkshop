using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLWorkshop.ViewModels.Register
{
    public class RegisterRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
    }
}

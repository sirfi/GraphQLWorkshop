using System.ComponentModel.DataAnnotations;

namespace GraphQLWorkshop.ViewModels.Login
{
    public class LoginRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

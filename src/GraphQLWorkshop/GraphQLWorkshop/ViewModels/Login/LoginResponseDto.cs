using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLWorkshop.ViewModels.Login
{
    public class LoginResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public UserDto User { get; set; }
    }

    public class UserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }

    }
}

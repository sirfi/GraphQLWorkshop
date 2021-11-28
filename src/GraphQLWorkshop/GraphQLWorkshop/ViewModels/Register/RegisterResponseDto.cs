using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLWorkshop.ViewModels.Login;

namespace GraphQLWorkshop.ViewModels.Register
{
    public class RegisterResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public UserDto User { get; set; }
    }
}

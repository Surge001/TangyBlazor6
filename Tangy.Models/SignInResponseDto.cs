using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy.Models
{
    public class SignInResponseDto
    {
        public bool IsAuthSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public string Token { get; set; }

        public UserDto UserDto { get; set; }
    }
}

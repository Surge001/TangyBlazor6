using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy.Models
{
    public class SignupResponseDto
    {
        public bool IsRegistrationSuccessful { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}

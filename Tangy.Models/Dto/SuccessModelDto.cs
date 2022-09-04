using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy.Models.Dto
{
    public class SuccessModelDto
    {
        public int StatusCode { get; set; }

        public string SuccessMessage { get; set; }

        public object Data { get; set; }
    }
}

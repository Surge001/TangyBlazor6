using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy.DataAccess;

namespace Tangy.Models.Dto
{
    public class OrderDto
    {
        public OrderHeaderDto OrderHeader { get; set; }

        public List<OrderDetailDto> OrderDetails { get; set; }
    }
}

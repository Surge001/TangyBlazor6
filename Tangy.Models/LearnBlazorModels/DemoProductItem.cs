using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy.Models.LearnBlazorModels
{
    public class DemoProductItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets price
        /// </summary>
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        public List<DemoProductProperties> Properties { get; set; }

    }
}

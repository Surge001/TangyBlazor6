using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tangy.Models.Dto;

namespace Tangy.DataAccess
{
    public class OrderDetailDto
    {
        public int Id { get; set; }

        [Required]
        public int OrderHeaderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [NotMapped]
        public virtual ProductDto Product { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string Size { get; set; }
        
        [Required]
        public string ProductName { get; set; }
    }
}

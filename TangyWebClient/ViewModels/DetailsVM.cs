using System.ComponentModel.DataAnnotations;
using Tangy.Models.Dto;

namespace TangyWebClient.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            this.ProductPrice = new();
            Count = 1;
        }
        [Range(1, int.MaxValue, ErrorMessage ="Please enter a value greater than 0")]
        public int Count { get; set; }

        [Required]
        public int SelectedProductPriceId { get; set; }

        public ProductPriceDto ProductPrice { get; set; }
    }
}

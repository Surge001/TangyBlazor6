using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Tangy.DataAccess
{
    public class OrderHeaderDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Order Total")]
        public double OrderTotal { get; set; }

        [Required]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Required]
        [Display(Name = "Shipping Date")]
        public DateTime ShippingDate { get; set; }

        [Required]
        public string Status { get; set; }

        public string? SessionId { get; set; }

        public string? PaymentIntentId { get; set; }

        // Order details:
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }


        [Required]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "PostalCode")]
        public string PostalCode { get; set; }

        [Display(Name ="Email")]
        [Required]
        public string Email { get; set; }
    }
}

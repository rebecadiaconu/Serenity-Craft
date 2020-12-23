using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Serenity_Craft.Models;

namespace Serenity_Craft_Library.Models
{
    public class Address
    {
        [Key]
        [ForeignKey("Delivery")]
        public int DeliveryId { get; set; }

        [Required,
         MinLength(4, ErrorMessage = "Your county name must have at least 4 characters.")]
        public string County { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string StreetNumber { get; set; }

        // -- navigation properties --

        // one-to-one relationship
        public virtual Delivery Delivery { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Serenity_Craft.Models;

namespace Serenity_Craft_Library.Models
{
    public class Delivery
    {
        [Key]
        public int DeliveryId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }

        public string PaymentMethod { get; set; }

        [CreditCard]
        public int CardNumber { get; set; }

        [RegularExpression(@"[0-9]+\.[0-9]*")]
        public double Total { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserName { get; set; }

        // -- navigation properties --

        // one-to-one relationship
        public virtual Address Address { get; set; }

        // one-to-many
        public virtual ICollection<InfoDelivery> InfoDeliveries { get; set; }
    }
}
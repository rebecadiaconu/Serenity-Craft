using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Serenity_Craft.Models
{
    public class PublisherContactInfo
    {
        [Required,
         MaxLength(40, ErrorMessage = "Publisher name too long.")]
        public string Name { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

    }
}
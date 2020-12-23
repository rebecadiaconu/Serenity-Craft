using Serenity_Craft.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Serenity_Craft_Library.Models
{
    public class Contact
    {
        [Key]
        [ForeignKey("Publisher")]
        public int PublisherId { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        // -- navigation properties --

        // one-to-one relationship
        public virtual Publisher Publisher { get; set; }
    }
}
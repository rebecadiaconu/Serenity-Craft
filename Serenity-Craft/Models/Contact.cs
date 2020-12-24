using Serenity_Craft.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Serenity_Craft.Models
{
    public class Contact
    {
        [Key]
        [ForeignKey("Publisher")]
        public int PublisherId { get; set; }

        [RegularExpression(@"^0(\d{9})$", ErrorMessage = "This is not a valid phone number!")]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        // -- navigation properties --

        // one-to-one relationship
        public virtual Publisher Publisher { get; set; }
    }
}
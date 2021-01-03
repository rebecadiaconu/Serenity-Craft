using Serenity_Craft.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Serenity_Craft.Models.MyValidations;

namespace Serenity_Craft.Models
{
    public class Contact
    {
        [Key]
        [ForeignKey("Publisher")] 
        public int PublisherId { get; set; }

        [PhoneNumberValidation]
        public string PhoneNumber { get; set; }

        [EmailValidation] 
        public string Email { get; set; }

        // -- navigation properties --

        // one-to-one relationship
        public virtual Publisher Publisher { get; set; }
    }
}
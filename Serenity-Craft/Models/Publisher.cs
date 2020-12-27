using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Serenity_Craft.Models
{
    public class Publisher
    {
        [Key]
        public int PublisherId { get; set; }

        [Required,
         MaxLength(40, ErrorMessage = "Publisher name too long.")]
        public string Name { get; set; }

        // -- navigation properties --

        // one-to-one relationship
        public virtual Contact Contact { get; set; }

        // one-to-many
        public ICollection<Book> Books { get; set; }
    }
}
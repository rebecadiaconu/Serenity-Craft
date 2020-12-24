using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Serenity_Craft.Models
{
    public class BookType
    {
        [Key]
        public int BookTypeId { get; set; }

        [Required,
         MaxLength(30, ErrorMessage = "Book type name too long.")]
        public string Name { get; set; }

        // -- navigation properties --

        // one-to-many 
        public ICollection<Book> Books { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Serenity_Craft.Models
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }

        [Required,
         MaxLength(20, ErrorMessage = "Genre name too long.")]
        public string Name { get; set; }

        // -- navigation properties --

        // many-to-many relationship
        public ICollection<Book> Books { get; set; }
    }
}
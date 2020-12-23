using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Serenity_Craft.Models;

namespace Serenity_Craft_Library.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required,
         MinLength(2, ErrorMessage = "The book title must have at least 2 characters."),
         MaxLength(50, ErrorMessage = "This title seems to be too long... chop it off a little.")]
        public string Title { get; set; }

        [Required,
         MinLength(5, ErrorMessage = "The author name must have at least 5 characters."),
         MaxLength(50, ErrorMessage = "This name seems to be too long... chop it off a little.")]
        public string Author { get; set; }

        [Required,
         RegularExpression("[0-9]+")]
        public int Pages { get; set; }

        [MaxLength(600, ErrorMessage = "Summary way too long, try to make it shorter!")]
        public string Summary { get; set; }


        //public string ImageFileName { get; set; }

        [Required,
         RegularExpression("[0-9]+")]
        public double Price { get; set; }

        public double BookRating { get; set; }

        [RegularExpression("[0-9]+")]
        public int InStock { get; set; }

        // --------------------------

        public int PublisherId { get; set; }
        public int BookTypeId { get; set; }


        // -- navigation properties --

        // one-to-many relationship
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<InfoDelivery> InfoDeliveries { get; set; }

        // many-to-one relationship
        public virtual Publisher Publisher { get; set; }
        public virtual BookType BookType { get; set; }

        // many-to-many relationship
        public virtual ICollection<Genre> Genres { get; set; }
    }
}
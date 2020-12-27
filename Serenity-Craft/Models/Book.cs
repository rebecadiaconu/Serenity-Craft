using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Serenity_Craft.Models;

namespace Serenity_Craft.Models
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
         RegularExpression(@"\d+(?:\.\d+)?")]
        public int Pages { get; set; }

        [MaxLength(600, ErrorMessage = "Summary way too long, try to make it shorter!")]
        public string Summary { get; set; }

        //[Required]
        //public string ImagePath { get; set; }

        [Required,
         RegularExpression(@"\d+(?:\.\d+)?")]
        public double Price { get; set; }


        private double _bookRating;
        public double BookRating
        {
            get => _bookRating;
            set => value = CalculateBookRating();
        }

        // dropdown lists

        [NotMapped]
        public IEnumerable<SelectListItem> PublishersList { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> BookTypesList { get; set; }

        // checkbox lists

        [NotMapped]
        public List<CheckBoxModel> GenresList { get; set; }

        // --------------------------

        [Required]
        public int PublisherId { get; set; }

        [Required]
        public int BookTypeId { get; set; }


        // -- navigation properties --

        // one-to-many relationship
        public virtual ICollection<Review> Reviews { get; set; }

        // many-to-one relationship
        public virtual Publisher Publisher { get; set; }
        public virtual BookType BookType { get; set; }

        // many-to-many relationship
        public virtual ICollection<Genre> Genres { get; set; }


        private double CalculateBookRating()
        {
            double rating = 0;

            if (Reviews == null || Reviews.Count == 0) return rating;

            int sum = 0;

            foreach (var review in Reviews)
            {
                sum += review.Note;
            }
            rating = sum / Reviews.Count;

            return rating;
        }
    }
}
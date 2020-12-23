using System;
using System.ComponentModel.DataAnnotations;

namespace Serenity_Craft_Library.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        [MaxLength(300, ErrorMessage = "Your opinion matters to us, but try to make it shorter!")]
        public string Text { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReviewDate { get; set; }
        [Required]
        public int Note { get; set; }

        [Required]
        public string UserName { get; set; }

        // many-to-one relationship
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}
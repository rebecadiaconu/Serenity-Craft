using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Web;

namespace Serenity_Craft.Models
{
    public class GenreBooks
    {
        [Key]
        public int GenreBooksId { get; set; }

        [Required]
        public int GenreId { get; set; }

        [Required]
        public int BookId { get; set; }

        // -- navigation properties --
        public virtual Book Book { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
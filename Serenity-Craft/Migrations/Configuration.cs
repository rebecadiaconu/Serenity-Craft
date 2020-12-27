using System.Collections.Generic;
using System.Data.Entity.Validation;
using Serenity_Craft.Models;

namespace Serenity_Craft.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Serenity_Craft.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Serenity_Craft.Models.ApplicationDbContext context)
        {
            var bookTypes = new List<BookType>
            {
                new BookType {Name = "Comic-book"},
                new BookType {Name = "Dictionary"},
                new BookType {Name = "Encyclopedia"},
                new BookType {Name = "Guide"},
                new BookType {Name = "Journal"},
                new BookType {Name = "Manga"},
                new BookType {Name = "Memoir"},
                new BookType {Name = "Newspaper"},
                new BookType {Name = "Novel"},
                new BookType {Name = "Poetry"},
                new BookType {Name = "Short story"},
                new BookType {Name = "Theatre/Sketch"}
            };
            try
            {
                bookTypes.ForEach(bt => context.BookTypes.AddOrUpdate(p => p.Name, bt));
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            var contacts = new List<Contact>
            {
                new Contact
                {
                    PhoneNumber = "0234567801",
                    Email = "wiley@gmail.com"
                },
                new Contact
                {
                    PhoneNumber = "0756444200",
                    Email = "hlivre@gmail.com"
                },
                new Contact
                {
                    PhoneNumber = "0780266113",
                    Email = "collins_harper@yahoo.com"
                }
            };
            try
            {
                contacts.ForEach(c => context.Contacts.AddOrUpdate(p => p.PublisherId, c));
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            var publishers = new List<Publisher>
            {
                new Publisher {Name = "Hachette Livre"},
                new Publisher {Name = "Herper Collins"},
                new Publisher {Name = "Wiley"}
            };
            try
            {
                publishers.ForEach(pb => context.Publishers.AddOrUpdate(p => p.Name, pb));
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            var genres = new List<Genre>
            {
                new Genre {Name = "Adventure"},
                new Genre {Name = "Art/architecture"},
                new Genre {Name = "Autobiography"},
                new Genre {Name = "Biography"},
                new Genre {Name = "Drama"},
                new Genre {Name = "Fantasy"},
                new Genre {Name = "Horror"},
                new Genre {Name = "Humor"},
                new Genre {Name = "History"},
                new Genre {Name = "Romance"},
                new Genre {Name = "Science fiction"},
                new Genre {Name = "Suspense"},
                new Genre {Name = "Religion"},
                new Genre {Name = "Spirituality"},
                new Genre {Name = "Philosophy"},
                new Genre {Name = "Memoir"},
                new Genre {Name = "Science"},
                new Genre {Name = "Thriller"},
                new Genre {Name = "Travel"}
            };
            try
            {
                genres.ForEach(g => context.Genres.AddOrUpdate(p => p.Name, g));
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            var books = new List<Book>
            {
                new Book
                {
                    Title = "Origins",
                    Author = "Dan Brown",
                    Pages = 724,
                    Summary = "",
                    Price = 46,
                    BookRating = 0,
                    PublisherId = publishers.Single(p => p.Name == "Wiley").PublisherId,
                    BookTypeId = bookTypes.Single(bt => bt.Name == "Novel").BookTypeId,
                    Genres = new List<Genre>(),
                    Reviews = new List<Review>()

                },
                new Book
                {
                    Title = "Hamlet",
                    Author = "Shakespeare",
                    Pages = 375,
                    Summary = "",
                    Price = 50,
                    BookRating = 0,
                    PublisherId = publishers.Single(p => p.Name == "Herper Collins").PublisherId,
                    BookTypeId = bookTypes.Single(bt => bt.Name == "Novel").BookTypeId,
                    Genres = new List<Genre>(),
                    Reviews = new List<Review>()
                },
                new Book
                {
                    Title = "The Canterbury Tales",
                    Author = "Geoffrey Chaucer",
                    Pages = 602,
                    Summary = "",
                    Price = 30,
                    BookRating = 0,
                    PublisherId = publishers.Single(p => p.Name == "Hachette Livre").PublisherId,
                    BookTypeId = bookTypes.Single(bt => bt.Name == "Novel").BookTypeId,
                    Genres = new List<Genre>(),
                    Reviews = new List<Review>()
                }
            };
            try
            {
                books.ForEach(b => context.Books.AddOrUpdate(p => p.Title, b));
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            try
            {
                AddOrUpdateBook(context, "The Canterbury Tales", "Romance");
                AddOrUpdateBook(context, "The Canterbury Tales", "Fantasy");
                AddOrUpdateBook(context, "Hamlet", "Drama");
                AddOrUpdateBook(context, "Hamlet", "Philosophy");
                AddOrUpdateBook(context, "Origins", "Drama");

                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            void AddOrUpdateBook(ApplicationDbContext ctx, string bookTitle, string genreName)
            {
                var bk = ctx.Books.SingleOrDefault(b => b.Title == bookTitle);
                var gens = bk.Genres.SingleOrDefault(g => g.Name == genreName);
                if (gens == null)
                    bk.Genres.Add(ctx.Genres.Single(g => g.Name == genreName));
            }
        }
    }
}

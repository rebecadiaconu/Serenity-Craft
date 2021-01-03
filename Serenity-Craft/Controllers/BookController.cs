using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serenity_Craft.Models;

namespace Serenity_Craft.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // READ
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
           var books = db.Books.Include("Publisher").Include("BookType").OrderBy(s => s.Title); ;
            ViewBag.Books = books.ToList();

            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Book book = db.Books.Find(id);
                if (book != null)
                {
                    return View(book);
                }

                return HttpNotFound("Couldn't find the book!");
            }

            return HttpNotFound("Parameter is missing...");
        }

        // CREATE
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            Book book = new Book
            {
                BookTypesList = GetAllBookTypes(),
                PublishersList = GetAllPublishers(),
                GenresList = GetAllGenres(),
                Genres = new List<Genre>(),
                Reviews = new List<Review>()
            };

            ViewBag.Message = null;
            return View(book);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult New(Book bookReq)
        {
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(bookReq.ImageFile.FileName);

                string fileExtension = Path.GetExtension(bookReq.ImageFile.FileName);

                fileName = fileName.Trim() + fileExtension;

                string uploadPath = "C:/Users/Asus/Desktop/DAW/Serenity-Craft/Serenity-Craft/Content/books/" + fileName;

                bookReq.ImagePath = "/Content/books/" + fileName;

                bookReq.Publisher = db.Publishers.FirstOrDefault(p => p.PublisherId.Equals(bookReq.PublisherId));
                bookReq.BookType = db.BookTypes.FirstOrDefault(p => p.BookTypeId.Equals(bookReq.BookTypeId));

                bookReq.BookTypesList = GetAllBookTypes();
                bookReq.PublishersList = GetAllPublishers();

                var genreSelected = bookReq.GenresList.Where(b => b.Checked).ToList();

                bookReq.Genres = new List<Genre>();
                for (int i = 0; i < genreSelected.Count(); i++)
                {
                    Genre genre = db.Genres.Find(genreSelected[i].Id);
                    bookReq.Genres.Add(genre);
                }

                if (ModelState.IsValid)
                {
                    if (NotExists(bookReq) == -1)
                    {
                        bookReq.ImageFile.SaveAs(uploadPath);
                        db.Books.Add(bookReq);
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }

                    ViewBag.Message = "The book is already in our database!";
                    return View(bookReq);
                }

                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();
                Console.WriteLine(errors);

                ViewBag.Message = "Oh snap! Change a few things up and try submitting again.";
                return View(bookReq);
            }
            catch (Exception)
            {
                ViewBag.Message = "Something went wrong. Please try again!";
                return View(bookReq);
            }
        }

        // UPDATE
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Book book = db.Books.Find(id);
                if (book != null)
                {
                    book.GenresList = GetAllGenres();
                    book.BookTypesList = GetAllBookTypes();
                    book.PublishersList = GetAllPublishers();

                    foreach (Genre checkedGenre in book.Genres)
                    {
                        book.GenresList.FirstOrDefault(g => g.Id == checkedGenre.GenreId).Checked = true;
                    }

                    return View(book);
                }

                return HttpNotFound("Couldn't find the book.");
            }

            return HttpNotFound("Parameter is missing...");
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Book bookReq)
        {
            try
            {
                int uploaded = 0;

                if (bookReq.ImageFile != null)
                {
                    uploaded = 1;

                    string fileName = Path.GetFileNameWithoutExtension(bookReq.ImageFile.FileName);
                    string fileExtension = Path.GetExtension(bookReq.ImageFile.FileName);
                    fileName = fileName.Trim() + fileExtension;

                    string uploadPath = "C:/Users/Asus/Desktop/DAW/Serenity-Craft/Serenity-Craft/Content/books/" + fileName;
                    bookReq.ImagePath = "/Content/books/" + fileName;
                    bookReq.ImageFile.SaveAs(uploadPath);
                }

                bookReq.Publisher = db.Publishers.FirstOrDefault(p => p.PublisherId.Equals(bookReq.PublisherId));
                bookReq.BookType = db.BookTypes.FirstOrDefault(p => p.BookTypeId.Equals(bookReq.BookTypeId));

                bookReq.BookTypesList = GetAllBookTypes();
                bookReq.PublishersList = GetAllPublishers();

                var selectedGenres = bookReq.GenresList.Where(b => b.Checked).ToList();

                if (ModelState.IsValid)
                {
                    if (NotExists(bookReq) == -1 || NotExists(bookReq) == id)
                    {
                        Book book = db.Books.Include("Publisher").Include("BookType")
                            .SingleOrDefault(b => b.BookId.Equals(id));

                        if (TryUpdateModel(book))
                        {
                            book.Title = bookReq.Title;
                            book.Author = bookReq.Author;
                            book.Pages = bookReq.Pages;
                            book.Price = bookReq.Price;
                            book.Summary = bookReq.Summary;
                            book.Publisher = bookReq.Publisher;
                            book.BookType = bookReq.BookType;
                            if (uploaded == 1)
                            {
                                book.ImagePath = bookReq.ImagePath;
                                book.ImageFile = bookReq.ImageFile;
                            }

                            book.Genres.Clear();
                            book.Genres = new List<Genre>();

                            for (int i = 0; i < selectedGenres.Count(); i++)
                            {
                                Genre genre = db.Genres.Find(selectedGenres[i].Id);
                                book.Genres.Add(genre);
                            }

                            db.SaveChanges();
                        }

                        return RedirectToAction("Index");
                    }

                    ViewBag.Message = "The book is already in our database!";
                    return View(bookReq);
                }

                ViewBag.Message = "Oh snap! Change a few things up and try submitting again.";
                return View(bookReq);
            }
            catch (Exception)
            {
                ViewBag.Message = "Something went wrong. Please try again!";
                return View(bookReq);
            }
        }

        // DELETE
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Book book = db.Books.Find(id);

            if (book != null)
            {
                db.Books.Remove(book);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return HttpNotFound("Couldn't find the book...");
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllBookTypes()
        {
            var selectList = new List<SelectListItem>();
            var bookTypes = db.BookTypes.OrderBy(bt => bt.Name);

            foreach (var bt in bookTypes.ToList())
            {
                selectList.Add(new SelectListItem
                {
                    Value = bt.BookTypeId.ToString(),
                    Text = bt.Name
                });
            }

            return selectList;
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllPublishers()
        {
            var selectList = new List<SelectListItem>();
            var publishers = db.Publishers.OrderBy(p => p.Name);

            foreach (var pub in publishers.ToList())
            {
                selectList.Add(new SelectListItem
                {
                    Value = pub.PublisherId.ToString(),
                    Text = pub.Name
                });
            }

            return selectList;
        }

        [NonAction]
        public List<CheckBoxModel> GetAllGenres()
        {
            var checkboxList = new List<CheckBoxModel>();
            var genres = db.Genres.OrderBy(g => g.Name);

            foreach (var genre in genres.ToList())
            {
                checkboxList.Add(new CheckBoxModel
                {
                    Id = genre.GenreId,
                    Name = genre.Name,
                    Checked = false
                });
            }

            return checkboxList;
        }

        [NonAction]
        public int NotExists(Book bookReq)
        {
            Book searchBook = db.Books.SingleOrDefault(b => b.Title.Equals(bookReq.Title));
            if (searchBook == null || !searchBook.Author.Equals(bookReq.Author))
            {
                return -1;
            }

            return searchBook.BookId;
        }

    }
}
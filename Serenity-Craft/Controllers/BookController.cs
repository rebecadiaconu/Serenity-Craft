using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serenity_Craft.Models;

namespace Serenity_Craft.Controllers
{
    public class BookController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // READ
        [HttpGet]
        public ActionResult Index()
        {
            List<Book> books = db.Books.Include("Publisher").Include("BookType").ToList();
            ViewBag.Books = books;

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

                return HttpNotFound("Cartea nu exista!");
            }

            return HttpNotFound("Parametrul lipseste!");
        }

        // CREATE
        [HttpGet]
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
        public ActionResult New(Book bookReq)
        {
            try
            {
                bookReq.Publisher = db.Publishers.FirstOrDefault(p => p.PublisherId.Equals(bookReq.PublisherId));
                bookReq.BookType = db.BookTypes.FirstOrDefault(p => p.BookTypeId.Equals(bookReq.BookTypeId));

                bookReq.BookTypesList = GetAllBookTypes();
                bookReq.PublishersList = GetAllPublishers();

                var genreSelected = bookReq.GenresList.Where(b => b.Checked).ToList();

                if (ModelState.IsValid)
                {

                    if (NotExists(bookReq) == -1)
                    {
                        bookReq.Genres = new List<Genre>();
                        for (int i = 0; i < genreSelected.Count(); i++)
                        {
                            Genre genre = db.Genres.Find(genreSelected[i].Id);
                            bookReq.Genres.Add(genre);
                        }

                        db.Books.Add(bookReq);
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }

                    ViewBag.Message = "Cartea se afla deja in baza de date!";
                    return View(bookReq);
                }

                ViewBag.Message = "Una sau mai multe validari nu sunt respectate!";
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

                return HttpNotFound("Nu exista aceasta carte");
            }

            return HttpNotFound("Parametrul lipseste!");
        }

        [HttpPut]
        public ActionResult Edit(int id, Book bookReq)
        {
            try
            {
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

                    ViewBag.Message = "Deja exista o carte cu caracteristici asemanatoare!";
                    return View(bookReq);
                }

                ViewBag.Message = "Una sau mai multe validari nu sunt respectate!";
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
        public ActionResult Delete(int id)
        {
            Book book = db.Books.Find(id);

            if (book != null)
            {
                db.Books.Remove(book);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return HttpNotFound("Cartea nu exista!");
        }

        [NonAction] 
        public IEnumerable<SelectListItem> GetAllBookTypes()
        {
            var selectList = new List<SelectListItem>();
            foreach (var bt in db.BookTypes.ToList())
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

            foreach (var pub in db.Publishers.ToList())
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
            foreach (var genre in db.Genres.ToList())
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serenity_Craft.Models;

namespace Serenity_Craft.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GenreController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // READ
        public ActionResult Index(int? id)
        {
            var genres = db.Genres.OrderBy(g=>g.Name);
            ViewBag.Genres = genres.ToList();

            if (id.HasValue)
            {
                ViewBag.Message = AllBooks((int)id).ToString();
                ViewBag.GenreId = id;

                return View();
            }

            ViewBag.Message = null;
            return View();
        }

        // CREATE
        [HttpGet]
        public ActionResult New()
        {
            Genre genre = new Genre();

            ViewBag.Message = null;
            return View(genre);
        }

        [HttpPost]
        public ActionResult New(Genre genreReq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Genre searchGenre = db.Genres.SingleOrDefault(p => p.Name.Equals(genreReq.Name));

                    if (searchGenre == null || searchGenre.GenreId == genreReq.GenreId)
                    {
                        db.Genres.Add(genreReq);
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }

                    ViewBag.Message = "Genre with the same name already exists in our database!";
                    return View(genreReq);
                }

                ViewBag.Message = "Oh snap! Change a few things up and try submitting again";
                return View(genreReq);
            }
            catch (Exception)
            {
                ViewBag.Message = "Something went wrong... Please try again!";
                return View(genreReq);
            }
        }

        // UPDATE
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Genre genre = db.Genres.Find(id);
                if (genre != null)
                {
                    return View(genre);
                }

                return HttpNotFound("Couldn't find the genre you are searching for...");
            }

            return HttpNotFound("Parameter is missing...");
        }

        [HttpPut]
        public ActionResult Edit(int id, Genre genreReq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Genre searchGenre = db.Genres.SingleOrDefault(p => p.Name.Equals(genreReq.Name));

                    if (searchGenre == null)
                    {
                        Genre genre = db.Genres.SingleOrDefault(bt => bt.GenreId.Equals(id));

                        if (TryUpdateModel(genre))
                        {
                            genre.Name = genreReq.Name;
                            db.SaveChanges();
                        }

                        return RedirectToAction("Index");
                    }

                    ViewBag.Message = "Genre with the same name already exists in our database!";
                    return View(genreReq);
                }

                ViewBag.Message = "Oh snap! Change a few things up and try submitting again";
                return View(genreReq);
            }
            catch (Exception)
            {
                ViewBag.Message = "Something went wrong... Please try again!";
                return View(genreReq);
            }
        }

        // DELETE
        [HttpDelete]
        public ActionResult Delete(int id, int? delete)
        {
            Genre genre = db.Genres.Find(id);

            if (genre != null)
            {
                if (delete.HasValue && delete == 1)
                {
                    foreach (var book in db.Books.ToList())
                    {
                        if (book.Genres.Count == 1 && book.Genres.Contains(genre))
                        {
                            db.Books.Remove(book);
                        }
                    }

                    db.Genres.Remove(genre);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                if (AllBooks(id) == 0)
                {
                    db.Genres.Remove(genre);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index", new { id = id });
            }

            return HttpNotFound("Couldn't find the genre you are searching for...");
        }

        [NonAction]
        public int AllBooks(int id)
        {
            var bookCounter = 0;

            foreach (var book in db.Books.ToList())
            {
                if (book.Genres.Count() == 1)
                {
                    Genre genre = book.Genres.FirstOrDefault(p => p.GenreId == id);

                    if (genre != null)
                    {
                        bookCounter += 1;
                    }
                }
            }

            return bookCounter;
        }

    }
}
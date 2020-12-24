using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serenity_Craft.Models;

namespace Serenity_Craft.Controllers
{
    public class BookTypeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // READ
        public ActionResult Index(int? id)
        {

            List<BookType> btypes = db.BookTypes.ToList();
            ViewBag.BookTypes = btypes;

            if (id.HasValue)
            {
                if (id == 0)
                {
                    ViewBag.Message =
                        "Exista carti cu acest tip de carte. Fa modificarile necesare inainte de stergere!";
                    return View();
                }

                ViewBag.Message = null;
                return View();
            }
            
            ViewBag.Message = null;
            return View();
        }

        // CREATE
        [HttpGet]
        public ActionResult New()
        {
            BookType bookType = new BookType();

            ViewBag.Message = null;
            return View(bookType);
        }

        [HttpPost]
        public ActionResult New(BookType typeReq)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    BookType searchBookType = db.BookTypes.SingleOrDefault(p => p.Name.Equals(typeReq.Name));

                    if (searchBookType == null)
                    {
                        db.BookTypes.Add(typeReq);
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }

                    ViewBag.Message = "Acest tip se afla deja in baza de date!";
                    return View(typeReq);
                }

                ViewBag.Message = "Una sau mai multe validari nu sunt respectate!";
                return View(typeReq);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Something went wrong. Please try again!";
                return View(typeReq);
            }
        }

        // UPDATE
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                BookType btype = db.BookTypes.Find(id);
                if (btype != null)
                {
                    return View(btype);
                }

                return HttpNotFound("Nu exista acest tip de carte!");
            }

            return HttpNotFound("Parametrul lipseste!");
        }

        [HttpPut]
        public ActionResult Edit(int id, BookType typeReq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookType searchBookType = db.BookTypes.SingleOrDefault(p => p.Name.Equals(typeReq.Name));

                    if (searchBookType == null)
                    {
                        BookType bookType = db.BookTypes.SingleOrDefault(bt => bt.BookTypeId.Equals(id));

                        if (TryUpdateModel(bookType))
                        {
                            bookType.Name = typeReq.Name;
                            db.SaveChanges();
                        }

                        return RedirectToAction("Index");
                    }

                    ViewBag.Message = "Deja exista acest tip de carte!";
                    return View(typeReq);
                }

                ViewBag.Message = "Una sau mai multe validari nu sunt respectate!";
                return View(typeReq);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Something went wrong... Please try again!";
                return View(typeReq);
            }
        }

        // DELETE
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            BookType bType = db.BookTypes.Find(id);

            if (bType != null)
            {

                if (AllBooks(id) == 0)
                {
                    db.BookTypes.Remove(bType);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index", new { id = 0 });
            }

            return HttpNotFound("Tipul de carte nu exista!");
        }

        [NonAction]
        public int AllBooks(int id)
        {
            var bookCounter = 0;

            foreach (var book in db.Books.ToList())
            {
                if (book.BookTypeId == id)
                {
                    bookCounter += 1;
                }
            }

            return bookCounter;
        }

    }
}
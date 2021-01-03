using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Serenity_Craft.Models;

namespace Serenity_Craft.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookTypeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // READ
        public ActionResult Index(int? id)
        {

            var btypes = db.BookTypes.OrderBy(bt =>bt.Name);
            ViewBag.BookTypes = btypes.ToList();

            if (id.HasValue)
            {
                ViewBag.Message = AllBooks((int)id).ToString();
                ViewBag.TypeId = id;

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

                    ViewBag.Message = "Book type with the same name already exists in our database!";
                    return View(typeReq);
                }

                ViewBag.Message = "Oh snap! Change a few things up and try submitting again";
                return View(typeReq);
            }
            catch (Exception)
            {
                ViewBag.Message = "Something went wrong... Please try again!";
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

                return HttpNotFound("Couldn't find the book type you are searching for...");
            }

            return HttpNotFound("Parameter is missing...");
        }

        [HttpPut]
        public ActionResult Edit(int id, BookType typeReq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookType searchBookType = db.BookTypes.SingleOrDefault(p => p.Name.Equals(typeReq.Name));

                    if (searchBookType == null || searchBookType.BookTypeId == typeReq.BookTypeId)
                    {
                        BookType bookType = db.BookTypes.SingleOrDefault(bt => bt.BookTypeId.Equals(id));

                        if (TryUpdateModel(bookType))
                        {
                            bookType.Name = typeReq.Name;
                            db.SaveChanges();
                        }

                        return RedirectToAction("Index");
                    }

                    ViewBag.Message = "Book type with the same name already exists in our database!";
                    return View(typeReq);
                }

                ViewBag.Message = "Oh snap! Change a few things up and try submitting again";
                return View(typeReq);
            }
            catch (Exception)
            {
                ViewBag.Message = "Something went wrong... Please try again!";
                return View(typeReq);
            }
        }

        // DELETE
        [HttpDelete]
        public ActionResult Delete(int id, int? delete)
        {
            BookType bType = db.BookTypes.Find(id);

            if (bType != null)
            {
                if (delete.HasValue && delete == 1)
                {
                    db.BookTypes.Remove(bType);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                int noOfbooks = AllBooks(id);

                if (noOfbooks == 0)
                {
                    db.BookTypes.Remove(bType);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index", new {id = id});
            }

            return HttpNotFound("Couldn't find the book you are searching for...");
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
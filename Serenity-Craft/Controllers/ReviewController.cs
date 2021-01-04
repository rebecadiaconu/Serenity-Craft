using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using Serenity_Craft.Models;

namespace Serenity_Craft.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // READ
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string sortOrder, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var reviews = db.Reviews.Include("Book").OrderBy(r => r.UserName);

            switch (sortOrder)
            {
                case "name_desc":
                    reviews = reviews.OrderByDescending(s => s.UserName);
                    break;
                case "Date":
                    reviews = reviews.OrderBy(s => s.ReviewDate);
                    break;
                case "date_desc":
                    reviews = reviews.OrderByDescending(s => s.ReviewDate);
                    break;
                default:
                    reviews = reviews.OrderBy(s => s.UserName);
                    break;
            }

            int pageSize = 7;
            int pageNumber = (page ?? 1);

            return View(reviews.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Review review = db.Reviews.Find(id);
                if (review != null)
                {
                    return View(review);
                }

                return HttpNotFound("Couldn't find the review you are searching for...");
            }

            return HttpNotFound("Parameter is missing...");
        }

            // CREATE
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public ActionResult New(int? id)
        {
            if (id.HasValue)
            {
                Book book = db.Books.Find(id);

                if (book != null)
                {
                    Review review = new Review();
                    ViewBag.Notes = GetAllNotes();
                    ViewBag.Book = id;
                    ViewBag.Message = null;

                    return View(review);
                }
            }

            return HttpNotFound("Parameter is missing!");
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public ActionResult New(int id, Review reviewReq)
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.FirstOrDefault(x => x.Id == userId);

            ViewBag.Notes = GetAllNotes();

            try
            {
                if (ModelState.IsValid)
                {
                    Book book = db.Books.Find(id);
                    reviewReq.BookId = id;
                    reviewReq.Book = book;
                    reviewReq.UserName = user?.Email;

                    if (NotExists(book, reviewReq.UserName) == 1)
                    {
                        book.Reviews.Add(reviewReq);
                        db.Reviews.Add(reviewReq);

                        db.SaveChanges();

                        return RedirectToAction("Details", "Book", new {id = id});
                    }

                    ViewBag.Message = "You already write a review about this book. Perhaps you want to change it.";
                    return View(reviewReq);
                }

                ViewBag.Message = "Oh snap! Change a few things up and try submitting again";
                return View(reviewReq);
            }
            catch (Exception)
            {
                ViewBag.Message = "Something went wrong... Please try again!";
                return View(reviewReq);
            }
        }

        // UPDATE
        [Authorize(Roles = "Admin, User")]
        public ActionResult Edit(int? id)
        {

            if (id.HasValue)
            {
                Review review = db.Reviews.Find(id);

                if (review != null)
                {
                    ViewBag.Notes = GetAllNotes();

                    return View(review);
                }

                return HttpNotFound("Couldn't find the review type you are searching for...");
            }

            return HttpNotFound("Parameter is missing...");
        }

        [HttpPut]
        [Authorize(Roles = "Admin, User")]
        public ActionResult Edit(int id, Review reviewReq)
        {
            try
            {
                reviewReq.ReviewDate = DateTime.Now;
                ViewBag.Notes = GetAllNotes();

                if (ModelState.IsValid)
                {
                    Review review = db.Reviews.Find(id);

                    if (TryUpdateModel(review))
                    {
                        review.Text = reviewReq.Text;
                        review.Note = reviewReq.Note;
                        review.ReviewDate = reviewReq.ReviewDate;

                        db.SaveChanges();
                    }

                    return RedirectToAction("Details", "Review", new {id = review.ReviewId});
                }

                ViewBag.Message = "Oh snap! Change a few things up and try submitting again";
                return View(reviewReq);
            }
            catch (Exception)
            {
                ViewBag.Message = "Something went wrong... Please try again!";
                return View(reviewReq);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, User")]
        public ActionResult Delete(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review != null)
            {
                Book book = db.Books.Find(review.BookId);

                if (book != null)
                {
                    book.Reviews.Remove(review);
                    db.Reviews.Remove(review);
                    db.SaveChanges();

                    return RedirectToAction("Details", "Book", new {id = book.BookId});
                }
            }

            return HttpNotFound("Couldn't find the review type you are searching for...");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public ActionResult MyReviews(string sortOrder, int? page)
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.FirstOrDefault(x => x.Id == userId);

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var reviews = db.Reviews.Include("Book").Where(r => r.UserName.Equals(user.Email)).OrderBy(r => r.Book.Title);

            switch (sortOrder)
            {
                case "name_desc": 
                    reviews = db.Reviews.Include("Book").Where(r => r.UserName.Equals(user.Email)).OrderByDescending(s => s.Book.Title);
                    break;
                case "Date":
                    reviews = db.Reviews.Include("Book").Where(r => r.UserName.Equals(user.Email)).OrderBy(s => s.ReviewDate);
                    break;
                case "date_desc":
                    reviews = db.Reviews.Include("Book").Where(r => r.UserName.Equals(user.Email)).OrderByDescending(s => s.ReviewDate);
                    break;
                default:
                    reviews = db.Reviews.Include("Book").Where(r => r.UserName.Equals(user.Email)).OrderBy(s => s.Book.Title);
                    break;
            }

            int pageSize = 7;
            int pageNumber = (page ?? 1);

            return View(reviews.ToPagedList(pageNumber, pageSize));
        }

        [NonAction]
        public int NotExists(Book book, string userName)
        {
            List<Review> reviews = book.Reviews.ToList();

            if (reviews.Count != 0)
            {
                foreach (var review in reviews)
                {
                    if (review.UserName == userName)
                    {
                        return 0;
                    }
                }
            }

            return 1;
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllNotes()
        {
            var selectList = new List<SelectListItem>();

            for (var i = 0; i <= 5; i++)
            {
                selectList.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
            }

            return selectList;
        }
    }
}
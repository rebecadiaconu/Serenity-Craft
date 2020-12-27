using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Serenity_Craft.Models;

namespace Serenity_Craft.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class ReviewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        

        // READ
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.FirstOrDefault(x => x.Id == userId);

            ViewBag.Reviews = db.Reviews.FirstOrDefault(p => p.UserName.Equals(user.UserName));

            return View();
        }

        // CREATE
        [HttpGet]
        public ActionResult New(int? id)
        {
            if (id.HasValue)
            {
                Book book = db.Books.Find(id);

                if (book != null)
                {
                    Review review = new Review();
                    ViewBag.Notes = GetAllNotes();
                    ViewBag.Message = null;

                    return View(review);
                }
            }

            return HttpNotFound("Parameter is missing!");
        }

        [HttpPost]
        public ActionResult New(int id, Review reviewReq)
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.FirstOrDefault(x => x.Id == userId);

            ViewBag.Notes = GetAllNotes();

            try
            {
                Book book = db.Books.Find(id);
                reviewReq.BookId = id;
                reviewReq.Book = book;
                reviewReq.UserName = user.Email;

                if (ModelState.IsValid)
                {
                    if (NotExists(book, reviewReq.UserName) == 1)
                    {
                        book.Reviews.Add(reviewReq);
                        db.Reviews.Add(reviewReq);
                    }

                    ViewBag.Message = "Deja ai o recenzie data despre aceasta carte. Doresti sa o modifci? Check `My Reviews.`";
                    return View(reviewReq);
                }

                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();
                Console.WriteLine(errors);

                ViewBag.Message = "Una sau mai multe validari nu sunt respectate!";
                return View(reviewReq);
            }
            catch (Exception)
            {
                ViewBag.Message = "Something went wrong. Please try again!";
                return View(reviewReq);
            }
        }


        [NonAction]
        public int NotExists(Book book, string userName)
        {
            List<Review> reviews = book.Reviews.ToList();

            if (reviews != null)
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
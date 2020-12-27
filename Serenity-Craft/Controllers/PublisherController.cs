using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serenity_Craft.Models;

namespace Serenity_Craft.Controllers
{
    public class PublisherController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // READ
        [HttpGet]
        public ActionResult Index(int? id)
        {
            List<Publisher> publishers = db.Publishers.Include("Contact").ToList();
            ViewBag.publishers = publishers;

            if (id.HasValue)
            {
                ViewBag.Message = AllBooks((int)id).ToString();
                ViewBag.PubId = id;

                return View();
            }

            ViewBag.Message = null;
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Publisher pub = db.Publishers.Find(id);
                if (pub != null)
                {
                    return View(pub);
                }

                return HttpNotFound("Editura nu exista!");
            }

            return HttpNotFound("Parametrul lipseste!");
        }

        // CREATE
        [HttpGet]
        public ActionResult New()
        {
            PublisherContactInfo pub = new PublisherContactInfo();

            ViewBag.Message = null;
            return View(pub);
        }

        [HttpPost]
        public ActionResult New(PublisherContactInfo pubReq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Contact contact = new Contact
                        {
                            PhoneNumber = pubReq.PhoneNumber,
                            Email = pubReq.Email
                        };
                    
                    Publisher publisher = new Publisher
                    {
                        Name = pubReq.Name,
                        Contact = contact
                    };

                    if (NotExists(publisher.Name) == -1)
                    {
                        db.Contacts.Add(contact);
                        db.Publishers.Add(publisher);
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }

                    ViewBag.Message = "Editura se afla deja in baza de date! Poate doresti sa o modifici!";
                    return View(pubReq);
                }

                ViewBag.Message = "Una sau mai multe validari nu sunt respectate!";
                return View(pubReq);
            }
            catch (Exception)
            {
                ViewBag.Message = "Something went wrong. Please try again!!";
                return View(pubReq);
            }
        }

        // UPDATE
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Publisher pub = db.Publishers.Find(id);

                if (pub != null)
                {
                    return View(pub);
                }

                return HttpNotFound("Nu exista aceasta editura");
            }

            return HttpNotFound("Parametrul lipseste!");
        }

        [HttpPut]
        public ActionResult Edit(int id, Publisher pubReq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (NotExists(pubReq.Name) == -1 || NotExists(pubReq.Name) == id)
                    {
                        Publisher publisher =
                            db.Publishers.Include("Contact").SingleOrDefault(p => p.PublisherId == id);

                        if (TryUpdateModel(publisher))
                        {
                            publisher.Name = pubReq.Name;
                            publisher.Contact = pubReq.Contact;

                            db.SaveChanges();
                        }

                        return RedirectToAction("Index");
                    }

                    ViewBag.Message = "Deja exista o editura cu caracteristici asemanatoare!";
                    return View(pubReq);
                }

                ViewBag.Message = "Una sau mai multe validari nu sunt respectate!";
                return View(pubReq);
            }
            catch (Exception)
            {
                ViewBag.Message = "Something went wrong. Please try again!";
                return View(pubReq);
            }
        }

        // DELETE
        [HttpDelete]
        public ActionResult Delete(int id, int? delete)
        {
            Publisher publisher = db.Publishers.Find(id);

            if (publisher != null)
            {
                if (delete.HasValue && delete == 1)
                {
                    foreach (var book in db.Books.ToList())
                    {
                        if (book.Publisher.Equals(publisher))
                        {
                            db.Books.Remove(book);
                        }
                    }

                    db.Contacts.Remove(publisher.Contact);
                    db.Publishers.Remove(publisher);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                if (AllBooks(id) == 0)
                {
                    db.Contacts.Remove(publisher.Contact);
                    db.Publishers.Remove(publisher);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index", new { id = id });
            }

            return HttpNotFound("Editura nu exista!");
        }


        [NonAction]
        public int NotExists(string name)
        {
            Publisher searchPub = db.Publishers.SingleOrDefault(b => b.Name.Equals(name));

            if (searchPub == null)
            {
                return -1;
            }

            return searchPub.PublisherId;
        }

        [NonAction]
        public int AllBooks(int id)
        {
            var bookCounter = 0;

            foreach (var book in db.Books.ToList())
            {
                if (book.PublisherId == id)
                {
                    bookCounter += 1;
                }
            }

            return bookCounter;
        }
    }
}
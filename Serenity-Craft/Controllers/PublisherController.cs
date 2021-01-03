﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serenity_Craft.Models;

namespace Serenity_Craft.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PublisherController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // READ
        [HttpGet]
        public ActionResult Index(int? id)
        {
            var publishers = db.Publishers.Include("Contact").OrderBy(p =>p.Name);
            ViewBag.publishers = publishers.ToList();

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

                return HttpNotFound("Couldn't find the publisher you are searching for...");
            }

            return HttpNotFound("Parameter is missing...");
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

                    ViewBag.Message = "Publisher with the same name already exists in our database!";
                    return View(pubReq);
                }

                ViewBag.Message = "Oh snap! Change a few things up and try submitting again";
                return View(pubReq);
            }
            catch (Exception)
            {
                ViewBag.Message = "Something went wrong... Please try again!";
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

                return HttpNotFound("Couldn't find the publisher you are searching for...");
            }

            return HttpNotFound("Parameter is missing...");
        }

        [HttpPut]
        public ActionResult Edit(int id, Publisher pubReq)
        {
            try
            {
                //Contact contact = new Contact
                //{
                //    PublisherId = id,
                //    PhoneNumber = pubReq.Contact.PhoneNumber,
                //    Email = pubReq.Contact.em
                //};
                //pubReq.Contact.PublisherId = id;

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

                    ViewBag.Message = "Publisher with the same name already exists in our database!";
                    return View(pubReq);
                }
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();
                Console.WriteLine(errors);

                ViewBag.Message = "Oh snap! Change a few things up and try submitting again";
                return View(pubReq);
            }
            catch (Exception)
            {
                ViewBag.Message = "Something went wrong... Please try again!";
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

                    if (publisher.Contact != null)
                    {
                        db.Contacts.Remove(publisher.Contact);
                    }
                    
                    db.Publishers.Remove(publisher);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                if (AllBooks(id) == 0)
                {
                    if (publisher.Contact != null)
                    {
                        db.Contacts.Remove(publisher.Contact);
                    }

                    db.Publishers.Remove(publisher);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index", new { id = id });
            }

            return HttpNotFound("Couldn't find the publisher you are searching for...");
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
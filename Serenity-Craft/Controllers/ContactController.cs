using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serenity_Craft.Models;

namespace Serenity_Craft.Controllers
{
    public class ContactController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // READ
        [HttpGet]
        public ActionResult Index(int? id)
        {
            List<Contact> contacts = db.Contacts.ToList();
            ViewBag.contacts = contacts;

            if (id.HasValue)
            {
                ViewBag.Message = id;

                return View();
            }

            ViewBag.Message = null;
            return View();
        }

        // UPDATE
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Contact contact = db.Publishers.Find(id).Contact;
                if (contact != null)
                {
                    ViewBag.Name = contact.Publisher.Name;
                    return View(contact);
                }
                return HttpNotFound("Nu exista acest contact!");
            }
            return HttpNotFound("Parametrul lipseste!");
        }

        [HttpPut]
        public ActionResult Edit(int id, Contact contactReq)
        {
            try
            {
                Publisher pub = db.Publishers.SingleOrDefault(c => c.PublisherId == id);
                contactReq.Publisher = pub;

                if (ModelState.IsValid)
                {
                    Contact contact = db.Contacts.SingleOrDefault(c => c.PublisherId == id);

                    if (TryUpdateModel(contact))
                    {
                        contact.PhoneNumber = contactReq.PhoneNumber;
                        contact.Email = contactReq.Email;

                        db.SaveChanges();
                    }

                    return RedirectToAction("Index", new { id = (dynamic)null });
                }

                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();
                Console.WriteLine(errors);

                ViewBag.Message = "Una sau mai multe validari nu sunt respectate!";
                return View(contactReq);
            }
            catch (Exception)
            {
                ViewBag.Message = "Something went wrong... Please try again!";
                return View(contactReq);
            }

        }


        // DELETE
        [HttpDelete]
        public ActionResult Delete(int id, int? delete)
        {
            Contact contact = db.Contacts.Find(id);

            if (contact != null)
            {
                if (delete.HasValue && delete == 1)
                {
                    db.Contacts.Remove(contact);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index", new {id = id});
            }

            return HttpNotFound("Contactul nu exista!");
        }
    }
}
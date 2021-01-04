using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Serenity_Craft.Models;

namespace Serenity_Craft.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ContactController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // READ
        [HttpGet]
        public ActionResult Index(string sortOrder, int? id, int? page)
        {
            var contacts = db.Contacts.OrderBy(c=>c.Publisher.Name);

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";

            if (id.HasValue)
            {
                ViewBag.Message = id;
            }
            else
            {
                ViewBag.Message = null;
            }

            switch (sortOrder)
            {
                case "title_desc":
                    contacts = contacts.OrderByDescending(s => s.Publisher.Name);
                    break;
                default:
                    contacts = contacts.OrderBy(s => s.Publisher.Name);
                    break;
            }

            int pageSize = 7;
            int pageNumber = (page ?? 1);

            return View(contacts.ToPagedList(pageNumber, pageSize));
        }

        // UPDATE
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Contact contact = db.Contacts.Find(id);
                if (contact != null)
                {
                    ViewBag.Name = contact.Publisher.Name;
                    return View(contact);
                }
                return HttpNotFound("Couldn't find the contact you are searching for...");
            }

            return HttpNotFound("Parameter is missing...");
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

                ViewBag.Message = "Oh snap! Change a few things up and try submitting again";
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

            return HttpNotFound("Couldn't find the contact you are searching for...");
        }
    }
}
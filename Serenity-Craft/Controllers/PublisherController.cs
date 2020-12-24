using System;
using System.Collections.Generic;
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
        public ActionResult Index()
        {
            List<Publisher> publishers = db.Publishers.Include("Contact").ToList();
            ViewBag.publishers = publishers;

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
            Publisher publisher = new Publisher
            {
                Contact = new Contact();
            };

            ViewBag.Message = null;
            return View(publisher);
        }

        [HttpPost]
        public ActionResult New(Publisher pubReq)
        {
            try
            {

            }
            catch (Exception e)
            {
                ViewBag.Message = "Something went wrong. Please try again!";
                return View(pubReq);
            }
        }
    }
}
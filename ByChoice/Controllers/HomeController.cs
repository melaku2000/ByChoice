using ByChoice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace ByChoice.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;

        public HomeController()
        {
            db = new ApplicationDbContext();
            db.Configuration.LazyLoadingEnabled = false;

        }
        // GET: ProductImage
        [Route("Index")]
        [HttpGet]
        public ActionResult Index()
        {
            if (db.ProductModels.Count() == 0)
                return View();

            return View(db.ProductModels.Include(p=>p.ProductCatagory).ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //GET
        public ActionResult RetrieveImage(int id)
        {
            var q = from temp in db.ProductModels where temp.ProductModelId == id select temp.Image;
            byte[] cover = q.First();
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        // GET: ProductModel/Details/5
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModel productModel = db.ProductModels.Find(id);
            if (productModel == null)
            {
                return HttpNotFound();
            }
            return View(productModel);
        }
        //GET
        public ActionResult RetrieveCerteficate(int id)
        {
            var q = from temp in db.ProductModels where temp.ProductModelId == id select temp.Certificate;
            byte[] cover = q.First();
            if (cover != null)
            {
                return File(cover, "application/pdf", "Certeficate");
            }
            else
            {
                return Content("We appologize the file you request is not found");
            }
        }
        //GET
        public ActionResult RetrieveBrowsher(int id)
        {
            var q = from temp in db.ProductModels where temp.ProductModelId == id select temp.Browsher;
            byte[] cover = q.First();
            if (cover != null)
            {
                return File(cover, "application/pdf", "Browsher");
            }
            else
            {
                return Content("We appologize the file you request is not found");
            }
        }
    }
}
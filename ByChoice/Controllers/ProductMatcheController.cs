using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ByChoice.Models;

namespace ByChoice.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductMatcheController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProductMatche
        public ActionResult Index()
        {
            var productMatches = db.ProductMatches.Include(p => p.ProductMatchPurpose).Include(p => p.ProductModel);
            return View(productMatches.ToList());
        }

        // GET: ProductMatche/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductMatch productMatch = db.ProductMatches.Include(p=>p.ProductModel).SingleOrDefault(p=>p.ProductModelId==id);
            if (productMatch == null)
            {
                return HttpNotFound();
            }
            return View(productMatch);
        }

        // GET: ProductMatche/Create
        public ActionResult Create(int? id)
        {
          
             var model= db.ProductModels.Include(p=>p.ProductCatagory).SingleOrDefault(p => p.ProductModelId == id);
            if (model == null)
                return RedirectToAction("Index", "ProductModel");

            ViewBag.ProductModel = model;
            return View();
        }

        // POST: ProductMatche/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductModelId,NoLight,Radio,FlashLight,TvSize,StudyLight,HomeUse,TravelingLight,Charger")] ProductMatch productMatch)
        {
            if (ModelState.IsValid)
            {
                //productMatch.ProductModelId = productMatch.ProductModelId;
                db.ProductMatches.Add(productMatch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.ProductMatchPurposeId = new SelectList(db.ProductMatchPurposes, "ProductMatchPurposeId", "Purpose", productMatch.ProductMatchPurposeId);
            ViewBag.ProductModelId = new SelectList(db.ProductModels, "ProductModelId", "ProductName", productMatch.ProductModelId);
            return View(productMatch);
        }

        // GET: ProductMatche/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductMatch productMatch = db.ProductMatches.Include(p=>p.ProductModel).SingleOrDefault(p=>p.ProductModelId==id);
            if (productMatch == null)
            {
                return HttpNotFound();
            }
            return View(productMatch);
        }

        // POST: ProductMatche/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductModelId,NoLight,Radio,FlashLight,TvSize,StudyLight,HomeUse,TravelingLight,Charger")] ProductMatch productMatch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productMatch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           // ViewBag.ProductMatchPurposeId = new SelectList(db.ProductMatchPurposes, "ProductMatchPurposeId", "Purpose", productMatch.ProductMatchPurposeId);
            ViewBag.ProductModelId = new SelectList(db.ProductModels, "ProductModelId", "ProductName", productMatch.ProductModelId);
            return View(productMatch);
        }

        // GET: ProductMatche/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductMatch productMatch = db.ProductMatches.Find(id);
            if (productMatch == null)
            {
                return HttpNotFound();
            }
            return View(productMatch);
        }

        // POST: ProductMatche/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductMatch productMatch = db.ProductMatches.Find(id);
            db.ProductMatches.Remove(productMatch);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult CustomerProductMatch()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult CustomerProductMatch([Bind(Include = "NoLight,Radio,FlashLight,TvSize,StudyLight,HomeUse,TravelingLight")] ProductMatch productMatch)
        {
            IEnumerable<ProductModel> product = db.ProductModels.Include(p=>p.ProductMatch);
            if(productMatch.FlashLight==true)
                product= product.Where(p => p.ProductMatch.FlashLight == true);
            if(productMatch.Radio==true)
                product= product.Where(p => p.ProductMatch.Radio == true);
            if(productMatch.StudyLight==true)
                product= product.Where(p => p.ProductMatch.StudyLight == true);
            if (productMatch.HomeUse==true)
                product= product.Where(p => p.ProductMatch.HomeUse == true);
            if(productMatch.TravelingLight==true)
                product= product.Where(p => p.ProductMatch.TravelingLight == true);
            if(productMatch.TvSize>0)
                product= product.Where(p => p.ProductMatch.TvSize == productMatch.TvSize);
            if(productMatch.NoLight>0)
                product= product.Where(p => p.ProductMatch.NoLight == productMatch.NoLight);
            ViewBag.MatchResult = product;
            return View();
        }

       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

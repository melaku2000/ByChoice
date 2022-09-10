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
    public class ProductCatagoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProductCatagory
        public ActionResult Index()
        {
            return View(db.ProductCatagories.ToList());
        }

        // GET: ProductCatagory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCatagory productCatagory = db.ProductCatagories.Find(id);
            if (productCatagory == null)
            {
                return HttpNotFound();
            }
            return View(productCatagory);
        }

        // GET: ProductCatagory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductCatagory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductCatagoryId,ProductCatagoryName")] ProductCatagory productCatagory)
        {
            if (ModelState.IsValid)
            {
                db.ProductCatagories.Add(productCatagory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productCatagory);
        }

        // GET: ProductCatagory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCatagory productCatagory = db.ProductCatagories.Find(id);
            if (productCatagory == null)
            {
                return HttpNotFound();
            }
            return View(productCatagory);
        }

        // POST: ProductCatagory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductCatagoryId,ProductCatagoryName")] ProductCatagory productCatagory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productCatagory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productCatagory);
        }

        // GET: ProductCatagory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCatagory productCatagory = db.ProductCatagories.Find(id);
            if (productCatagory == null)
            {
                return HttpNotFound();
            }
            return View(productCatagory);
        }

        // POST: ProductCatagory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductCatagory productCatagory = db.ProductCatagories.Find(id);
            db.ProductCatagories.Remove(productCatagory);
            db.SaveChanges();
            return RedirectToAction("Index");
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

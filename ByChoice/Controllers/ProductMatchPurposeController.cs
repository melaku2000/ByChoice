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
    public class ProductMatchPurposeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProductMatchPurpose
        public ActionResult Index()
        {
            return View(db.ProductMatchPurposes.ToList());
        }

        // GET: ProductMatchPurpose/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductMatchPurpose productMatchPurpose = db.ProductMatchPurposes.Find(id);
            if (productMatchPurpose == null)
            {
                return HttpNotFound();
            }
            return View(productMatchPurpose);
        }

        // GET: ProductMatchPurpose/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductMatchPurpose/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductMatchPurposeId,Purpose")] ProductMatchPurpose productMatchPurpose)
        {
            if (ModelState.IsValid)
            {
                db.ProductMatchPurposes.Add(productMatchPurpose);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productMatchPurpose);
        }

        // GET: ProductMatchPurpose/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductMatchPurpose productMatchPurpose = db.ProductMatchPurposes.Find(id);
            if (productMatchPurpose == null)
            {
                return HttpNotFound();
            }
            return View(productMatchPurpose);
        }

        // POST: ProductMatchPurpose/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductMatchPurposeId,Purpose")] ProductMatchPurpose productMatchPurpose)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productMatchPurpose).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productMatchPurpose);
        }

        // GET: ProductMatchPurpose/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductMatchPurpose productMatchPurpose = db.ProductMatchPurposes.Find(id);
            if (productMatchPurpose == null)
            {
                return HttpNotFound();
            }
            return View(productMatchPurpose);
        }

        // POST: ProductMatchPurpose/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductMatchPurpose productMatchPurpose = db.ProductMatchPurposes.Find(id);
            db.ProductMatchPurposes.Remove(productMatchPurpose);
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

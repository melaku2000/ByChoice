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
    public class WarrantyController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Warranty
        public ActionResult Index()
        {
            var warranties = db.Warranties.Include(w => w.Customer).Include(w => w.Product);
            return View(warranties.ToList());
        }

        // GET: Warranty/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warranty warranty = db.Warranties.Find(id);
            if (warranty == null)
            {
                return HttpNotFound();
            }
            return View(warranty);
        }

        // GET: Warranty/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductId");
            return View();
        }

        // POST: Warranty/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WarrantyId,WarrantyCode,CustomerId,ProductId,Id,SoledDate,SMSID")] Warranty warranty)
        {
            if (ModelState.IsValid)
            {
                db.Warranties.Add(warranty);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", warranty.CustomerId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "AgentId", warranty.ProductId);
            return View(warranty);
        }

        // GET: Warranty/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warranty warranty = db.Warranties.Find(id);
            if (warranty == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", warranty.CustomerId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "AgentId", warranty.ProductId);
            return View(warranty);
        }

        // POST: Warranty/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WarrantyId,WarrantyCode,CustomerId,ProductId,Id,SoledDate,SMSID")] Warranty warranty)
        {
            if (ModelState.IsValid)
            {
                db.Entry(warranty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", warranty.CustomerId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "AgentId", warranty.ProductId);
            return View(warranty);
        }

        // GET: Warranty/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warranty warranty = db.Warranties.Find(id);
            if (warranty == null)
            {
                return HttpNotFound();
            }
            return View(warranty);
        }

        // POST: Warranty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Warranty warranty = db.Warranties.Find(id);
            db.Warranties.Remove(warranty);
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

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
    public class ClaimTypeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ClaimType
        public ActionResult Index()
        {
            return View(db.ClaimTypes.ToList());
        }

        // GET: ClaimType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaimType claimType = db.ClaimTypes.Find(id);
            if (claimType == null)
            {
                return HttpNotFound();
            }
            return View(claimType);
        }

        // GET: ClaimType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClaimType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClaimTypeId,ClaimTypeName,IsSelected")] ClaimType claimType)
        {
            if (ModelState.IsValid)
            {
                db.ClaimTypes.Add(claimType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(claimType);
        }

        // GET: ClaimType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaimType claimType = db.ClaimTypes.Find(id);
            if (claimType == null)
            {
                return HttpNotFound();
            }
            return View(claimType);
        }

        // POST: ClaimType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClaimTypeId,ClaimTypeName,IsSelected")] ClaimType claimType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(claimType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(claimType);
        }

        // GET: ClaimType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaimType claimType = db.ClaimTypes.Find(id);
            if (claimType == null)
            {
                return HttpNotFound();
            }
            return View(claimType);
        }

        // POST: ClaimType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClaimType claimType = db.ClaimTypes.Find(id);
            db.ClaimTypes.Remove(claimType);
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

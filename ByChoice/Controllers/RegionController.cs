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
    public class RegionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Region
        public ActionResult Index()
        {
            return View(db.Regions.OrderBy(r=>r.RegionName).ToList());
        }

        // GET: Region/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Region region = db.Regions.Find(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return View(region);
        }

        // GET: Region/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Region/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RegionId,RegionName")] Region region)
        {
            if (ModelState.IsValid)
            {
                db.Regions.Add(region);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(region);
        }

        // GET: Region/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Region region = db.Regions.Find(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return View(region);
        }

        // POST: Region/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RegionId,RegionName")] Region region)
        {
            if (ModelState.IsValid)
            {
                db.Entry(region).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(region);
        }

        // GET: Region/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Region region = db.Regions.Find(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return View(region);
        }

        // POST: Region/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Region region = db.Regions.Find(id);
            db.Regions.Remove(region);
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

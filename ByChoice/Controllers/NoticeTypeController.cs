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
    public class NoticeTypeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NoticeType
        public ActionResult Index()
        {
            return View(db.NoticeTypes.ToList());
        }

        // GET: NoticeType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoticeType noticeType = db.NoticeTypes.Find(id);
            if (noticeType == null)
            {
                return HttpNotFound();
            }
            return View(noticeType);
        }

        // GET: NoticeType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NoticeType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NoticeTypeId,NoticeTypeName,IsSelected")] NoticeType noticeType)
        {
            if (ModelState.IsValid)
            {
                db.NoticeTypes.Add(noticeType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(noticeType);
        }

        // GET: NoticeType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoticeType noticeType = db.NoticeTypes.Find(id);
            if (noticeType == null)
            {
                return HttpNotFound();
            }
            return View(noticeType);
        }

        // POST: NoticeType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NoticeTypeId,NoticeTypeName,IsSelected")] NoticeType noticeType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(noticeType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(noticeType);
        }

        // GET: NoticeType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoticeType noticeType = db.NoticeTypes.Find(id);
            if (noticeType == null)
            {
                return HttpNotFound();
            }
            return View(noticeType);
        }

        // POST: NoticeType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NoticeType noticeType = db.NoticeTypes.Find(id);
            db.NoticeTypes.Remove(noticeType);
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

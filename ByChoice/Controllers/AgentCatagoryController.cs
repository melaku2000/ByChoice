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

    public class AgentCatagoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AgentCatagory
        public ActionResult Index()
        {
            return View(db.AgentCatagories.ToList());
        }

        // GET: AgentCatagory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentCatagory agentCatagory = db.AgentCatagories.Find(id);
            if (agentCatagory == null)
            {
                return HttpNotFound();
            }
            return View(agentCatagory);
        }

        // GET: AgentCatagory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AgentCatagory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AgentCatagoryId,AgentCatagoryType")] AgentCatagory agentCatagory)
        {
            if (ModelState.IsValid)
            {
                db.AgentCatagories.Add(agentCatagory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(agentCatagory);
        }

        // GET: AgentCatagory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentCatagory agentCatagory = db.AgentCatagories.Find(id);
            if (agentCatagory == null)
            {
                return HttpNotFound();
            }
            return View(agentCatagory);
        }

        // POST: AgentCatagory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AgentCatagoryId,AgentCatagoryType")] AgentCatagory agentCatagory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agentCatagory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agentCatagory);
        }

        // GET: AgentCatagory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentCatagory agentCatagory = db.AgentCatagories.Find(id);
            if (agentCatagory == null)
            {
                return HttpNotFound();
            }
            return View(agentCatagory);
        }

        // POST: AgentCatagory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AgentCatagory agentCatagory = db.AgentCatagories.Find(id);
            db.AgentCatagories.Remove(agentCatagory);
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

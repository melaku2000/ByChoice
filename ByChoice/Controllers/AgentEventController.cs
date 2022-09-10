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
    public class AgentEventController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AgentEvent
        public ActionResult Index()
        {
            var agentEvents = db.AgentEvents.Include(a => a.Agent).Include(a => a.Event);
            return View(agentEvents.ToList());
        }

        // GET: AgentEvent/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentEvent agentEvent = db.AgentEvents.Find(id);
            if (agentEvent == null)
            {
                return HttpNotFound();
            }
            return View(agentEvent);
        }

        // GET: AgentEvent/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.Users, "Id", "TaxNumber");
            ViewBag.EventId = new SelectList(db.Events, "EventId", "Location");
            return View();
        }

        // POST: AgentEvent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AgentEventId,EventId,Id")] AgentEvent agentEvent)
        {
            if (ModelState.IsValid)
            {
                db.AgentEvents.Add(agentEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.Users, "Id", "TaxNumber", agentEvent.Id);
            ViewBag.EventId = new SelectList(db.Events, "EventId", "Location", agentEvent.EventId);
            return View(agentEvent);
        }

        // GET: AgentEvent/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentEvent agentEvent = db.AgentEvents.Find(id);
            if (agentEvent == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "TaxNumber", agentEvent.Id);
            ViewBag.EventId = new SelectList(db.Events, "EventId", "Location", agentEvent.EventId);
            return View(agentEvent);
        }

        // POST: AgentEvent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AgentEventId,EventId,Id")] AgentEvent agentEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agentEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "TaxNumber", agentEvent.Id);
            ViewBag.EventId = new SelectList(db.Events, "EventId", "Location", agentEvent.EventId);
            return View(agentEvent);
        }

        // GET: AgentEvent/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentEvent agentEvent = db.AgentEvents.Find(id);
            if (agentEvent == null)
            {
                return HttpNotFound();
            }
            return View(agentEvent);
        }

        // POST: AgentEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AgentEvent agentEvent = db.AgentEvents.Find(id);
            db.AgentEvents.Remove(agentEvent);
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

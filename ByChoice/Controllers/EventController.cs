using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ByChoice.Models;

using System.Data.Entity.Infrastructure;
using ByChoice.ViewModels;
using ByChoice.DTOs;

namespace ByChoice.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EventController : Controller
    {
        private ApplicationDbContext db;

        public EventController()
        {
            db = new ApplicationDbContext();
            db.Configuration.LazyLoadingEnabled = false;

        }

        // GET: Event
        public ActionResult Index()
        {
            var events = db.Events.Include(e => e.EventType);
            return View(events.ToList());
        }

        // GET: Event/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            var model = new DetailEventViewModel
            {
                EventTypeId = @event.EventTypeId,
                Location = @event.Location,
                Detail = @event.Detail,
                PostDate = @event.PostDate,
                EventDate = @event.EventDate,

                //Agents=db.AgentEvents.Include(a=>a.Agent).Include(a=>a.Agent.AgentCatagory).Include(a=>a.Agent.ApplicationUser)
                //.Include(a=>a.Agent.ApplicationUser.Region).Where(a=>a.EventId== @event.EventId).Select(a=>a.Agent).ToList()
                Agents=db.AgentEvents.Include(a=>a.Agent).Where(a => a.EventId == @event.EventId)
                .Select(a => a.Agent).Include(u=>u.ApplicationUser).Include(u=>u.ApplicationUser.Region).Include(u=>u.AgentCatagory)
            };
            return View(model);
        }

        // GET: Event/Create
        public ActionResult Create()
        {
            ViewBag.EventTypeId = new SelectList(db.EventTypes, "EventTypeId", "EventName");

            var model = new CreateEventViewModel
            {
                Agents = db.Agents.Include(a=>a.ApplicationUser).Include(a=>a.ApplicationUser.Region).Include(a=>a.AgentCatagory).ToList()
            };
            return View(model);
        }

        // POST: Event/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "EventId,EventTypeId,PostDate,EventDate,Location,Detail,IsSelected")] Event @event)
        public ActionResult Create(CreateEventViewModel model)
        {
            if (model.Agents.Count(a=>a.ApplicationUser.IsSelected==true)>0)
            {
                var agents = model.Agents.Where(a => a.ApplicationUser.IsSelected == true);

                var eve = new Event
                {
                    EventTypeId=model.EventTypeId,
                    Location=model.Location,
                    Detail=model.Detail,
                    PostDate = DateTime.Now,
                    EventDate=model.EventDate
                 };
                
                db.Events.Add(eve);
                foreach(var a in agents)
                {
                    var agentEvent = new AgentEvent
                    {
                        EventId = eve.EventId,
                        Id = a.Id,
                        IsSelected = false
                    };
                    db.AgentEvents.Add(agentEvent);
                }
               // db.Events.Add(model.Event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventTypeId = new SelectList(db.EventTypes, "EventTypeId", "EventName", model.EventTypeId);
            var eventModel  = new CreateEventViewModel
            {
                //Event=model.Event,
                Agents = db.Agents.Include(a => a.ApplicationUser).Include(a => a.ApplicationUser.Region).Include(a => a.AgentCatagory).ToList()
            };
            return View(eventModel);
        }

        // GET: Event/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event ev = db.Events.Include(e => e.EventType).SingleOrDefault(e => e.EventId == id);
            if (ev == null)
            {
                return HttpNotFound();
            }
            Event model = db.Events.Include(i => i.EventType)
               .Include(i => i.AgentEvents).Where(i => i.EventId == id).Single();
            PopulateAssignedAgentData(model);
            if (model == null)
            {
                return HttpNotFound();
            }

            ViewBag.EventTypeId = new SelectList(db.EventTypes, "EventTypeId", "EventName", ev.EventTypeId);
            return View(model);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "EventId,EventTypeId,PostDate,EventDate,Location,Detail,IsSelected")] Event @event)
        public ActionResult Edit(int? id, string[] selectedAgent)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var eventToUpdate = db.Events
               .Include(i => i.AgentEvents)
               .Where(i => i.EventId == id)
               .Single();

            var agentEventToUpdate = db.AgentEvents.Include(a => a.Agent).Include(a => a.Event).Where(a => a.EventId == id);
            if (TryUpdateModel(eventToUpdate, "",
               new string[] { "EventTypeId", "EventDate", "Location" }))
            {
                try
                {
                    UpdateAgentEvent(selectedAgent, agentEventToUpdate, eventToUpdate);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedAgentData(eventToUpdate);

            return View(eventToUpdate);
        }
        private void UpdateAgentEvent (string[] selectedAgent, IEnumerable<AgentEvent> eventToUpdate,Event ev)
        {
            if (selectedAgent == null)
            {
                eventToUpdate = new List<AgentEvent>();
                return;
            }
            var agentEventToUpdate = db.AgentEvents; ;
            var selectedAgentHS = new HashSet<string>(selectedAgent);

            var agentEvent = new HashSet<string>
                (eventToUpdate.Select(c => c.Id));
           foreach(var agent in db.Agents.Include(a=>a.AgentEvents))
            {
                if (selectedAgentHS.Contains(agent.Id))
                {
                    if (!agentEvent.Contains(agent.Id))
                    {
                        var model = new AgentEvent
                        {
                            Id = agent.Id,
                            EventId = ev.EventId
                        };
                        agentEventToUpdate.Add(model);
                    }
                }
                else
                {
                    if (agentEvent.Contains(agent.Id))
                    {
                        agentEventToUpdate.Remove(agent.AgentEvents.Single());
                    }
                }
            }
        }

        // GET: Event/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event ev  = db.Events.Include(e => e.AgentEvents).Where(e => e.EventId == id).Single();
            Event @event = db.Events.Find(id);
            db.Events.Remove(ev);
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

        private void PopulateAssignedAgentData (Event eve)
        {
            var allAgent = db.Agents.Include(a=>a.AgentCatagory).Include(a=>a.ApplicationUser).Include(a=>a.ApplicationUser.Region);
           // var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseID));
            var agents = new HashSet<string>(eve.AgentEvents.Select(c => c.Id));
            var viewModel = new List<AgentEventDTO>();
            foreach (var agent in allAgent)
            {
                viewModel.Add(new AgentEventDTO
                {
                    Id = agent.Id,
                    EventId = eve.EventId,
                    AgentName = agent.AgentName,
                    AgentCatagoryType = agent.AgentCatagory.AgentCatagoryType,
                    FullName = agent.ApplicationUser.FullName,
                    RegionName = agent.ApplicationUser.Region.RegionName,
                    TaxNumber = agent.ApplicationUser.TaxNumber,
                    PhoneNumber = agent.ApplicationUser.PhoneNumber,
                    IsSelected = agents.Contains(agent.Id)
                    //Assigned = instructorCourses.Contains(course.CourseID)
                });
            }
            ViewBag.Agents = viewModel;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ByChoice.DTOs;
using ByChoice.Models;

namespace ByChoice.Controllers
{
    public class NoticeController : Controller
    {

        private ApplicationDbContext db;

        public NoticeController()
        {
            db = new ApplicationDbContext();
            db.Configuration.LazyLoadingEnabled = false;

        }

        // GET: Notice
        public ActionResult Index()
        {
            var notices = db.Notices.Include(n => n.NoticeType);
            return View(notices.ToList());
        }

        // GET: Notice/Details/5
        public ActionResult NoticeDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notice notice = db.Notices.Find(id);
            if (notice == null)
            {
                return HttpNotFound();
            }
            var model = new DetailNoticeViewModel
            {
                NoticeId=notice.NoticeId,
                NoticeTypeId = notice.NoticeTypeId,
                PostDate = notice.PostDate,
                Detail = notice.Detail,

                //Agents=db.AgentEvents.Include(a=>a.Agent).Include(a=>a.Agent.AgentCatagory).Include(a=>a.Agent.ApplicationUser)
                //.Include(a=>a.Agent.ApplicationUser.Region).Where(a=>a.EventId== @event.EventId).Select(a=>a.Agent).ToList()
                Agents = db.AgentNotices.Include(a => a.Agent).Where(a => a.NoticeId == notice.NoticeId)
                .Select(a => a.Agent).Include(u => u.ApplicationUser).Include(u => u.ApplicationUser.Region).Include(u => u.AgentCatagory)
            };
            return View(model);
        }

        // GET: Notice/Create
        public ActionResult Create()
        {
            ViewBag.NoticeTypeId = new SelectList(db.NoticeTypes, "NoticeTypeId", "NoticeTypeName");
            var model = new CreateNoticeViewModel
            {
                Agents = db.Agents.Include(a => a.ApplicationUser).Include(a => a.ApplicationUser.Region).Include(a => a.AgentCatagory).ToList()
            };
            return View(model);
        }

        // POST: Notice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateNoticeViewModel model)
        {
            if (model.Agents.Count(a => a.ApplicationUser.IsSelected == true) > 0)
            {
                var agents = model.Agents.Where(a => a.ApplicationUser.IsSelected == true);

                var notice = new Notice
                {
                    NoticeTypeId = model.NoticeTypeId,
                    Detail = model.Detail,
                    PostDate = DateTime.Now,
                };

                db.Notices.Add(notice);
                foreach (var a in agents)
                {
                    var agentNotice = new AgentNotice
                    {
                        NoticeId = notice.NoticeId,
                        Id = a.Id,
                    };
                    db.AgentNotices.Add(agentNotice);
                }
                // db.Events.Add(model.Event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.NoticeTypeId = new SelectList(db.NoticeTypes, "NoticeTypeId", "NoticeTypeName", model.NoticeTypeId);
            var eventModel = new CreateNoticeViewModel
            {
                //Event=model.Event,
                Agents = db.Agents.Include(a => a.ApplicationUser).Include(a => a.ApplicationUser.Region).Include(a => a.AgentCatagory).ToList()
            };
            return View(eventModel);
        }

        // GET: Notice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notice notice = db.Notices.Include(e => e.NoticeType).SingleOrDefault(e => e.NoticeId == id);
            if (notice == null)
            {
                return HttpNotFound();
            }
            Notice model = db.Notices.Include(i => i.NoticeType)
               .Include(i => i.AgentNotices).Where(i => i.NoticeId == id).Single();
            PopulateAssignedAgentData(model);
            if (model == null)
            {
                return HttpNotFound();
            }

            ViewBag.NoticeTypeId = new SelectList(db.NoticeTypes, "NoticeTypeId", "NoticeTypeName", notice.NoticeTypeId);
            return View(model);
        }

        // POST: Notice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedAgent)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var noticeToUpdate = db.Notices
               .Include(i => i.AgentNotices)
               .Where(i => i.NoticeId == id)
               .Single();

            var agentNoticeToUpdate  = db.AgentNotices.Include(a => a.Agent).Include(a => a.Notice).Where(a => a.NoticeId == id);
            if (TryUpdateModel(noticeToUpdate, "",
               new string[] { "NoticeTypeId", "PostDate", "Detail" }))
            {
                try
                {
                    UpdateAgentEvent(selectedAgent, agentNoticeToUpdate, noticeToUpdate) ;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedAgentData(noticeToUpdate);

            return View(noticeToUpdate);
        }

        // GET: Notice/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notice notice = db.Notices.Find(id);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }

        // POST: Notice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Notice notice = db.Notices.Find(id);
            db.Notices.Remove(notice);
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

        private void PopulateAssignedAgentData(Notice notice )
        {
            var allAgent = db.Agents.Include(a => a.AgentCatagory).Include(a => a.ApplicationUser).Include(a => a.ApplicationUser.Region);
            var agents = new HashSet<string>(notice.AgentNotices.Select(c => c.Id));
            var viewModel = new List<NoticeDTOs>();
            foreach (var agent in allAgent)
            {
                viewModel.Add(new NoticeDTOs
                {
                    Id = agent.Id,
                    NoticeId = notice.NoticeId,
                    AgentName = agent.AgentName,
                    AgentCatagoryType = agent.AgentCatagory.AgentCatagoryType,
                    FullName = agent.ApplicationUser.FullName,
                    RegionName = agent.ApplicationUser.Region.RegionName,
                    TaxNumber = agent.ApplicationUser.TaxNumber,
                    PhoneNumber = agent.ApplicationUser.PhoneNumber,
                    IsSelected = agents.Contains(agent.Id)
                });
            }
            ViewBag.Agents = viewModel;
        }

        private void UpdateAgentEvent(string[] selectedAgent, IEnumerable<AgentNotice> noticeToUpdate, Notice ev)
        {
            if (selectedAgent == null)
            {
                noticeToUpdate = new List<AgentNotice>();
                return;
            }
            var agentNoticeToUpdate = db.AgentNotices;

            var selectedAgentHS = new HashSet<string>(selectedAgent);

            var agentNotice = new HashSet<string>
                (noticeToUpdate.Select(c => c.Id));
            foreach (var agent in db.Agents.Include(a => a.AgentNotices))
            {
                if (selectedAgentHS.Contains(agent.Id))
                {
                    if (!agentNotice.Contains(agent.Id))
                    {
                        var model = new AgentNotice
                        {
                            Id = agent.Id,
                            NoticeId = ev.NoticeId
                        };
                        agentNoticeToUpdate.Add(model);
                    }
                }
                else
                {
                    if (agentNotice.Contains(agent.Id))
                    {
                        agentNoticeToUpdate.Remove(agent.AgentNotices.Single());
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ByChoice.Models;
using ByChoice.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;

namespace ByChoice.Controllers
{
    public class AgentController : Controller
    {
        private ApplicationDbContext db;

        public AgentController()
        {
            db = new ApplicationDbContext();
            db.Configuration.LazyLoadingEnabled = false;

        }
        // GET: Agent
        [Authorize(Roles = "Admin")]
        public ViewResult Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            // IEnumerable<Product> products;
            ViewBag.CurrentFilter = searchString;

            var agents = from s in db.Agents
                            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                agents = agents.Where(s => s.ApplicationUser.PhoneNumber.Contains(searchString));
            }
            int pageSize = 20;
            int pageNumber = (page ?? 1);


            //var agents = db.Agents.Include(a => a.AgentCatagory).Include(a => a.ApplicationUser).Include(a => a.ApplicationUser.Region);

            return View(agents.OrderBy(p => p.AgentName).Include(p => p.ApplicationUser).Include(p=>p.ApplicationUser.Region).ToPagedList(pageNumber, pageSize));

           // return View(agents.ToList());
        }

        [Authorize(Roles = "Agent,Admin")]
        // GET: Agent/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent =db.Agents.Include(a => a.AgentCatagory).Include(a => a.ApplicationUser).Include(a => a.ApplicationUser.Region).Single(a=>a.Id==id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);
        }


        // GET: Agent/Create
        [Authorize(Roles = "Admin")]
        public ActionResult CreateAgent(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            // IEnumerable<Product> products;
            ViewBag.CurrentFilter = searchString;

            var agents = from s in db.Users
                         select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                agents = agents.Where(s => s.PhoneNumber.Contains(searchString));
            }
            int pageSize = 20;
            int pageNumber = (page ?? 1);


            //var agents = db.Agents.Include(a => a.AgentCatagory).Include(a => a.ApplicationUser).Include(a => a.ApplicationUser.Region);

            return View(agents.OrderBy(a=>a.RegesterDate).Include(a => a.Region).Where(u => !db.Agents.Any(a => a.Id == u.Id)).Where(a => a.Email != "melakumen@gmail.com").ToPagedList(pageNumber, pageSize));

            //var model = db.Users.Include(u=>u.Region).Where(u => !db.Agents.Any(a => a.Id == u.Id)).Where(a => a.Email != "melakumen@gmail.com");
            //return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create(string Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var agent = db.Users.SingleOrDefault(u=>u.Id==Id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = agent;
            ViewBag.AgentCatagoryId = new SelectList(db.AgentCatagories, "AgentCatagoryId", "AgentCatagoryType");
            //var model = db.Users.Where(u => u.EmailConfirmed == true).Where(u => !db.Agents.Any(a => a.Id == u.Id)).Where(a => a.Email != "melakumen@gmail.com");
            //var agent = (from user in model
            //             select new
            //             {
            //                 UserId = user.Id,
            //                 FName = user.FirstName + " " + user.LastName,
            //                 Role = (from userRole in user.Roles join role in db.Roles on userRole.RoleId equals role.Id select role.Name).ToList()
            //             }).ToList().Select(p => new RoleViewModels()
            //             {
            //                 Id = p.UserId,
            //                 FullName = p.FName,
            //                 Roles = string.Join(",", p.Role)
            //             }).Where(p => p.Roles == "Agent");
            //ViewBag.AgentCatagoryId = new SelectList(db.AgentCatagories, "AgentCatagoryId", "AgentCatagoryType");
            //// ViewBag.Id = new SelectList(db.Users, "Id", "TaxNumber");
            //ViewBag.Id = new SelectList(agent, "Id", "FullName");
            return View();
        }
        // POST: Agent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,AgentName,AgentCatagoryId,Photo,Latitude,Longitude,ImageData")] Agent agent)
        {
            HttpPostedFileBase file = Request.Files["ImageData"];
            if (file != null)
            {
                BinaryReader reader = new BinaryReader(file.InputStream);
                agent.Photo = reader.ReadBytes((int)file.ContentLength);
            }
                if (ModelState.IsValid)
            {
                db.Agents.Add(agent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AgentCatagoryId = new SelectList(db.AgentCatagories, "AgentCatagoryId", "AgentCatagoryType", agent.AgentCatagoryId);
            ViewBag.Id = new SelectList(db.Users, "Id", "FullName", agent.Id);
            return View(agent);
        }

        [Authorize(Roles = "Agent,Admin")]
        // GET: Agent/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = db.Agents.Find(id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgentCatagoryId = new SelectList(db.AgentCatagories, "AgentCatagoryId", "AgentCatagoryType", agent.AgentCatagoryId);
            ViewBag.Id = new SelectList(db.Users, "Id", "TaxNumber", agent.Id);
            return View(agent);
        }

        // POST: Agent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Agent,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AgentName,AgentCatagoryId,Photo,Latitude,Longitude")] Agent agent)
        //public ActionResult Edit(HttpPostedFileBase files,Agent agent)
        {
            HttpPostedFileBase file = Request.Files["ImageData"];

            if (file != null)
            {
                BinaryReader reader = new BinaryReader(file.InputStream);
                agent.Photo = reader.ReadBytes((int)file.ContentLength);
            }
            if (ModelState.IsValid)
            {
                db.Entry(agent).State = EntityState.Modified;
                db.SaveChanges();
                if (User.IsInRole("Agent"))
                {
                    return  RedirectToAction("Detail","Agent",new { id = User.Identity.GetUserId() });
                }
                return RedirectToAction("Index");
            }
            ViewBag.AgentCatagoryId = new SelectList(db.AgentCatagories, "AgentCatagoryId", "AgentCatagoryType", agent.AgentCatagoryId);
            ViewBag.Id = new SelectList(db.Users, "Id", "TaxNumber", agent.Id);
            return View(agent);
        }

        // GET: Agent/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = db.Agents.Find(id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);
        }

        // POST: Agent/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Agent agent = db.Agents.Find(id);
            db.Agents.Remove(agent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Agent,Admin")]
        public ActionResult RetrieveImage(string id)
        {
            var q = from temp in db.Agents where temp.Id == id select temp.Photo;
            if (q != null)
            {
                byte[] cover = q.First();
                if (cover != null)
                {
                    return File(cover, "image/jpg");
                }
                else
                {
                    return Content("We appologize the file you request is not found");
                }
            }
            return null;
        }
        [Authorize(Roles = "Agent")]
        public ActionResult EventDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var eve = db.AgentEvents.Include(e => e.Event).Include(e => e.Event.EventType).SingleOrDefault(e => e.AgentEventId == id);
            if (eve == null)
            {
                return HttpNotFound();
            }
           
            return View(eve);
        }
        [Authorize(Roles = "Agent")]
        public ActionResult Event()
        {
            var user = User.Identity.GetUserId();
            return View(db.AgentEvents.Include(e => e.Event).Include(e=>e.Event.EventType).Where(e => e.Id == user));
        }

        // GET: Notice
        [Authorize(Roles = "Agent")]
        public ActionResult Notice()
        {
            var user = User.Identity.GetUserId();

            return View(db.AgentNotices.Include(e => e.Notice).Include(e => e.Notice.NoticeType).Where(e => e.Id == user));
        }

        [Authorize(Roles = "Agent")]
        public ActionResult NoticeDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var eve = db.AgentNotices.Include(e => e.Notice).Include(e => e.Notice.NoticeType).SingleOrDefault(e => e.AgentNoitceId == id);
            if (eve == null)
            {
                return HttpNotFound();
            }

            return View(eve);
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

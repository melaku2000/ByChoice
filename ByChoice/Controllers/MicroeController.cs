using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ByChoice.Models;
using ByChoice.ViewModels;
using PagedList;

namespace ByChoice.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MicroeController : Controller
    {
        private ApplicationDbContext db;

        public MicroeController()
        {
            db = new ApplicationDbContext();
            db.Configuration.LazyLoadingEnabled = false;
        }
        // GET: Microe
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

            var micros  = from s in db.Micros
                         select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                micros = micros.Where(s => s.ApplicationUser.PhoneNumber.Contains(searchString));
            }
            int pageSize = 20;
            int pageNumber = (page ?? 1);


            //var agents = db.Agents.Include(a => a.AgentCatagory).Include(a => a.ApplicationUser).Include(a => a.ApplicationUser.Region);

            return View(micros.OrderBy(p => p.MicroName).Include(p => p.ApplicationUser).Include(p => p.ApplicationUser.Region).ToPagedList(pageNumber, pageSize));

            // return View(agents.ToList());
        }

        // GET: Microe/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Micro micro  = db.Micros.Include(a => a.ApplicationUser).Include(a => a.ApplicationUser.Region).Single(a => a.Id == id);

            if (micro == null)
            {
                return HttpNotFound();
            }
            return View(micro);
        }

        // GET: Microe/Create
        public ActionResult Create()
        {
            var model = db.Users.Where(u => !db.Micros.Any(a => a.Id == u.Id)).Where(a => a.Email != "melakumen@gmail.com");
            var agent = (from user in model
                         select new
                         {
                             UserId = user.Id,
                             FName = user.FirstName + " " + user.LastName,
                             Role = (from userRole in user.Roles join role in db.Roles on userRole.RoleId equals role.Id select role.Name).ToList()
                         }).ToList().Select(p => new RoleViewModels()
                         {
                             Id = p.UserId,
                             FullName = p.FName,
                             Roles = string.Join(",", p.Role)
                         }).Where(p => p.Roles == "MicroFinance");
            ViewBag.Id = new SelectList(agent, "Id", "FullName");
            return View();
        }

        // POST: Microe/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MicroName,IntrestRate,Photo,Latitude,Longitude")] Micro micro)
        {
            HttpPostedFileBase file = Request.Files["ImageData"];
            if (file != null)
            {
                BinaryReader reader = new BinaryReader(file.InputStream);
                micro.Photo = reader.ReadBytes((int)file.ContentLength);
            }
            if (ModelState.IsValid)
            {
                db.Micros.Add(micro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.Users, "Id", "TaxNumber", micro.Id);
            return View(micro);
        }

        // GET: Microe/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Micro micro = db.Micros.Find(id);
            if (micro == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "TaxNumber", micro.Id);
            return View(micro);
        }

        // POST: Microe/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MicroName,IntrestRate,Photo,Latitude,Longitude")] Micro micro)
        {
            HttpPostedFileBase file = Request.Files["ImageData"];

            if (file != null)
            {
                BinaryReader reader = new BinaryReader(file.InputStream);
                micro.Photo = reader.ReadBytes((int)file.ContentLength);
            }
            if (ModelState.IsValid)
            {
                db.Entry(micro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "TaxNumber", micro.Id);
            return View(micro);
        }

        // GET: Microe/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Micro micro = db.Micros.Find(id);
            if (micro == null)
            {
                return HttpNotFound();
            }
            return View(micro);
        }

        // POST: Microe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Micro micro = db.Micros.Find(id);
            db.Micros.Remove(micro);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RetrieveImage(string id)
        {
            var q = from temp in db.Micros where temp.Id == id select temp.Photo;
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ByChoice.Models;
using PagedList;

namespace ByChoice.Controllers
{
    public class OnGoController : Controller
    {
        private ApplicationDbContext db;

        public OnGoController()
        {
            db = new ApplicationDbContext();
            db.Configuration.LazyLoadingEnabled = false;

        }
        // GET: OnGo
        public ActionResult AgentLocator(int? currentFilter, int? RegionId, int? page)
        {
            if (RegionId != null)
            {
                page = 1;
            }
            else
            {
                RegionId = currentFilter;
            }
            // IEnumerable<Product> products;
            ViewBag.CurrentFilter = RegionId;

            var agents = from s in db.Agents
                         select s;

            if (!String.IsNullOrEmpty(RegionId.ToString()))
            {
                agents = agents.Where(s => s.ApplicationUser.RegionId==RegionId);
            }
            int pageSize = 20;
            int pageNumber = (page ?? 1);


            ViewBag.RegionId = new SelectList(db.Regions, "RegionId", "RegionName");

            return View(agents.OrderBy(p => p.AgentName).Include(p => p.ApplicationUser).Include(p => p.AgentCatagory).Include(p => p.ApplicationUser.Region).ToPagedList(pageNumber, pageSize));

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
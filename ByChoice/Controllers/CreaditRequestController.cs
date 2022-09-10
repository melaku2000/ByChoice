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
    /*
    public class CreaditRequestController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CreaditRequest
        public ActionResult Index()
        {
            var creaditRequests = db.CreaditRequests.Include(c => c.Agent).Include(c => c.Customer).Include(c => c.Micro);
            return View(creaditRequests.ToList());
        }

        // GET: CreaditRequest/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreaditRequest creaditRequest = db.CreaditRequests.Find(id);
            if (creaditRequest == null)
            {
                return HttpNotFound();
            }
            return View(creaditRequest);
        }

        //// GET: CreaditRequest/Create
        //public ActionResult Create()
        //{
        //    ViewBag.AgentId = new SelectList(db.Agents, "Id", "AgentName");
        //    ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName");
        //    ViewBag.MicroId = new SelectList(db.Micros, "Id", "MicroName");
        //    return View();
        //}
        
        // GET: CreaditRequest/Create
        public ActionResult Create(CreaditRequestViewModel model,string Id)
        {
            if(Id==null)
                model = (CreaditRequestViewModel)TempData["CreaditRequest"];
            //CreaditRequestViewModel VMCreadit =(CreaditRequestViewModel) TempData["CreaditRequest"];
            List<ProductViewModel> viewModels = new List<ProductViewModel>();

            if (model.ProductViewModels.Count() > 0)
            {
                //foreach(var product in model.Products)
                //{
                //    var proModel = db.Products.Include(p => p.ProductModel).SingleOrDefault(p => p.ProductId == p.ProductId);
                //    if (proModel != null)
                //    {
                //        viewModels.Add(new ProductViewModel {
                //            ProductModelId=proModel.ProductModelId,
                //            Serial=proModel.Serial,
                //            ManufacturedDate=proModel.ManufacturedDate,
                //            BrandCompanyName=proModel.ProductModel.ModelName,
                //            Price=proModel.ProductModel.Price
                //        });
                //    }
                //}
                var vm = new CreaditRequestViewModel
                {
                    Customer = model.Customer,
                    ProductViewModels = viewModels,
                    Micro = db.Micros.Include(m => m.ApplicationUser).Include(m => m.ApplicationUser.Region).FirstOrDefault()
                };
            }
            
            ViewBag.AgentId = new SelectList(db.Agents, "Id", "AgentName");
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName");
            ViewBag.MicroId = new SelectList(db.Micros, "Id", "MicroName");
            return View();
        }

        // POST: CreaditRequest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CreaditRequestId,CustomerId,AgentId,MicroId,RequestDate,RequestAmount,Decline,Approved,ApprovedDeclineDate,CreaditConfirmed,Delivered,New,IsSelected")] CreaditRequest creaditRequest)
        {
            if (ModelState.IsValid)
            {
                db.CreaditRequests.Add(creaditRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AgentId = new SelectList(db.Agents, "Id", "AgentName", creaditRequest.AgentId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", creaditRequest.CustomerId);
            ViewBag.MicroId = new SelectList(db.Micros, "Id", "MicroName", creaditRequest.MicroId);
            return View(creaditRequest);
        }

        // GET: CreaditRequest/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreaditRequest creaditRequest = db.CreaditRequests.Find(id);
            if (creaditRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgentId = new SelectList(db.Agents, "Id", "AgentName", creaditRequest.AgentId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", creaditRequest.CustomerId);
            ViewBag.MicroId = new SelectList(db.Micros, "Id", "MicroName", creaditRequest.MicroId);
            return View(creaditRequest);
        }

        // POST: CreaditRequest/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CreaditRequestId,CustomerId,AgentId,MicroId,RequestDate,RequestAmount,Decline,Approved,ApprovedDeclineDate,CreaditConfirmed,Delivered,New,IsSelected")] CreaditRequest creaditRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(creaditRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgentId = new SelectList(db.Agents, "Id", "AgentName", creaditRequest.AgentId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", creaditRequest.CustomerId);
            ViewBag.MicroId = new SelectList(db.Micros, "Id", "MicroName", creaditRequest.MicroId);
            return View(creaditRequest);
        }

        // GET: CreaditRequest/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreaditRequest creaditRequest = db.CreaditRequests.Find(id);
            if (creaditRequest == null)
            {
                return HttpNotFound();
            }
            return View(creaditRequest);
        }

        // POST: CreaditRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CreaditRequest creaditRequest = db.CreaditRequests.Find(id);
            db.CreaditRequests.Remove(creaditRequest);
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
    */
}

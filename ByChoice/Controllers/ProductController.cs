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
using Microsoft.AspNet.Identity;
using PagedList;

namespace ByChoice.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db;

        public ProductController()
        {
            db = new ApplicationDbContext();
            db.Configuration.LazyLoadingEnabled = false;

        }
        // GET: Product
        [Authorize(Roles = "Admin,Agent")]
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
            var user = User.Identity.GetUserId();
            var products = from s in db.Products
                           select s;
           
                if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Serial.Contains(searchString));
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            if (User.IsInRole("Agent"))
            {
                return View(products.OrderBy(s => s.ManufacturedDate).Include(s => s.ProductModel).Include(s => s.Agent).Where(s=>s.Agent.Id==user).Where(s=>s.Sold==false).ToPagedList(pageNumber, pageSize));
            }
            return View(products.OrderBy(s =>s.ManufacturedDate).Include(s => s.ProductModel).Include(s => s.Agent).ToPagedList(pageNumber, pageSize));
        }

        // GET: Product/Details/5
        [Authorize(Roles = "Admin,Agent")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Include(s => s.ProductModel).Include(s => s.Agent).SingleOrDefault(s=>s.ProductId==id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var model = new ProductDetailViewModel
            {
                Product=product,
                ProductHistories=db.ProductHistoryLists
                .Include(h=>h.ProductHistory)
                .Include(h=>h.ProductHistory.WarrantyTransfer)
                .Include(h=>h.ProductHistory.WarrantyTransfer.ToAgent)
                .Include(h=>h.ProductHistory.WarrantyTransfer.FromAgent)
                .Where(h=>h.ProductId==product.ProductId)
            };
            return View(model);
        }

        // GET: Product/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.AgentId = new SelectList(db.Agents, "Id", "AgentName");
            ViewBag.ProductModelId = new SelectList(db.ProductModels, "ProductModelId", "ModelName");
            return View();
        }


        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ProductId,AgentId,ProductModelId,Serial,ManufacturedDate,Sold,IsSelected")] Product product)
        {
            product.Serial = product.Serial.ToUpper();
            if (ModelState.IsValid)
            {
                    db.Products.Add(product);
                    db.SaveChanges();
                
                return RedirectToAction("Index");
            }

            ViewBag.AgentId = new SelectList(db.Agents, "Id", "AgentName", product.AgentId);
            ViewBag.ProductModelId = new SelectList(db.ProductModels, "ProductModelId", "ProductName", product.ProductModelId);
            return View(product);
        }

        // GET: Product/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgentId = new SelectList(db.Agents, "Id", "AgentName", product.AgentId);
            ViewBag.ProductModelId = new SelectList(db.ProductModels, "ProductModelId", "ProductName", product.ProductModelId);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ProductId,AgentId,ProductModelId,Serial,ManufacturedDate,Sold,IsSelected")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgentId = new SelectList(db.Agents, "Id", "AgentName", product.AgentId);
            ViewBag.ProductModelId = new SelectList(db.ProductModels, "ProductModelId", "ProductName", product.ProductModelId);
            return View(product);
        }

        // GET: Product/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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

        //
        // Upload Product List
        //
        [Authorize(Roles = "Admin")]
        public ActionResult UploadProductList()
        {
            ViewBag.AgentId = new SelectList(db.Agents, "Id", "AgentName");
            ViewBag.ProductModelId = new SelectList(db.ProductModels, "ProductModelId", "ModelName");
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult UploadProductList([Bind(Include = "AgentId,ProductModelId,PreFix,InitialNo,ManufacturedDate,Quantity")] BulkProductInsertViewModel product)
        {
            if (!ModelState.IsValid)
            {
                return HttpNotFound();
            }
            List<Product> model = new List<Product>();
            if (product.Quantity > 0)
            {
                string agent = product.AgentId;
                int productModel = product.ProductModelId;
                string prefix = product.PreFix;
                int initial = product.InitialNo;
                int final = product.InitialNo + product.Quantity;
                for(int i=product.InitialNo;i< final; i++)
                {
                    db.Products.Add
                        (
                        new Product {
                            AgentId =agent,
                            ProductModelId=productModel,
                            Serial=prefix+i,
                            ManufacturedDate=product.ManufacturedDate
                        });
                }

                db.SaveChanges();
            }
            return RedirectToAction("Index", "Product");
        }
    }
}

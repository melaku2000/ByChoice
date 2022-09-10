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
    public class OrderListController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: OrderList
        public ActionResult Index()
        {
            var orderLists = db.OrderLists.Include(o => o.Order).Include(o => o.ProductModel);
            return View(orderLists.ToList());
        }

        // GET: OrderList/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderList orderList = db.OrderLists.Find(id);
            if (orderList == null)
            {
                return HttpNotFound();
            }
            return View(orderList);
        }

        // GET: OrderList/Create
        public ActionResult Create()
        {
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "Id");
            ViewBag.ProductModelId = new SelectList(db.ProductModels, "ProductModelId", "ProductName");
            return View();
        }

        // POST: OrderList/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderListId,OrderId,ProductModelId,Quantity")] OrderList orderList)
        {
            if (ModelState.IsValid)
            {
                db.OrderLists.Add(orderList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "Id", orderList.OrderId);
            ViewBag.ProductModelId = new SelectList(db.ProductModels, "ProductModelId", "ProductName", orderList.ProductModelId);
            return View(orderList);
        }

        // GET: OrderList/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderList orderList = db.OrderLists.Find(id);
            if (orderList == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "Id", orderList.OrderId);
            ViewBag.ProductModelId = new SelectList(db.ProductModels, "ProductModelId", "ProductName", orderList.ProductModelId);
            return View(orderList);
        }

        // POST: OrderList/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderListId,OrderId,ProductModelId,Quantity")] OrderList orderList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "Id", orderList.OrderId);
            ViewBag.ProductModelId = new SelectList(db.ProductModels, "ProductModelId", "ProductName", orderList.ProductModelId);
            return View(orderList);
        }

        // GET: OrderList/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderList orderList = db.OrderLists.Find(id);
            if (orderList == null)
            {
                return HttpNotFound();
            }
            return View(orderList);
        }

        // POST: OrderList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderList orderList = db.OrderLists.Find(id);
            db.OrderLists.Remove(orderList);
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ByChoice.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ByChoice.Controllers
{

    public class WarrantyTransferController : Controller
    {
        private ApplicationDbContext db;

        public WarrantyTransferController()
        {
            db = new ApplicationDbContext();
            db.Configuration.LazyLoadingEnabled = false;

        }

        // GET: WarrantyTransfer
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

            return View(agents.OrderBy(p => p.AgentName).Include(p => p.ApplicationUser).Include(p => p.ApplicationUser.Region).ToPagedList(pageNumber, pageSize));

            // return View(agents.ToList());
        }

        // GET: WarrantyTransfer/Details/5
        public ActionResult GenerateTransferCode(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent FraomAgent = db.Agents.Find(id);
            if (FraomAgent == null)
            {
                return HttpNotFound();
            }
            var user = User.Identity.GetUserId();
            Agent ToAgent = db.Agents.Find(user);
            if (ToAgent == null)
            {
                return HttpNotFound();
            }
            var warrantyT = db.WarrantyTransfers.SingleOrDefault(w => w.ToAgentId == user);

            try
            {
                if (warrantyT != null)
                {
                    warrantyT.ToAgent = ToAgent;
                    warrantyT.ToAgentId = ToAgent.Id;
                    warrantyT.FromAgentId = FraomAgent.Id;
                    warrantyT.RequestDate = DateTime.Now;
                    warrantyT.TransferCode = GCode();

                    db.SaveChanges();
                    return View(db.WarrantyTransfers.Include(w=>w.ToAgent).Include(w=>w.FromAgent).SingleOrDefault(w=>w.WarrantyTransferId==warrantyT.WarrantyTransferId));
                }
                else
                {
                    var transfer = new WarrantyTransfer
                    {
                        FromAgentId = FraomAgent.Id,
                        ToAgentId = ToAgent.Id,
                        ToAgent=ToAgent,
                        RequestDate = DateTime.Now,
                        TransferCode = GCode()
                    };
                    db.WarrantyTransfers.Add(transfer);

                    db.SaveChanges();
                    return View(transfer);
                }
            }
            catch (Exception)
            {

                throw;
            }
            //return View(warrantyTransfer);
        }

        // GET: Transfer Code
        public ActionResult InsertCode(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent ToAgent = db.Agents.Find(id);
            if (ToAgent == null)
            {
                return HttpNotFound();
            }
            var user = User.Identity.GetUserId();
            Agent FromAgent = db.Agents.Find(user);
            if (FromAgent == null)
            {
                return HttpNotFound();
            }
            
                var transfer = new WarrantyTransfer
                {
                    FromAgent = FromAgent,
                    FromAgentId=FromAgent.Id,
                    ToAgent = ToAgent,
                    ToAgentId = ToAgent.Id,
                };
            
            return View(transfer);
        }


        // POST: WarrantyTransfer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertCode([Bind(Include = "WarrantyTransferId,ToAgentId,FromAgentId,RequestDate,TransferCode")] WarrantyTransfer warrantyTransfer)
        {
            ViewBag.TransferWarranty = null;

            if (!string.IsNullOrWhiteSpace( warrantyTransfer.TransferCode))
            {
                if (warrantyTransfer.ToAgentId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Agent ToAgent = db.Agents.Find(warrantyTransfer.ToAgentId);
                if (ToAgent == null)
                {
                    return HttpNotFound();
                }
                var user = User.Identity.GetUserId();
                Agent FraomAgent = db.Agents.Find(user);
                if (FraomAgent == null)
                {
                    return HttpNotFound();
                }
                var transfer = db.WarrantyTransfers.Where(w => w.ToAgentId == ToAgent.Id).Where(w => w.TransferCode == warrantyTransfer.TransferCode).SingleOrDefault();
                if (transfer != null)
                {
                    var warranty = new ConfermTransferViewModel
                    {
                        WarrantyTransfer=transfer,
                        ToAgent=transfer.ToAgent,
                        Products= db.Products.OrderBy(p => p.ManufacturedDate).Include(s => s.ProductModel).Include(s => s.Agent).Where(s => s.Agent.Id == user).Where(s => s.Sold == false).ToList()
                    };
                    //TempData["TransferWarranty"] = transfer;
                    // TempData.Keep();
                    //return RedirectToAction("TransferWarranty", warranty);
                    return View("TransferWarranty", warranty);

                    //return View("TransferWarranty", warranty);

                }
            }
            //ViewBag.ToAgentId = new SelectList(db.Agents, "Id", "AgentName", warrantyTransfer.ToAgentId);
            return View(warrantyTransfer);
        }
        public ActionResult TransferWarranty(ConfermTransferViewModel model)
        {
            var user = User.Identity.GetUserId();

            var warranty = new ConfermTransferViewModel
            {
                WarrantyTransfer = model.WarrantyTransfer,
                ToAgent = model.ToAgent,
                Products = db.Products.OrderBy(p => p.ManufacturedDate).Include(s => s.ProductModel).Include(s => s.Agent).Where(s => s.Agent.Id == user).ToList()
            };
            //ViewBag.First = TempData["TransferWarranty"];
            //TempData.Keep();

            if (model != null)
            {
                return View(model);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // Conferm Transfer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfermTransfer(ConfermTransferViewModel model)
        {

            if (model.Products.Where(p => p.IsSelected == true).Count() > 0)
            {
                var viewMode = new ConfermTransferViewModel
                {
                    WarrantyTransfer=db.WarrantyTransfers.Include(w=>w.ToAgent).Include(w=>w.ToAgent.AgentCatagory).SingleOrDefault(w=>w.WarrantyTransferId==model.WarrantyTransfer.WarrantyTransferId),
                    Products= model.Products.Where(p => p.IsSelected == true).ToList()
                };

                return View(viewMode);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult ConfermTransfered(ConfermTransferViewModel model)
        {

            if (model.Products.Count() > 0)
            {
                if (model != null)
                {

                    var toAgent = db.Agents.SingleOrDefault(a => a.Id == model.WarrantyTransfer.ToAgentId).Id;
                    if (toAgent != null)
                    {
                        var proHistory = new ProductHistory
                        {
                            WarrantyTransferId = model.WarrantyTransfer.WarrantyTransferId,
                            TransferdTime = DateTime.Now
                        };
                        db.ProductHistories.Add(proHistory);

                        foreach (var p in model.Products)
                        {
                            db.ProductHistoryLists.Add(
                                new ProductHistoryList
                                {
                                    ProductHistoryId = proHistory.ProductHistoryId,
                                    ProductId = p.ProductId
                                });
                            var product = db.Products.SingleOrDefault(pro => pro.ProductId == p.ProductId);
                            if (product != null)
                                product.AgentId = toAgent;
                        }

                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        return RedirectToAction("Index");
                    }

                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: WarrantyTransfer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarrantyTransfer warrantyTransfer = db.WarrantyTransfers.Find(id);
            if (warrantyTransfer == null)
            {
                return HttpNotFound();
            }
            ViewBag.ToAgentId = new SelectList(db.Agents, "Id", "AgentName", warrantyTransfer.ToAgentId);
            return View(warrantyTransfer);
        }


        //// POST: WarrantyTransfer/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "WarrantyTransferId,ToAgentId,FromAgentId,RequestDate,TransferCode")] WarrantyTransfer warrantyTransfer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(warrantyTransfer).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ToAgentId = new SelectList(db.Agents, "Id", "AgentName", warrantyTransfer.ToAgentId);
        //    return View(warrantyTransfer);
        //}

        //// GET: WarrantyTransfer/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    WarrantyTransfer warrantyTransfer = db.WarrantyTransfers.Find(id);
        //    if (warrantyTransfer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(warrantyTransfer);
        //}

        //// POST: WarrantyTransfer/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    WarrantyTransfer warrantyTransfer = db.WarrantyTransfers.Find(id);
        //    db.WarrantyTransfers.Remove(warrantyTransfer);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public string GCode()
        {
            
                    var RandomCode = new Random();
                    var Buffer = new char[6];
                    for (var i = 0; i < 6; i++)
                    {
                        Buffer[i] = (char)('A' + RandomCode.Next(0, 26));
                    }

            return string.Join("", Buffer);
        }

    }
}

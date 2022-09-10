using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
    public class CustomerController : Controller
    {
        private ApplicationDbContext db;

        public CustomerController()
        {
            db = new ApplicationDbContext();
            db.Configuration.LazyLoadingEnabled = false;

        }

        // GET: Customer
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

            var customers = from s in db.Customers
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(s => s.CustomerPhone.Contains(searchString));
            }
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            //var customers = db.Customers.Include(c => c.Region);

            return View(customers.OrderBy(p => p.RegesterDate).Include(p => p.Region).ToPagedList(pageNumber, pageSize));

            //return View(customers.ToList());
        }

        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer  customer = db.Customers.Include(c=>c.Region).SingleOrDefault(c=>c.CustomerId==id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            var model = new CustomerDetailViewModel
            {
                Customer=customer,
                warranties=db.Warranties.Include(w=>w.Product).Include(w=>w.Product.ProductModel).Include(w=>w.Product.Agent).Where(w=>w.CustomerId==customer.CustomerId)
            };
            return View(model);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            ViewBag.RegionId = new SelectList(db.Regions, "RegionId", "RegionName");
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerId,FirstName,LastName,CustomerPhone,Country,RegionId,Subcity,Woreda,Kebele,HouseNumber")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                Customer cust = customer;
                customer.RegesterDate = DateTime.Now;
                customer.IsSelected = false;
                db.Customers.Add(cust);
                db.SaveChanges();
                return RedirectToAction("CustomerProduct", new { Controller = "Customer", action = "CustomerProduct", id = customer.CustomerId });
            }

            ViewBag.RegionId = new SelectList(db.Regions, "RegionId", "RegionName", customer.RegionId);
            return View(customer);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegionId = new SelectList(db.Regions, "RegionId", "RegionName", customer.RegionId);
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerId,FirstName,LastName,CustomerPhone,Country,RegionId,Subcity,Woreda,Kebele,HouseNumber")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RegionId = new SelectList(db.Regions, "RegionId", "RegionName", customer.RegionId);
            return View(customer);
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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

        // To bind Customer with Product list
        public ActionResult CustomerProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            var user = User.Identity.GetUserId();
            var model = new CustomerProductViewModel
            {
                Customer=customer,
                Products=db.Products.Include(p=>p.ProductModel).Include(p=>p.Agent).Where(p=>p.AgentId== user).Where(p=>p.Sold == false).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfermWarranty (CustomerProductViewModel model)
        {

            if (model.Products.Where(p => p.IsSelected == true).Count() > 0)
            {
                if (model.Customer.CustomerId > 0)
                {
                    // To Request Microfinance
                    /*
                    if (model.Customer.IsSelected == true)
                    {
                        List<ProductViewModel> viewModels = new List<ProductViewModel>();

                        foreach (var product in model.Products.Where(p=>p.IsSelected==true))
                        {
                            var proModel = db.Products.Include(p => p.ProductModel).SingleOrDefault(p => p.ProductId == product.ProductId);
                            if (proModel != null)
                            {
                                viewModels.Add(new ProductViewModel
                                {
                                    ProductModelId = proModel.ProductModelId,
                                    Serial = proModel.Serial,
                                    ManufacturedDate = proModel.ManufacturedDate,
                                    BrandCompanyName = proModel.ProductModel.ModelName,
                                    Price = proModel.ProductModel.Price
                                });
                            }
                        }
                        var VM = new CreaditRequestViewModel
                        {
                            Customer = db.Customers.SingleOrDefault(c => c.CustomerId == model.Customer.CustomerId),
                            ProductViewModels=viewModels,
                            Micro=null
                        };
                        TempData["CreaditRequest"] = VM;
                        return RedirectToAction("Create", "CreaditRequest", VM);

                    }
                    */
                    var viewMode = new CustomerProductViewModel
                    {
                        Customer = db.Customers.SingleOrDefault(c => c.CustomerId == model.Customer.CustomerId),
                        Products = model.Products.Where(p => p.IsSelected == true).ToList()
                    };
                   
                    return View(viewMode);
                }
               
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult CustomerProduct(CustomerProductViewModel model)
        {
            if (model.Products.Count() > 0)
            {
                var customer = db.Customers.SingleOrDefault(c => c.CustomerId == model.Customer.CustomerId);
                var sendTo = customer.CustomerPhone;
                var message = "Dear " + model.Customer.FullName + ", your Customer ID: BCC" + string.Format("{0:0000000}", model.Customer.CustomerId) + ", And warranty Verification code for the Product ";

                foreach (var p in model.Products)
                {
                    Product CurrentProduct = db.Products.SingleOrDefault(cp => cp.ProductId == p.ProductId);
                    if (CurrentProduct != null)
                    {
                        CurrentProduct.Sold = true;
                        db.Warranties.Add(new Warranty
                        {
                            CustomerId = model.Customer.CustomerId,
                            ProductId = p.ProductId,
                            WarrantyCode = "RE" + string.Format("{0:0000000}", (model.Customer.CustomerId.ToString() + p.ProductId.ToString())),
                            SoledDate = DateTime.Now,
                            SMSID = "SMSID_Sample"
                        });
                        message = message + ":(Serial No: " + p.Serial + ", Warranty code: " + "BC" + string.Format("{0:0000000}", (model.Customer.CustomerId + p.ProductId) + ")");
                       // db.SaveChanges();
                    }


                }
                message = message + " Thank you for using our product. Buy Choice.";
               // SendSms(message);

                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [NonAction]
        public ActionResult SendSms(string text)
        {
            try
            {
                var expDate = DateTime.Now.AddYears(2);
                var accountSid = Models.TwilioAuth.TwilioAccountSid;
                var authToken = Models.TwilioAuth.TwilioAuthToken;
                TwilioClient.Init(accountSid, authToken);
                var to = new PhoneNumber("+251901213198");
                //var to = new PhoneNumber(sendTo);
                var sms = text;
                var from = new PhoneNumber(Models.TwilioAuth.SMSAccountFrom);
                var message = MessageResource.Create(to: to, from: from, body: sms);
                Console.WriteLine(message.Sid);
                return Content(message.Sid);
            }
            catch (DbEntityValidationException e)
            {
                ViewBag.AdminError = e;
                return View("AdminError");
            }

        }
    }
}

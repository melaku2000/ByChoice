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

namespace ByChoice.Controllers
{
    public class ProductModelController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProductModel
        public ActionResult Index()
        {
            var productModels = db.ProductModels.Include(p => p.ProductCatagory);
            return View(productModels.ToList());
        }

        // GET: ProductModel/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModel productModel = db.ProductModels.Include(p=>p.ProductMatch).Include(p=>p.ProductCatagory).SingleOrDefault(p => p.ProductModelId == id);
            if (productModel == null)
            {
                return HttpNotFound();
            }
            return View(productModel);
        }

        // GET: ProductModel/Create
        public ActionResult Create()
        {
            ViewBag.ProductCatagoryId = new SelectList(db.ProductCatagories, "ProductCatagoryId", "ProductCatagoryName");
            return View();
        }

        // POST: ProductModel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductModelId,ProductCatagoryId,ProductName,BrandCompanyName,Model,WarrantyPeriod,LifeTime,Description,Image,Browsher,Certificate,IsSelected,Quantity,Price")] ProductModel productModel)
        //public ActionResult Create( ProductModel productModel)
        {
            HttpPostedFileBase file = Request.Files["ImageData"];
            HttpPostedFileBase Browsher = Request.Files["BrowsherData"];
            HttpPostedFileBase Certificate = Request.Files["CertificateData"];

            byte[] img, certeficate, browsher=null;
            if (file != null)
            {
                img = ConvertToBytes(file);
            }
            else
            {
               img = null;
            }
            if (Browsher != null)
            {
                browsher = ConvertToBytes(Browsher);
            }
            else
            {
                browsher = null;
            }
            if (Certificate != null)
            {
                certeficate = ConvertToBytes(Certificate);
            }
            else
            {
                certeficate= null;
            }
            
            var model = new ProductModel
            {
                ProductCatagoryId= productModel.ProductCatagoryId,
                ProductName = productModel.ProductName,
                BrandCompanyName = productModel.BrandCompanyName,
                Model = productModel.Model,
                WarrantyPeriod = productModel.WarrantyPeriod,
                LifeTime = productModel.LifeTime,
                Description = productModel.Description,
                Image=img,
                Browsher=browsher,
                Certificate=certeficate,
                IsSelected=false,
                Quantity=productModel.Quantity,
                Price=productModel.Price
            };
            //if (ModelState.IsValid)
            //{
            //    db.ProductModels.Add(productModel);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            try
            {
                db.ProductModels.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.ProductCatagoryId = new SelectList(db.ProductCatagories, "ProductCatagoryId", "ProductCatagoryName", productModel.ProductCatagoryId);
                return View(productModel);
                throw;
            }
        }
        // To Convert HttpHosted files
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
        public ActionResult RetrieveImage(int id)
        {
            var q = from temp in db.ProductModels where temp.ProductModelId == id select temp.Image;
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
        //Retrive Certificate for View
        public ActionResult RetrieveCerteficate(int id)
        {
            var q = from temp in db.ProductModels where temp.ProductModelId == id select temp.Certificate;
            byte[] cover = q.First();
            if (cover != null)
            {
                return File(cover, "application/pdf", "Certeficate");
            }
            else
            {
                return Content("We appologize the file you request is not found");
            }
        }
        //Retrive Browsher for View
        public ActionResult RetrieveBrowsher(int id)
        {
            var q = from temp in db.ProductModels where temp.ProductModelId == id select temp.Browsher;
            byte[] cover = q.First();
            if (cover != null)
            {
                return File(cover, "application/pdf", "Browsher");
            }
            else
            {
                return Content("We appologize the file you request is not found");
            }
        }
        // GET: ProductModel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModel productModel = db.ProductModels.Find(id);
            if (productModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductCatagoryId = new SelectList(db.ProductCatagories, "ProductCatagoryId", "ProductCatagoryName", productModel.ProductCatagoryId);
            return View(productModel);
        }

        // POST: ProductModel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductModelId,ProductCatagoryId,ProductName,BrandCompanyName,Model,WarrantyPeriod,LifeTime,Description,Image,Browsher,Certificate,IsSelected,Quantity,Price")] ProductModel productModel)
        {
            HttpPostedFileBase file = Request.Files["ImageData"];
            HttpPostedFileBase Browsher = Request.Files["BrowsherData"];
            HttpPostedFileBase Certificate = Request.Files["CertificateData"];

           

            if (ModelState.IsValid)
            {
                ProductModel model = db.ProductModels.SingleOrDefault(p => p.ProductModelId == productModel.ProductModelId);
                if (model != null)
                {
                    if (file != null)
                    {
                        model.Image = ConvertToBytes(file);
                    }
                    //else
                    //{
                    //    img = null;
                    //}
                    if (Browsher != null)
                    {
                        model.Browsher = ConvertToBytes(Browsher);
                    }
                    //else
                    //{
                    //    browsher = null;
                    //}
                    if (Certificate != null)
                    {
                        model.Certificate = ConvertToBytes(Certificate);
                    }
                    //else
                    //{
                    //    certeficate = null;
                    //}
                    //db.Entry(productModel).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
               
                
            }
            ViewBag.ProductCatagoryId = new SelectList(db.ProductCatagories, "ProductCatagoryId", "ProductCatagoryName", productModel.ProductCatagoryId);
            return View(productModel);
        }

        // GET: ProductModel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModel productModel = db.ProductModels.Find(id);
            if (productModel == null)
            {
                return HttpNotFound();
            }
            return View(productModel);
        }

        // POST: ProductModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductModel productModel = db.ProductModels.Find(id);
            db.ProductModels.Remove(productModel);
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

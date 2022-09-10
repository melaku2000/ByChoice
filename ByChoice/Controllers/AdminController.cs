using ByChoice.Models;
using ByChoice.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ByChoice.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        public AdminController()
        {
            _context = new ApplicationDbContext();
            _context.Configuration.LazyLoadingEnabled = false;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Admin
        public ActionResult Index()
        {
            var model = new AdminViewModel
            {
                Order = _context.Orders.Where(o => o.New != false).Count(),
                Claim = _context.Claims.Where(o => o.New != false).Count(),
                AgentTotal = _context.Agents.Count(),
                CustomerTotal = _context.Customers.Count(),
                TotalProduct = _context.Products.Count(),
                TotalSales = _context.Products.Count(p => p.Sold == true),
                Event = _context.Events.Count(e => e.EventDate >= DateTime.Now)
            };
            return View(model);
        }
        // Dashboard Methods
        public JsonResult GetData()
        {

            ChartData _chart = new ChartData();
            var cust = _context.Products.Include(p => p.Agent).GroupBy(p => p.Agent).Select(g => new { name = g.Key.AgentName, qty = g.Count(c => c.Sold == true) }).ToArray();

            _chart.labels = cust.Select(c => c.name).ToArray();
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();

            _dataSet.Add(new Datasets()
            {
                label = "Total Activity",
                data = cust.Select(c => c.qty).ToArray(),
                borderColor = new string[] { "rgba(75,192,192,1)" },
                backgroundColor = new string[] { "rgba(75,192,192,0.4)" },
                borderWidth = "1"
            });
            _dataSet.Add(new Datasets()
            {
                label = "Last weeks",
                data = new int[] { 10, 12, 9 },
                borderColor = new string[] { "rgba(75,192,192,1)" },
                backgroundColor = new string[] { "rgba(75,192,192,0.4)" },
                borderWidth = "1"
            });

            _chart.datasets = _dataSet;
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDataProduct()
        {

            ChartData _chart = new ChartData();
            var cust = _context.Products.Include(p=>p.ProductModel).GroupBy(p => p.ProductModel).Select(g => new { name = g.Key.Model, qty = g.Count(c => c.Sold == true) }).ToArray();

            _chart.labels = cust.Select(c => c.name).ToArray();
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();

            _dataSet.Add(new Datasets()
            {
                label = "Total Product sold",
                data = cust.Select(c => c.qty).ToArray(),
                borderColor = new string[] { "rgba(75,192,192,1)" },
                backgroundColor = new string[] { "rgba(75,192,192,0.4)" },
                borderWidth = "1"
            });
            _dataSet.Add(new Datasets()
            {
                label = "Last weeks",
                data = new int[] { 10, 12, 9, 5, 7, 9, 4, 0, 13, 8, 3, 7, 4, 11, 10 },
                borderColor = new string[] { "rgba(75,192,192,1)" },
                backgroundColor = new string[] { "rgba(75,192,192,0.4)" },
                borderWidth = "1"
            });

            _chart.datasets = _dataSet;
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMapData()
        {
            var agent = _context.Agents.ToArray();
            List<MapData> data = new List<MapData>();
            foreach (var a in agent)
            {
                var model = new MapData
                {
                    Id = a.Id,
                    PlaceName = a.AgentName,
                    GeoLong = a.Longitude,
                    GeoLat = a.Latitude
                };
                data.Add(model);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
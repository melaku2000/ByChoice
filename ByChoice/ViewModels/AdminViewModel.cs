using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ByChoice.ViewModels
{
    public class AdminViewModel
    {
        public int Order { get; set; }
        public int Claim { get; set; }
        public int AgentTotal { get; set; }
        public int CustomerTotal { get; set; }
        public int TotalProduct { get; set; }
        public int TotalSales { get; set; }
        public int Event { get; set; }
        public int Notice { get; set; }
    }

    public class ChartData 
    {
        public string[] labels { get; set; }
        public List<Datasets> datasets { get; set; }
    }
    public class Datasets
    {
        public string label { get; set; }
        public string[] backgroundColor { get; set; }
        public string[] borderColor { get; set; }
        public string borderWidth { get; set; }
        public int[] data { get; set; }
    }

    public class MapData
    {
        public string Id { get; set; }
        public string PlaceName { get; set; }
        public decimal? GeoLong { get; set; }
        public decimal? GeoLat { get; set; }
    }
}
using ByChoice.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ByChoice.DTOs
{
    public class NoticeDTOs
    {
        public string Id { get; set; }

        public int NoticeId { get; set; }

        public string AgentName { get; set; }
        public string AgentCatagoryType { get; set; }
        public string FullName { get; set; }
        public string RegionName { get; set; }
        public string TaxNumber { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsSelected { get; set; }
    }

    public class NoticeDTOsViewModel
    {
        public Notice Notice { get; set; }
        public List<NoticeDTOs> NoticeDTOs { get; set; }
    }
}
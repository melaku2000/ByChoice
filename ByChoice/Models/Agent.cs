using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ByChoice.Models
{
    public class Agent
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Agent Name")]
        public string AgentName { get; set; }

        public int AgentCatagoryId { get; set; }

        public byte[] Photo { get; set; }

        [Required(ErrorMessage = "Please enter city latitude")]

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:00.000000}", ApplyFormatInEditMode = true)]
        public decimal? Latitude { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:00.000000}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please enter city longitude ")]
        public decimal? Longitude { get; set; }

        // Navigations
        public ApplicationUser ApplicationUser { get; set; }
        public AgentCatagory AgentCatagory { get; set; }
        public ICollection<Product> Products { get; set; }
        //public ICollection<CreaditRequest> CreaditRequests { get; set; }
        public ICollection<WarrantyTransfer>  WarrantyTransfers { get; set; }
        public ICollection<AgentEvent>  AgentEvents { get; set; }
        public ICollection<AgentNotice> AgentNotices { get; set; }
        public ICollection<Order> Orders { get; set; }
        //public ICollection<Warranty> Warranties { get; set; }
    }
    public class AgentCatagory
    {
        [Key]
        [Display(Name = "Agent")]
        public int AgentCatagoryId { get; set; }

        [Display(Name = "Agent Type")]
        public string AgentCatagoryType { get; set; }

        

        // Navigation
        public ICollection<Agent> Agents { get; set; }

    }

    public class Micro
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Micro-Finance Name")]
        public string MicroName { get; set; }

        [Required, Range(1, 20),Display(Name ="Interset Rate")]
        public byte IntrestRate { get; set; }

        public byte[] Photo { get; set; }

        [Required(ErrorMessage = "Please enter city latitude")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:00.000000}", ApplyFormatInEditMode = true)]
        public decimal? Latitude { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:00.000000}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please enter city longitude ")]
        public decimal? Longitude { get; set; }

        // Navigations
        public ApplicationUser ApplicationUser { get; set; }
        //public ICollection<CreaditRequest> CreaditRequests { get; set; }

    }

}
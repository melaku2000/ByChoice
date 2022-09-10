using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ByChoice.Models
{
    public class Region
    {
        [Key]
        public int RegionId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Region")]
        public string RegionName { get; set; }

        //public ICollection<ApplicationUser> ApplicationUser { get; set; }
        public ICollection<Customer> Customers {get; set; }
    }
}
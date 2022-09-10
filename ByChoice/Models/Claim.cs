using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ByChoice.Models
{
    public class Claim
    {
        [Key,Display(Name ="Claim Id")]
        public int ClaimId { get; set; }

        [Required,Display(Name ="Customer Warranty")]
        public int WarrantyId { get; set; }

        [Display(Name ="Claim Date")]
        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ClaimDate { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        // Replaies
        public bool? Repaired { get; set; }
        public bool? Extended { get; set; }
        public bool? Collected { get; set; }

        public bool? New { get; set; }
        public bool? IsSelected { get; set; }

        // Navigations
        public Warranty Warranty { get; set; }
    }

    public class ClaimList
    {
        [Key, Display(Name = "Customer Claim Id")]
        public int ClaimListId { get; set; }

        [Display(Name = "Warranty Claim")]
        public int ClaimId { get; set; }

        [Display(Name = "Claim Type")]
        public int ClaimTypeId { get; set; }

        // Navigations
        public ClaimType ClaimType { get; set; }
        public Claim Claim { get; set; }
    }

    public class ClaimType
    {
        [Key, Display(Name = "Claim Type")]
        public int ClaimTypeId { get; set; }

        [Display(Name = "Claim Type")]
        public string ClaimTypeName { get; set; }

        [Display(Name = "IsSelected")]
        public bool? IsSelected { get; set; }

        // Navigations
        public ICollection<ClaimList> ClaimLists { get; set; }
    }
}
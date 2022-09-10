using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ByChoice.Models
{
    public class Info
    {
        [Key]
        public int InfoId { get; set; }
        public string InfoType { get; set; }
    }
    public class CallService
    {
        [Key]
        public int CallServiceId { get; set; }

        [Required,Display(Name ="Officer")]
        public string Id { get; set; }

        [Required,Display(Name ="Caller Name")]
        public string CallerName { get; set; }

        [Required,Display(Name ="Caller Phone"),MinLength(9),MaxLength(9)]
        public string CallerPhone  { get; set; }

        [Required,Display(Name ="Region")]
        public int RegionId { get; set; }
        public Region Region  { get; set; }

        [Required,Display(Name ="Info Type")]
        public string InfoType { get; set; }

        [Required,Display(Name ="Caller Intrested")]
        public string IntrestedIn { get; set; }

        [Display(Name ="Other"),DataType(DataType.MultilineText)]
        public string Other { get; set; }

        [Display(Name ="Date"),Required]
        public DateTime CallingDate { get; set; }

        // Navigations
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<Info> Infos { get; set; }
        public ICollection<Micro> Micros { get; set; }
        public ICollection<Agent> Agents { get; set; }
    }
    public class CallProductReg
    {
        [Key]
        public int CallProductRegId { get; set; }

        [Required, Display(Name = "Officer")]
        public string Id { get; set; }

        [Required, Display(Name = "Caller Name")]
        public string CallerName { get; set; }

        [Required, Display(Name = "Caller Phone"), MinLength(9), MaxLength(9)]
        public string CallerPhone { get; set; }

        [Required, Display(Name = "Region")]
        public int RegionId { get; set; }
        public Region Region { get; set; }

        [Display(Name = "Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Display(Name = "Date"), Required]
        public DateTime CallingDate { get; set; }
        
        // Navigations
        public ApplicationUser ApplicationUser { get; set; }
    }

    public class CallClaim
    {
        [Key]
        public int CallClaimId { get; set; }

        [Required, Display(Name = "Officer")]
        public string Id { get; set; }

        // Navigations
        public ApplicationUser ApplicationUser { get; set; }
    }
}

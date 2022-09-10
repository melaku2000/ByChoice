using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByChoice.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required,Phone]
        [Display(Name = "Mobile Number")]
        public string CustomerPhone { get; set; }

        [StringLength(50), Required]
        public string Country { get; set; }

        [Required,Display(Name = "Region")]
        public int RegionId { get; set; }

        [StringLength(50)]
        [Display(Name = "Subcity/Unique Zone")]
        public String Subcity { get; set; }

        [StringLength(50)]
        public String Woreda { get; set; }

        [StringLength(50)]
        public String Kebele { get; set; }

        [StringLength(50)]
        [Display(Name = "House Number")]
        public String HouseNumber { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RegesterDate { get; set; }

        public bool IsSelected { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        // Navigations
        public Region Region { get; set; }
        public ICollection<Warranty> Warranties { get; set; }
        //public ICollection<CreaditRequest> CreaditRequests { get; set; }
    }

    public class Warranty
    {
        [Key]
        public int WarrantyId { get; set; }

        [Required,Display(Name = "Warranty Code")]
        public string WarrantyCode { get; set; }

        [Required,Display(Name = "Customer ID")]
        public int CustomerId { get; set; }

        [Required,Display(Name = "Product ID")]
        public int ProductId { get; set; }

        //[Required, Display(Name = "Agent")]
        //public string Id { get; set; }

        [Required,Display(Name = "Sold Date")]
        public DateTime SoledDate { get; set; }

        [Required, Display(Name = "SMS ID")]
        public string SMSID { get; set; }

        // Navigations
        public Customer Customer { get; set; }
        public Product Product { get; set; }
       // public Agent Agent { get; set; }
        public ICollection<Claim> Claims { get; set; }
    }

    public class CustomerProductViewModel
    {
        public Customer Customer { get; set; }
        public List<Product> Products { get; set; }
    }

    public class CustomerDetailViewModel
    {
        public Customer Customer { get; set; }
        public IEnumerable<Warranty> warranties { get; set; }
    }
}
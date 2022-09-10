using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ByChoice.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(10,MinimumLength =10)]
        [Display(Name = "TIN")]
        public string TaxNumber { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName{ get; set; }

        [StringLength(50), Required]
        public string Country { get; set; }

        [Display(Name = "Region"), Required]
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public String HouseNumber { get; set; }

        [Display(Name ="Registered Date")]
        public DateTime? RegesterDate { get; set; }
        //Added 14 Aug 18
        public bool IsSelected { get; set; }

        [Display(Name = "Contact Person")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        // Navigations
        public Agent Agent { get; set; }
        public Micro Micro { get; set; }
        public Region Region { get; set; }
       // public ICollection<Product> Products { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Micro> Micros { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<AgentCatagory> AgentCatagories { get; set; }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<AgentEvent> AgentEvents { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderList> OrderLists { get; set; }

        public DbSet<Notice> Notices { get; set; }
        public DbSet<AgentNotice> AgentNotices { get; set; }
        public DbSet<NoticeType> NoticeTypes { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Warranty> Warranties { get; set; }
        public DbSet<Product> Products  { get; set; } 
        public DbSet<ProductHistory> ProductHistories  { get; set; }
        public DbSet<ProductHistoryList> ProductHistoryLists  { get; set; }
        public DbSet<ProductModel> ProductModels  { get; set; }
        public DbSet<ProductCatagory> ProductCatagories  { get; set; }

        public DbSet<ProductMatchPurpose> ProductMatchPurposes  { get; set; }
        public DbSet<ProductMatch> ProductMatches { get; set; }

       
        public DbSet<Claim> Claims  { get; set; }
        public DbSet<ClaimList> ClaimLists  { get; set; }
        public DbSet<ClaimType> ClaimTypes  { get; set; }

        
         public DbSet<WarrantyTransfer> WarrantyTransfers  { get; set; }
        /*
        public DbSet<CreaditRequest> CreaditRequests  { get; set; }
        public DbSet<Creadit> Creadits  { get; set; }
        public DbSet<CreaditRequestList>  CreaditRequestLists { get; set; }
        public DbSet<CreaditBill> CreaditBills  { get; set; }
        */


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

     //   public System.Data.Entity.DbSet<ByChoice.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}
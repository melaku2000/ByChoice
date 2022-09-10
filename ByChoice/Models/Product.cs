using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByChoice.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required, Display(Name = "Agent")]
        public string AgentId { get; set; }

        [Required,Display(Name ="Model")]
        public int ProductModelId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Serial Number")]
        public string Serial { get; set; }

        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ManufacturedDate { get; set; }

        [Display(Name = "Sold")]
        public bool Sold { get; set; }

        public bool IsSelected { get; set; }

        // Navigations
        //public ApplicationUser ApplicationUser { get; set; }
        public Agent Agent { get; set; }
        public ProductModel ProductModel { get; set; }
        public ICollection<Warranty> Warranties { get; set; }
        //public Warranty Warranty { get; set; }
        public ICollection<ProductHistoryList> ProductHistoryLists { get; set; }
    }

    public class ProductCatagory
    {
        [Key]
        public int ProductCatagoryId { get; set; }
        [StringLength(50)]
        [Display(Name = "Catagory")]
        public string ProductCatagoryName { get; set; }

        // Navigations
        public ICollection<ProductModel> ProductModels{ get; set; }
    }

    public class ProductModel 
    {
        [Key]
        public int ProductModelId { get; set; }

        [Display(Name = "Product Catagory")]
        public int ProductCatagoryId { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Brand / Company Name")]
        public string BrandCompanyName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Warranty Terms")]
        public int WarrantyPeriod { get; set; }

        [Required]
        [Display(Name = "Life Time")]
        public int LifeTime { get; set; }

        [AllowHtml]
        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        //[Required]
        public byte[] Image { get; set; }

        //New Added
        public byte[] Browsher { get; set; }

        public byte[] Certificate { get; set; }

        [Display(Name = "Select")]
        public bool IsSelected { get; set; }

        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        // Calculated Property
        [Display(Name ="Model")]
        public string ModelName
        {
            get { return BrandCompanyName + ", " + Model; }
        }
        // Navigations
        public ProductCatagory  ProductCatagory { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<CreaditRequestList> CreaditRequestLists { get; set; }

        public ProductMatch ProductMatch { get; set; }
    }

    public class ProductMatch
    {
        [Key]
        [ForeignKey("ProductModel")]
        public int ProductModelId { get; set; }

        [Required,Display(Name ="No of Light")]
        public int NoLight { get; set; }

        public bool Radio { get; set; }

        public bool Charger { get; set; }

        [Display(Name ="Flash Light")]
        public bool FlashLight { get; set; }

        [Display(Name ="Television Size")]
        public int? TvSize { get; set; }

        [Display(Name = "Study Light")]
        public bool StudyLight { get; set; }

        [Display(Name = "Home Use")]
        public bool HomeUse { get; set; }

        [Display(Name = "Traveling Light")]
        public bool TravelingLight { get; set; }
        // Navigation
        public ProductModel ProductModel { get; set; }
        public ProductMatchPurpose ProductMatchPurpose { get; set; }
    }
    public class ProductMatchPurpose
    {
        [Key]
        public int ProductMatchPurposeId { get; set; }

        public string Purpose { get; set; }

        // Navigation
        public ICollection<ProductModel> ProductModels { get; set; }
    }
    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<ProductHistoryList> ProductHistories { get; set; }
    }

    public class BulkProductInsertViewModel
    {

        [Required, Display(Name = "Agent")]
        public string AgentId { get; set; }

        [Required, Display(Name = "Model")]
        public int ProductModelId { get; set; }

        [Display(Name ="Pre-fix Character")]
        public string PreFix { get; set; }

        [Required, Display(Name ="Initial No")]
        public int InitialNo { get; set; }

        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ManufacturedDate { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
    }
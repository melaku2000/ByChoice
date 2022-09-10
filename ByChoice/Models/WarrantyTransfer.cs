using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ByChoice.Models
{
    public class WarrantyTransfer 
    {
        [Key]
        public int WarrantyTransferId { get; set; }

        [Required]
        [Display(Name = "Transfer To")]
        public string ToAgentId { get; set; }  // Query Transfer Agent

        [Required]
        [Display(Name = "Transfer From")]
        public string FromAgentId { get; set; }             //Who transfer Warranti

        [Required]
        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; }
        [Required]
        [Display(Name = "Transaction Code")]
        public string TransferCode { get; set; }

        // Navigations
        public Agent ToAgent { get; set; }
        public Agent FromAgent { get; set; }

    }

   
    public class ProductHistory
    {
        [Key]
        public int ProductHistoryId { get; set; }

        [Required,Display(Name ="Transfer Warranty")]
        public int WarrantyTransferId { get; set; }

        [Required]
        [Display(Name = "Transferd Time")]
        public DateTime TransferdTime { get; set; }

        // Navigations
        public WarrantyTransfer WarrantyTransfer { get; set; }
        public ICollection<ProductHistoryList> ProductHistoryLists { get; set; }
    }

    public class ProductHistoryList
    {
        [Key]
        public int ProductHistoryListId { get; set; }

        [Display(Name = "Product History")]
        public int ProductHistoryId { get; set; }

        [Required]
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        // Navigations
        public Product Product { get; set; }
        public ProductHistory ProductHistory { get; set; }
    }

    public class WarrantyTransferViewModel
    {
        public WarrantyTransfer WarrantyTransfer { get; set; }
        public List<Product> Products { get; set; }
    }

    public class ConfermTransferViewModel
    {
        public WarrantyTransfer WarrantyTransfer { get; set; }
        public Agent ToAgent { get; set; }
        public List<Product> Products { get; set; }
    }

}
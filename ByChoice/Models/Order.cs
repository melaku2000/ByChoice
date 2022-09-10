using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ByChoice.Models
{
    public class Order
    {
        [Key,Display(Name ="Order Id")]
        public int OrderId { get; set; }

        [Required, Display(Name = "Agent")]
        public string Id { get; set; }

        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }
        
        [Display(Name ="Order Amount"),Required]
        public decimal OrderAmount { get; set; }

        [DataType(DataType.MultilineText)]
        public string Detail { get; set; }

        public bool? New { get; set; }

        [Display(Name = "Select")]
        public bool IsSelected { get; set; }

        // Navigations
        public Agent Agent { get; set; }
        public ICollection<OrderList> OrderLists { get; set; }

    }

    public class OrderList
    {
        [Key, Display(Name = "Order List")]
        public int OrderListId { get; set; }

        [Required, Display(Name = "Order")]
        public int OrderId { get; set; }

        [Required, Display(Name = "Product")]
        public int ProductModelId { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Calculated Property
        //public decimal TotalValue => ProductModel.Price * Quantity;
        // Navigations
        public Order Order { get; set; }
        public ProductModel ProductModel { get; set; }
    }
}
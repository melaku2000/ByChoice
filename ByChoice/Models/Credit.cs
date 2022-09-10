using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByChoice.Models
{
    public class CreaditRequest
    {
        [Key, Display(Name = "Credit Request Id")]
        public int CreaditRequestId { get; set; }

        [Required, Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Required, Display(Name = "Agent")]
        public string AgentId { get; set; }

        [Required, Display(Name = "Micro-finance")]
        public string MicroId { get; set; }

        [Required, Display(Name = "Request Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RequestDate { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [Required, Display(Name = "Requested Amount")]
        public decimal RequestAmount { get; set; }

        public bool? Decline { get; set; }
        public bool? Approved { get; set; }

        [Display(Name = "Date")]
        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ApprovedDeclineDate { get; set; }

        public bool? CreaditConfirmed { get; set; }
        public bool? Delivered { get; set; }

        public bool? New { get; set; }
        public bool IsSelected { get; set; }

        // Navigations
        public Customer Customer { get; set; }
        public Agent Agent { get; set; }
        public Micro Micro { get; set; }
        public ICollection<CreaditRequestList> CreaditRequestLists { get; set; }
        public ICollection<Creadit> Creadits { get; set; }
    }

    public class CreaditRequestList
    {
        [Key]
        public int CreditRequestListId { get; set; }

        [Display(Name = "Creadit Request")]
        public int CreaditRequestId { get; set; }

        [Required, Display(Name = "Product")]
        public int ProductModelId { get; set; }

        public int? Quantity { get; set; }

        [Display(Name = "Approved Quantity")]
        public int? ApprovedQuantity { get; set; }

        // Navigations
        public ProductModel ProductModel { get; set; }
        public CreaditRequest CreaditRequest { get; set; }
    }

    public class Creadit
    {
        [Key,Display(Name ="Credit Id")]
        public int CreaditId { get; set; }

        [Required,Display(Name ="Credit Request")]
        public int CreaditRequestId { get; set; }

        [Required, Display(Name = "Approved Amount")]
        public decimal ApprovedAmount { get; set; }

        [Required, Display(Name = "Term")] //for loan Term Years
        [Range(0, 2)]
        public byte? TermYear { get; set; }

        [Required, Display(Name = "Months")] //for loan Term Months
        [Range(0, 11)]
        public byte? TermMonth { get; set; }

        [Required, Display(Name = "Payment Period")] // Weekly, Monthly, Quarterly
        public string PaymentPeriod { get; set; }

        [Required, Display(Name = "Payment Period Amount")] // Period Payment
        public string PaymentPeriodAmount { get; set; }

        [Column(TypeName = "money"), DataType(DataType.Currency)]
        [Required, Display(Name = "Total Interest")]
        public decimal Intrest { get; set; }

        [Column(TypeName = "money"), DataType(DataType.Currency)]
        [Display(Name = "Total Loan Amount")]
        public decimal TotalLoanAmount {
            get { return ApprovedAmount + Intrest; }
        }

        [Column(TypeName = "money"), DataType(DataType.Currency)]
        [Display(Name = "Remaining Amount")]
        public decimal RemainingLoan
        {
            get { return TotalLoanAmount - LoanCovered; }
        }

        [Column(TypeName = "money"), DataType(DataType.Currency)]
        [Display(Name = "Loan Covred")]
        public decimal LoanCovered { get; set; }

        // Navigations
        public CreaditRequest CreaditRequest { get; set; }
        public ICollection<CreaditBill> CreaditBills { get; set; }
    }

    public class CreaditBill
    {
        [Key]
        public int CreaditBillId { get; set; }

        [Required, Display(Name = "Credit Id")]
        public int CreaditId { get; set; }

        [Required, Display(Name = "Paid Amount")]
        [Column(TypeName = "money"), DataType(DataType.Currency)]
        public decimal PaidAmount { get; set; }

        [Display(Name = "Date")]
        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PaidDate { get; set; }

        // Navigations
        public Creadit Creadit { get; set; }
    }
    public class CreaditRequestViewModel
    {
        public Customer Customer { get; set; }
        //public IEnumerable<Micro> Micros { get; set; }
        public Micro Micro { get; set; }
        public List<ProductViewModel> ProductViewModels { get; set; }
    }

    public class ProductViewModel
    {
        [ Display(Name = "Model")]
        public int ProductModelId { get; set; }

        [StringLength(50)]
        [Display(Name = "Serial Number")]
        public string Serial { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ManufacturedDate { get; set; }
        [StringLength(50)]

        [Display(Name = "Brand / Company Name")]
        public string BrandCompanyName { get; set; }

        [DataType(DataType.Currency)]
        public decimal? Price { get; set; }
    }
}
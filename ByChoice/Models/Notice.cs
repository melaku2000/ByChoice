using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ByChoice.Models
{
    public class Notice
    {
        [Key,Display(Name ="Notice")]
        public int NoticeId { get; set; }

        public int NoticeTypeId { get; set; }

        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }

        [Display(Name ="Detail"),Required]
        [DataType(DataType.MultilineText)]
        public string Detail { get; set; }

        [Display(Name = "Select")]
        public bool IsSelected { get; set; }

        // Navigations
        public NoticeType NoticeType { get; set; }
        public ICollection<AgentNotice> AgentNotices { get; set; }
    }

    public class NoticeType
    {
        [Key, Display(Name = "Detail Id")]
        public int NoticeTypeId { get; set; }

        [Display(Name = "Notice Type")]
        public string NoticeTypeName { get; set; }

        [Display(Name = "Select")]
        public bool IsSelected { get; set; }

        // Navigations
        public ICollection<Notice> Notices { get; set; }
    }

    public class AgentNotice
    {
        [Key, Display(Name = "Agent Notice Id")]
        public int AgentNoitceId { get; set; }

        [Display(Name = "Notice")]
        public int NoticeId { get; set; }

        [Display(Name = "Agent")]
        public string Id { get; set; }

        [Display(Name = "Select")]
        public bool IsSelected { get; set; }

        [Display(Name = "Select")]
        public bool New { get; set; }

        // Navigations
        public Agent Agent { get; set; }
        public Notice Notice { get; set; }
    }

    public class CreateNoticeViewModel
    {
        [Key, Display(Name = "Notice")]
        public int NoticeId { get; set; }

        public int NoticeTypeId { get; set; }

        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }

        [Display(Name = "Detail"), Required]
        [DataType(DataType.MultilineText)]
        public string Detail { get; set; }

        [Display(Name = "Select")]
        public bool IsSelected { get; set; }

        // Navigations
        public List<Agent> Agents { get; set; }
        public NoticeType NoticeType { get; set; }
        //public ICollection<AgentNotice> AgentNotices { get; set; }
    }

    public class DetailNoticeViewModel 
    {
        [Key, Display(Name = "Notice")]
        public int NoticeId { get; set; }

        public int NoticeTypeId { get; set; }

        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }

        [Display(Name = "Detail"), Required]
        [DataType(DataType.MultilineText)]
        public string Detail { get; set; }

        [Display(Name = "Select")]
        public bool IsSelected { get; set; }

        // Navigations
        public IEnumerable<Agent> Agents { get; set; }
        public NoticeType NoticeType { get; set; }
        //public ICollection<AgentNotice> AgentNotices { get; set; }
    }
}
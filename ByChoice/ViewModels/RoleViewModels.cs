using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ByChoice.Models;

namespace ByChoice.ViewModels
{
    public class RoleViewModels
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Roles { get; set; }
    }

    public class AssignedAgent
    {
        public string Id { get; set; }
        public string AgentName { get; set; }
        public string Phone { get; set; }
        public bool Assigned { get; set; }
    }

    public class CreateEventViewModel
    {
        [Key, Display(Name = "Event Id")]
        public int EventId { get; set; }

        [Required, Display(Name = "Event")]
        public int EventTypeId { get; set; }

        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }

        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EventDate { get; set; }

        [Required]
        public string Location { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Detail { get; set; }

        [Display(Name = "Select")]
        public bool IsSelected { get; set; }


        public List<Agent> Agents { get; set; }
        public EventType EventType { get; set; }
    }
    public class DetailEventViewModel
    {
        [Key, Display(Name = "Event Id")]
        public int EventId { get; set; }

        [Required, Display(Name = "Event")]
        public int EventTypeId { get; set; }

        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }

        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EventDate { get; set; }

        [Required]
        public string Location { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Detail { get; set; }

        [Display(Name = "Select")]
        public bool IsSelected { get; set; }


        public IEnumerable<Agent> Agents { get; set; }
        public EventType EventType { get; set; }
    }
}
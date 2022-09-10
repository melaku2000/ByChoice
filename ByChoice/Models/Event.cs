using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ByChoice.Models
{
    public class Event
    {
        [Key,Display(Name ="Event Id")]
        public int EventId { get; set; }

        [Required,Display(Name ="Event")]
        public int EventTypeId { get; set; }

        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }

        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EventDate { get; set; }

        [Required]  
        public string Location { get; set; }

        [Required,DataType(DataType.MultilineText)]
        public string Detail { get; set; }

        [Display(Name = "Select")]
        public bool IsSelected { get; set; }

        // Navigations
        public EventType EventType { get; set; }
        public ICollection<AgentEvent> AgentEvents { get; set; }
    }

    public class EventType
    {
        [Key, Display(Name = "Event Type")]
        public int EventTypeId { get; set; }

        [Display(Name = "Event Type Name")]
        public string EventName { get; set; }

        [Display(Name = "Select")]
        public bool IsSelected { get; set; }

        // Navigations
        public ICollection<Event> Events { get; set; }
    }

    public class AgentEvent
    {
        [Key, Display(Name = "Event Id")]
        public int AgentEventId { get; set; }

        [Display(Name = "Event")]
        public int EventId { get; set; }

        [ Display(Name = "Agent")]
        public string Id { get; set; }

        [Display(Name = "Select")]
        public bool IsSelected { get; set; }

        [Display(Name = "Select")]
        public bool New { get; set; }

        // Navigations
        public Agent Agent { get; set; }
        public Event Event { get; set; }
    }
}
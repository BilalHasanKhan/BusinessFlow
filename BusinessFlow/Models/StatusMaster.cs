using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BusinessFlow.Models
{
    public class StatusMaster
    {
        [Key]
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int  StatusTypeId { get; set; }
        [ForeignKey("StatusTypeId")]
        public virtual StatusType StatusType { get; set; }
        public virtual ICollection<Enquiry> Enquiry { get; set; }
        public virtual ICollection<Project> ProjectStatus { get; set; }
    }

    public enum Status
    {

      Await_Appoint=1,
      Site_Visited,
      Quote_1_Sent,
      Quote_2_Sent,
      Confirmed,
      Quote_3_Sent,
      Quote_4_Sent,
      Await_Discussion,
      Drawings_being_prepared,
      Drawings_sent_await_approval,
      Revised_drawings_being_prepared,
      Await_approval_on_revised_drawings,
      SignOff_meeting_to_be_fixed,
      SignOff_Fixed
    }
}
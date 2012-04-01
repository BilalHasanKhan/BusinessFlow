using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BusinessFlow.Models
{
    public class Project
    {
        [Key]
        public int ProjectID { get; set; }
        [ScaffoldColumn(false)]
        public int EnquiryID { get; set; }
        [DisplayName("Confirmation Date")]
        [ScaffoldColumn(false)]
        public DateTime ConfirmDate { get; set; }
        [UIHint("Status")]
        public virtual StatusMaster Status { get; set; }
        public virtual ICollection<TeamProject> Teams { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
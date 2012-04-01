using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BusinessFlow.Models
{
    public class Task
    {
        [Key]
        [ScaffoldColumn(false)]
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        [DisplayName("Task Description")]
        [UIHint("TextArea")]
        public string TaskDescription { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Task Start Date")]
        [UIHint("Date")]
        public DateTime TaskStartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Task End Date")]
        [UIHint("Date")]
        public DateTime TaskEndDate { get; set; }
        [ScaffoldColumn(false)][DefaultValue(false)]
        public bool IsAssigned { get; set; }
        [ScaffoldColumn(false)]
        public int ProjectID { get; set; }
        [ForeignKey("ProjectID")]
        public Project Project { get; set; }
        public ICollection<EmployeeTask> EmployeeTask { get; set; }
    }
}
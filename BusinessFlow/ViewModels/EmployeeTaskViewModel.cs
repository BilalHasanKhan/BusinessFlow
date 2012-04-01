using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BusinessFlow.Models;
using System.ComponentModel;

namespace BusinessFlow.ViewModels
{
    public class EmployeeTaskViewModel
    {
        [Key]
        public int TaskID { get; set; }
        public int ProjectID { get; set; }
        public string TaskName { get; set; }
        public string TaskDetails { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Task Start Date")]
        [UIHint("Date")]
        public DateTime TaskStartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Task End Date")]
        [UIHint("Date")]
        public DateTime TaskEndDate { get; set; }
        public int TotalHours { get; set; }
        public int HoursSpent { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BusinessFlow.Models
{
    public class EmployeeTask
    {
        [Key]
        public int EmployeeTaskID { get; set; }
        public int EmployeeID { get; set; }
        [ForeignKey("EmployeeID")]
        public Employee Employee { get; set; }
        public int TaskID { get; set; }
        public int Efforts { get; set; }
        [ForeignKey("TaskID")]
        public Task Task { get; set; }
    }
}
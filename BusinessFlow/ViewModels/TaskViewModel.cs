using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BusinessFlow.Models;
using System.ComponentModel;

namespace BusinessFlow.ViewModels
{
    public class TaskViewModel
    {
        public int TaskID { get; set; }

        public int ProjectID { get; set; }
        [DisplayName("Task Name")]
        public string TaskName { get; set; }

        [DisplayName("Task Description")]
        public string TaskDescription { get; set; }

        public int EmployeeID { get; set; }

        [DisplayName("Task Start Date")]
        public DateTime TaskStartDate { get; set; }

        [DisplayName("Task End Date")]
        public DateTime TaskEndDate { get; set; }

        public bool IsAssigned { get; set; }

    }
}
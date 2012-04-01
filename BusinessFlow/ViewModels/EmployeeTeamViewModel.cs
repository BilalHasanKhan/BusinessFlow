using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BusinessFlow.Models;
using System.ComponentModel;

namespace BusinessFlow.ViewModels
{
    public class EmployeeTeamViewModel
    {
        [DisplayName("Employee ID")]
        public int EmployeeID { get; set; }
        [DisplayName("Full Name")]
        public string EmployeeName { get; set; }
        [DataType (DataType.EmailAddress)]
        [DisplayName("Email ID")]
        public string EmployeeEmail { get; set; }
        [DisplayName("Contact Number")]
        public string ContactNumber { get; set; }
        [DisplayName("Has Assignment")]
        [MaxLength(5)]
        public string isTaskAssigned { get; set; }
        [DisplayName("Number of Assigments")]
        public int AssignmentCount { get; set; }
        [DisplayName("Is Team Lead")]
        public string isTeamLeader { get; set; }
        public string TeamName { get; set; }
        public string TeamLeaderName { get; set; }

    }
}
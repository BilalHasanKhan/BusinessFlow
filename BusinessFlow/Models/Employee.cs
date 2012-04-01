using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BusinessFlow.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        [ScaffoldColumn(false)]
        public int ContactID { get; set; }
        [UIHint("Contact")][ForeignKey("ContactID")]    
        public Contact EmployeeContact { get; set; }
        public string EmployeeRole { get; set; }
        public int TeamID { get; set; }
        [ForeignKey("TeamID")]
        public Team EmployeeTeam { get; set; }
        [DefaultValue(false)]
        public bool IsTaskAssigned { get; set; }
        public int AssignmentCount { get; set; }
       
        
    }
}
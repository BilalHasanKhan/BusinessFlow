using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BusinessFlow.Models;
using System.ComponentModel;

namespace BusinessFlow.ViewModels
{
    public class TeamProjectsViewModel
    {
        public int ProjectID { get; set; }
        public DateTime ConfirmDate { get; set; }
        public string Requirement { get; set; }
        public string DesignerName { get; set; }
        [DisplayName("Assigned Team")]    
        public string Team { get; set; }
        public string Status { get; set; }
        public DateTime DeliveryDate { get; set; }
        [DisplayName("Requirement Doc")]
        public string FileName { get; set; }

    }
}
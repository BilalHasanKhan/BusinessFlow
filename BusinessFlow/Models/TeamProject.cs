using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BusinessFlow.Models
{
    public class TeamProject
    {
        [Key]
        public int TeamProjectID { get; set; }
        public int TeamID { get; set; }
        [ForeignKey("TeamID")]
        public Team Team { get; set; }
        public int ProjectID { get; set; }
        [ForeignKey("ProjectID")]
        public Project Project { get; set; }
    }
}
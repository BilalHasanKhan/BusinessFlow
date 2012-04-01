using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BusinessFlow.Models
{
    public class EnquiryDetails
    {
        [Key][ScaffoldColumn(false)]
        public int EnquiryDetailsID { get; set; }
        public string FileName { get; set; }
        [ScaffoldColumn(false)]
        public string UniqueName { get; set; }
        public double Amount { get; set; }
        [ScaffoldColumn(false)]
        public int EnquiryID { get; set; }
        public virtual Enquiry Enquiry { get; set; } 

    }
}
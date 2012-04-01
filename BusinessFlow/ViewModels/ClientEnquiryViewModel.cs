using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BusinessFlow.Models;
using System.ComponentModel;

namespace BusinessFlow.ViewModels
{
    public class ClientEnquiryViewModel
    {
        public int EnquiryID { get; set; }
        [DisplayName("Requirement Summary")]
        public string Requirement { get; set; }
        [DataType(DataType.Date)]
        public DateTime TentativeDeliveryDate{get; set;}
        [DisplayName("Amount Paid")]
        public double Amount { get; set; }
        public string Status {get;set;}



    }
}
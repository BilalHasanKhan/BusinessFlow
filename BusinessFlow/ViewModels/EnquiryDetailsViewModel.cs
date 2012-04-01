using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BusinessFlow.Models;

namespace BusinessFlow.ViewModels
{
    public class EnquiryDetailsViewModel
    {
        public Enquiry Enquiry { get; set; }
        public EnquiryDetails EnquiryDetails { get; set; }
    }
}
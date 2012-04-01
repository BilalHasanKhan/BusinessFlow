using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BusinessFlow.Models;


namespace BusinessFlow.ViewModels
{
    public class AccountClientDetailsViewModel
    {
        public Enquiry Enquiry1 { get; set; }
        public EnquiryDetails EnquiryDetails1 { get; set; }
        public ClientRegister ClientRegister { get; set; }
    }
}
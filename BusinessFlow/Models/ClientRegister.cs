using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BusinessFlow.Models
{
    public class ClientRegister
    {
        [Key]
        [ScaffoldColumn(false)]
        public int ClientID { get; set; }
        [ScaffoldColumn(false)]
        public int ContactID { get; set; }
        [ScaffoldColumn(false)]
        public int BillingAddressID { get; set; }
        [ScaffoldColumn(false)]
        public int DeliveryAddressID { get; set; }
        public string AdditionalInformation { get; set; }
        public double Amount { get; set; }
        [ScaffoldColumn(false)]
        public int EnquiryID { get; set; }
        public virtual Enquiry Enquiry { get; set; }
        [ForeignKey("ContactID")]
        public virtual Contact Contact { get; set; }
        [UIHint("BillingAddID")]
        [ForeignKey("BillingAddressID")]
        public virtual Address BillingAddID { get; set; }
        [UIHint("BillingAddID")]
        [ForeignKey("DeliveryAddressID")]
        public virtual Address DeliveryAddID { get; set; }
    }
}
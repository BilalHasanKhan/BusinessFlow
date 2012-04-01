using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BusinessFlow.Models
{
    public class Enquiry
    {
        [Key]
        public int EnquiryID { get; set; }
        [ScaffoldColumn(false)]
        public int ContactID { get; set; }
        [ScaffoldColumn(false)]
        public int AddressID { get; set; }
        [ScaffoldColumn(false)]
        public int DesignerContactID { get; set; }
        [DataType (DataType.Date)][DisplayName("Tentative Delivery Date")]
        public DateTime TentativeDeliveryDate { get; set; }
        public string Requirement { get; set; }
        public int StatusId { get; set; }
        public int? EnquiryDetailsID { get; set; }
        [ForeignKey("ContactID")]
        public virtual Contact Contact {get;set;}
        [ForeignKey("AddressID")]
        public virtual Address SiteAddress { get; set; }
        [ForeignKey("DesignerContactID")]
        [UIHint("Contact")]
        public virtual Contact DesignerContact { get; set; }
        [ForeignKey("StatusId")][UIHint("Status")]
        public virtual StatusMaster Status { get; set; }
      

    }
}
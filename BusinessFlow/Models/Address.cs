using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BusinessFlow.Models
{
    public class Address
    {
        [Key]
        [ScaffoldColumn(false)]
        public int AddressID { get; set; }
        [DisplayName("Site Address")]
        public string address { get; set; }
        
        public virtual ICollection<Enquiry> Enquiry { get; set; }
        public virtual ICollection<ClientRegister> ClientRegister { get; set; }
    }
}

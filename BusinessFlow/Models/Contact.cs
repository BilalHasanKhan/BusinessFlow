using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BusinessFlow.Models
{
   public class Contact
    {
       [Key]
       [ScaffoldColumn(false)]
       public int ContactID { get; set; }
        [Required]
        [DisplayName("Name")]
        public string ContactName { get; set; }
        [Required]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string ContactEmail { get; set; }
        [Required]
        [DisplayName("Phone Numer")]
        [DataType(DataType.PhoneNumber)]
        public string MobileNumber { get; set; }
        [DisplayName("Alternate Name")]
        public string AlternateName { get; set; }
        [DisplayName("Alternate Email")]
        [DataType(DataType.EmailAddress)]
        public string AlternateEmail { get; set; }
        [DisplayName("Alternate Phone Numer")]
        [DataType(DataType.PhoneNumber)]
        public string AlternateMobileNumber { get; set; }
        public virtual ICollection<Enquiry> Enquiry { get; set; }
        public virtual ICollection<Employee> Employee { get; set; } 
       
    }
}

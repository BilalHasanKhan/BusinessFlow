using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BusinessFlow.Models
{
    public class Role
    {
        //Membership required
        [Key()]
        public virtual Guid RoleId { get; set; }
        [Required()]
        [MaxLength(100)]
        public virtual string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }

        //Optional
        [MaxLength(250)]
        public virtual string Description { get; set; }
    }
}
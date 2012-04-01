using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace BusinessFlow.Models
{
    public class StatusType
    {
        [Key]
       public int StatusTypeId { get; set; }
       public string Type { get; set; }
       public ICollection<StatusMaster> StatusMaster { get; set; }

        
    }
}
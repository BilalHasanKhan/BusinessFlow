using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BusinessFlow.Models;
using System.ComponentModel;

namespace BusinessFlow.ViewModels
{
    public class AccountsViewModel
    {
        [DisplayName("ID")]
       public int EnquiryId
        {
            get;
            set;
        }

      public  string ContactName
        {
            get;
            set;
        }

       public string Requirement
        {
            get;
            set;
            
        }
     [DataType(DataType.Date)][UIHint("Date")]
       public DateTime TentativeDeliveryDate
        {
            get;
            set;
        }

     public  string DesignerEmail
        {
            get;
            set;

        }

        [UIHint("Status")]
      public string Status
        {
            get;
            set;
        }
      public string SiteAddress
      {
          get;
          set;
      }
      public double Amount
      {
          get;

          set;

      }

     
      public string Attachment
      {
          get;
          set;
      }
    }
}
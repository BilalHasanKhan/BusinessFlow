using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc.Mailer;
using System.Net.Mail;
using BusinessFlow.Models;

namespace BusinessFlow.Mailers
{ 
    public interface IEnquiryMailer
    {
				
		MailMessage Submitted(string email,int enquiryId);
		
				
		MailMessage Update();
		
		
	}
}
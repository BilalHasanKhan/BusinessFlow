using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc.Mailer;
using System.Net.Mail;

namespace BusinessFlow.Mailers
{ 
    public interface IClient
    {
				
		MailMessage Registered();
		
				
		MailMessage InfoUpdated();
		
		
	}
}
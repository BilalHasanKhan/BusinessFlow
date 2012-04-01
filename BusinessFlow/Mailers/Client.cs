using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc.Mailer;
using System.Net.Mail;

namespace BusinessFlow.Mailers
{ 
    public class Client : MailerBase, IClient     
	{
		public Client():
			base()
		{
			MasterName="_Layout";
		}

		
		public virtual MailMessage Registered()
		{
			var mailMessage = new MailMessage{Subject = "Registered"};
			
			//mailMessage.To.Add("some-email@example.com");
			//ViewBag.Data = someObject;
			PopulateBody(mailMessage, viewName: "Registered");

			return mailMessage;
		}

		
		public virtual MailMessage InfoUpdated()
		{
			var mailMessage = new MailMessage{Subject = "InfoUpdated"};
			
			//mailMessage.To.Add("some-email@example.com");
			//ViewBag.Data = someObject;
			PopulateBody(mailMessage, viewName: "InfoUpdated");

			return mailMessage;
		}

		
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc.Mailer;
using System.Net.Mail;
using BusinessFlow.Models;

namespace BusinessFlow.Mailers
{ 
    public class EnquiryMailer : MailerBase, IEnquiryMailer     
	{
		public EnquiryMailer():
			base()
		{
			MasterName="_Layout";
		}

		
		public virtual MailMessage Submitted(string email,int enquiryId)
		{
			var mailMessage = new MailMessage{Subject = "Enquiry has been submitted for you."};

            mailMessage.To.Add(email);
            ViewBag.EnquiryId = enquiryId;
           	PopulateBody(mailMessage, viewName: "Submitted");

			return mailMessage;
		}

		
		public virtual MailMessage Update()
		{
			var mailMessage = new MailMessage{Subject = "Update"};
			
			//mailMessage.To.Add("some-email@example.com");
			//ViewBag.Data = someObject;
			PopulateBody(mailMessage, viewName: "Update");

			return mailMessage;
		}

		
	}
}
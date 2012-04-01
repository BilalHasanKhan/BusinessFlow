using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessFlow.Models;
using Telerik.Web.Mvc;
using BusinessFlow.ViewModels;
using System.Web.Security;

namespace BusinessFlow.Controllers
{   
    public class AccountsController : Controller
    {
       private readonly IContactRepository contactRepository;
		private readonly IAddressRepository addressRepository;
		private readonly IEnquiryRepository enquiryRepository;
        private readonly IStatusRepository statusRepository;
        private readonly IEnquiryDetailsRepository enquiryDetailsRepository;
        private readonly IClientRegisterRepository clientRegisterRepository;
        
	
        public AccountsController(IContactRepository contactRepository, IAddressRepository addressRepository, IEnquiryRepository enquiryRepository, IStatusRepository statusRepository, IEnquiryDetailsRepository enquiryDetailsRepository, IClientRegisterRepository clientRegisterRepository)
        {
			this.contactRepository = contactRepository;
			this.addressRepository = addressRepository;
			this.enquiryRepository = enquiryRepository;
            this.statusRepository = statusRepository;
            this.enquiryDetailsRepository = enquiryDetailsRepository;
            this.clientRegisterRepository = clientRegisterRepository;
        }

        //
        // GET: /Accounts/

        public ViewResult AccountsDashboard()
        {
            //var result= from p in  this.statusRepository.All where this.statusRepository.All.Whe
            ViewBag.Status = statusRepository.All;
            //if (statusRepository.Find(2).StatusName.Equals("")) ;

           
          
            //List<Enquiry> enquirylist = enquiryRepository.All.ToList();
            //ViewData["Enquiries"] = enquirylist;
            return View();
        }

        [GridAction]
        public ActionResult Select()
        {
            List<Enquiry> enquiries = enquiryRepository.All.ToList();
            List<EnquiryDetails> enquiryDetails = enquiryDetailsRepository.All.ToList();

            var data = from e in enquiries
                       join ed in enquiryDetails on e.EnquiryID equals ed.EnquiryID
                       where ed.Amount >= Constants.ThresholdAmount
                       select new AccountsViewModel// Use anonymous type to avoid JSON serialization exceptions due to circular object references. Also serialize only the required properties (for performance)
                       {
                           EnquiryId = e.EnquiryID,
                           ContactName = e.Contact.ContactName,
                           Requirement = e.Requirement,
                           SiteAddress = e.SiteAddress.address,
                           TentativeDeliveryDate = e.TentativeDeliveryDate,
                           DesignerEmail = e.DesignerContact.ContactEmail,
                           Amount = ed.Amount,
                           Attachment = ed.FileName == null ? "No" : "Yes",
                           Status = e.Status.StatusName

                       };

            return View(new GridModel(data));
        }

        [GridAction]
        public ActionResult Update(Enquiry enquiry)
        {
            var actualEnquiry = enquiryRepository.Find(enquiry.EnquiryID);

            if (actualEnquiry != null && ModelState.IsValid)
            {
                actualEnquiry.StatusId = enquiry.Status.StatusId;
                actualEnquiry.TentativeDeliveryDate = enquiry.TentativeDeliveryDate;
                actualEnquiry.Requirement = actualEnquiry.Requirement;

                enquiryRepository.InsertOrUpdate(actualEnquiry);
                enquiryRepository.Save();
            }

            List<Enquiry> enquiries = enquiryRepository.All.ToList();
            var enquiryDetails = enquiryDetailsRepository.All;
            var data = from e in enquiries
                       join ed in enquiryDetails on e.EnquiryID equals ed.EnquiryID
                       where ed.Amount >= Constants.ThresholdAmount
                       select new AccountsViewModel// Use anonymous type to avoid JSON serialization exceptions due to circular object references. Also serialize only the required properties (for performance)
                       {
                           EnquiryId = e.EnquiryID,
                           ContactName = e.Contact.ContactName,
                           Requirement = e.Requirement,
                           SiteAddress = e.SiteAddress.address,
                           TentativeDeliveryDate = e.TentativeDeliveryDate,
                           DesignerEmail = e.DesignerContact.ContactEmail,
                           Amount = ed.Amount,
                           Attachment = ed.FileName == null ? "No" : "Yes",
                           Status = e.Status.StatusName

                       };

            return View(new GridModel(data));
        }

        [GridAction]

        public ActionResult SelectNoAdvance()
        {
            List<Enquiry> enquiries = enquiryRepository.All.ToList();
            List<EnquiryDetails> enquiryDetails = enquiryDetailsRepository.All.ToList();

            var data = from e in enquiries
                       join ed in enquiryDetails on e.EnquiryID equals ed.EnquiryID
                       where ed.Amount < Constants.ThresholdAmount
                       select new AccountsViewModel// Use anonymous type to avoid JSON serialization exceptions due to circular object references. Also serialize only the required properties (for performance)
                       {
                           EnquiryId = e.EnquiryID,
                           ContactName = e.Contact.ContactName,
                           Requirement = e.Requirement,
                           SiteAddress = e.SiteAddress.address,
                           TentativeDeliveryDate = e.TentativeDeliveryDate,
                           DesignerEmail = e.DesignerContact.ContactEmail,
                           Attachment = ed.FileName == null ? "No" : "Yes",
                           Status = e.Status.StatusName

                       };

            return View(new GridModel(data));
        }

        //
        // GET: /Accounts/Details/5

        public ViewResult Details(int id)
        {
            Enquiry enq = enquiryRepository.Find(id);
            EnquiryDetails enquirydetails = enquiryDetailsRepository.FindByEnquiryID(id);
            ClientRegister clientDetails = clientRegisterRepository.FindEnquiryId(id);
            List<Contact> contactlist = new List<Contact>();
            List<Address> addresslist = new List<Address>();
            List<Contact> designer = new List<Contact>();
            Contact contact = enq.Contact;
            Address address = enq.SiteAddress;
            contactlist.Add(contact);
            addresslist.Add(address);
            ViewData["Contact"] = contactlist;
            ViewData["Address"] = addresslist;
            contact = enq.DesignerContact;
            designer.Add(contact);
            ViewData["Designer"] = designer;
            AccountsDetailsViewModel vwModel = new AccountsDetailsViewModel();
            List<EnquiryDetails> detailsList = new List<EnquiryDetails>();
            detailsList.Add(enquirydetails);
            vwModel.Enquiry = enq;
            vwModel.EnquiryDetails = enquirydetails;
            ViewData["Details"] = detailsList;
            if (clientDetails != null)
            {
                AccountClientDetailsViewModel vWModelNew = new AccountClientDetailsViewModel();
                List<ClientRegister> detailsList1 = new List<ClientRegister>();
                detailsList1.Add(clientDetails);
                vWModelNew.Enquiry1 = enq;
                vWModelNew.EnquiryDetails1 = enquirydetails;
                vWModelNew.ClientRegister = clientDetails;
                ViewData["Details"] = detailsList1;
                return View("Details_All", vWModelNew);
            }
            else
            {
                return View("Details", vwModel);
            }
        }
        //{
        //    Enquiry enquiry = enquiryRepository.Find(id);
        //    return View(enquiry);
        //}

        //
        // GET: /Accounts/Create

        public ActionResult Create()
        {
          
            return View();
        } 

        //
        // POST: /Accounts/Create

        [HttpPost]
        public ActionResult Create(Enquiry enquiry)
        {
            if (ModelState.IsValid)
            {
               
                return RedirectToAction("Index");  
            }

          
            return View(enquiry);
        }
        
        //
        // GET: /Accounts/Edit/5
 
        public ActionResult Edit(int id)
        {
            
            return View();
        }

        //
        // POST: /Accounts/Edit/5

        [HttpPost]
        public ActionResult Edit(Enquiry enquiry)
        {
            if (ModelState.IsValid)
            {
             
                return RedirectToAction("Index");
            }
           
            return View(enquiry);
        }

        //
        // GET: /Accounts/Delete/5
 
        public ActionResult Delete(int id)
        {
         
            return View();
        }

        //
        // POST: /Accounts/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
          
            return RedirectToAction("Index");
        }

        public ActionResult ClientRegister(int Id)
        {
            ViewBag.EnquiryID = Id;
            TempData["EnquiryIDClient"] = Id;
            return View();
        }
        [HttpPost]
        public ActionResult ClientRegister(ClientRegister form)
        {
            ClientRegister details = new ClientRegister();
            details = form;
            int enqID = Convert.ToInt32(TempData["EnquiryIdClient"]);
            //RegisterModel regMod = new RegisterModel();
            //AccountController accController = new AccountController();
            Enquiry enq = new Enquiry();
            enq = enquiryRepository.Find(enqID);
            //regMod.UserName = enq.Contact.ContactEmail;
            string Password = PasswordGeneration(enq.Contact.ContactName, enq.Contact.MobileNumber);
            //regMod.Email = enq.Contact.ContactEmail;
            //accController.Register(regMod);
            MembershipCreateStatus createStatus;
            CodeFirstMembershipProvider provider = new CodeFirstMembershipProvider();
            provider.CreateAccount(enq.Contact.ContactEmail, Password, enq.Contact.ContactEmail, out createStatus);
            CodeFirstRoleProvider roleProvider = new CodeFirstRoleProvider();
            if (createStatus == MembershipCreateStatus.Success)
            {
                if (!roleProvider.RoleExists("Client"))
                {
                    roleProvider.CreateRole("Client");
                }

                string[] roles = { "Client" };
                string[] users = {enq.Contact.ContactEmail.ToString()};
                roleProvider.AddUsersToRoles(users, roles);

            }
            details.EnquiryID = Convert.ToInt32(TempData["EnquiryIdClient"]);
            details.ContactID = enq.ContactID; // Please see this--Pankaj
            clientRegisterRepository.InsertOrUpdate(details);
            clientRegisterRepository.Save();
            return RedirectToAction("Details", new { id = details.EnquiryID });
        }

        public ActionResult ConfirmProject(int Id)
        {
            ViewBag.EnquiryID = Id;
            TempData["EnquiryIDClient"] = Id;
            Project p = new Project();
            p.EnquiryID = Id;
            BusinessFlow.Controllers.ProjectsController pdc = new ProjectsController();
            try
            {
                pdc.Create(p);
                new JavaScriptResult { Script = "alert('You are done!');" };
            }
            catch
            {
                return new JavaScriptResult { Script = "alert('You are not done!');" };
            }
            return View();
        }
        //[HttpPost]
        //public ActionResult ConfirmProject(FormCollection form)
        //{
        //    //ClientRegister details = new ClientRegister();
        //    //details.Amount = Convert.ToDouble(form.GetValue("amount").AttemptedValue);
        //    //details.ClientName = form.GetValue("clientName").AttemptedValue.ToString();
        //    //details.EnquiryID = Convert.ToInt32(TempData["EnquiryIdClient"]);
        //    //details.Password = PasswordGeneration( form.GetValue("clientName").AttemptedValue.ToString());
        //    //clientRegisterRepository.InsertOrUpdate(details);
        //    //clientRegisterRepository.Save();
        //    //return RedirectToAction("Details", new { id = details.EnquiryID });
        //}

        public string PasswordGeneration(string name,string number)
        {
            return (name.Substring(0,3) + number.Substring(number.Length-3));
        }

    }
}
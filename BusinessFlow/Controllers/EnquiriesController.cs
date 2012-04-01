using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessFlow.Models;
using System.IO;
using BusinessFlow.ViewModels;
using Telerik.Web.Mvc;
using Mvc.Mailer;
using BusinessFlow.Mailers;

namespace BusinessFlow.Controllers
{   
    public class EnquiriesController : Controller
    {
		private readonly IContactRepository contactRepository;
		private readonly IAddressRepository addressRepository;
		private readonly IEnquiryRepository enquiryRepository;
        private readonly IStatusRepository statusRepository;
        private readonly IEnquiryDetailsRepository enquiryDetailsRepository;


        public EnquiriesController(IContactRepository contactRepository, IAddressRepository addressRepository, IEnquiryRepository enquiryRepository, IStatusRepository statusRepository, IEnquiryDetailsRepository enquiryDetailsRepository)
        {
			this.contactRepository = contactRepository;
			this.addressRepository = addressRepository;
			this.enquiryRepository = enquiryRepository;
            this.statusRepository = statusRepository;
            this.enquiryDetailsRepository = enquiryDetailsRepository;
        }

        //
        // GET: /Enquiries/

        public ViewResult Index()
        {
            ViewBag.Status = statusRepository.All;
            return View();
        }

        public ViewResult QuotationDashboard()
        {
            ViewBag.Status = statusRepository.All;
            return View();
        }

        [GridAction]
        public ActionResult Select()
        {
            List<Enquiry> enquiries = enquiryRepository.All.ToList();
           

            var data = from e in enquiries
                       select new AccountsViewModel// Use anonymous type to avoid JSON serialization exceptions due to circular object references. Also serialize only the required properties (for performance)
                       {
                           EnquiryId = e.EnquiryID,
                           ContactName = e.Contact.ContactName,
                           Requirement = e.Requirement,
                           SiteAddress = e.SiteAddress.address,
                           TentativeDeliveryDate = e.TentativeDeliveryDate,
                           DesignerEmail = e.DesignerContact.ContactEmail,
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
            var data = from e in enquiries
                       select new AccountsViewModel// Use anonymous type to avoid JSON serialization exceptions due to circular object references. Also serialize only the required properties (for performance)
                       {
                           EnquiryId = e.EnquiryID,
                           ContactName = e.Contact.ContactName,
                           Requirement = e.Requirement,
                           SiteAddress = e.SiteAddress.address,
                           TentativeDeliveryDate = e.TentativeDeliveryDate,
                           DesignerEmail = e.DesignerContact.ContactEmail,
                           Status = e.Status.StatusName

                       };

            return View(new GridModel(data));
        }

        //
        // GET: /Enquiries/Details/5

        public ViewResult Details(int id)
        {
            Enquiry enquiry = enquiryRepository.Find(id);
            EnquiryDetails enquirydetails = enquiryDetailsRepository.FindByEnquiryID(id);
            List<Contact> contactlist = new List<Contact>();
            List<Address> addresslist = new List<Address>();
            List<Contact> designer = new List<Contact>();
            Contact contact = enquiry.Contact;
            Address address = enquiry.SiteAddress;
            contactlist.Add(contact);
            addresslist.Add(address);
            ViewData["Contact"] = contactlist;
            ViewData["Address"] = addresslist;
            contact = enquiry.DesignerContact;
            designer.Add(contact);
            ViewData["Designer"] = designer;
            if (enquirydetails != null)
            {
                EnquiryDetailsViewModel vwModel = new EnquiryDetailsViewModel();
                List<EnquiryDetails> detailsList = new List<EnquiryDetails>();
                detailsList.Add(enquirydetails);
                vwModel.Enquiry = enquiry;
                vwModel.EnquiryDetails = enquirydetails;
                ViewData["Details"] = detailsList;
                return View("Details_All",vwModel);
            }
            else
            {
                return View(enquiry);
            }
        }

        //
        // GET: /Enquiries/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
			ViewBag.PossibleContacts = contactRepository.All;
			ViewBag.PossibleAddresses = addressRepository.All;
			ViewBag.PossibleDesignerContacts = contactRepository.All;
            return View();
        } 

        //
        // POST: /Enquiries/Create
       [AllowAnonymous]
        [HttpPost]
        public ActionResult Create(Enquiry enquiry)
        {
            if (ModelState.IsValid) {
                              
                enquiry.StatusId = statusRepository.Find(Convert.ToInt32(Status.Await_Appoint)).StatusId;
                enquiryRepository.InsertOrUpdate(enquiry);
                enquiryRepository.Save();
                var mailer = new EnquiryMailer();
                var msg = mailer.Submitted(enquiry.Contact.ContactEmail,enquiry.EnquiryID);
                msg.Send();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleContacts = contactRepository.All;
				ViewBag.PossibleAddresses = addressRepository.All;
				ViewBag.PossibleDesignerContacts = contactRepository.All;
				return View();
			}
        }
        
        //
        // GET: /Enquiries/Edit/5
 
        public ActionResult Edit(int id)
        {
			ViewBag.PossibleContacts = contactRepository.All;
			ViewBag.PossibleAddresses = addressRepository.All;
			ViewBag.PossibleDesignerContacts = contactRepository.All;
             return View(enquiryRepository.Find(id));
        }

        //
        // POST: /Enquiries/Edit/5

        [HttpPost]
        public ActionResult Edit(Enquiry enquiry)
        {
            if (ModelState.IsValid) {
                enquiryRepository.InsertOrUpdate(enquiry);
                enquiryRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleContacts = contactRepository.All;
				ViewBag.PossibleAddresses = addressRepository.All;
				ViewBag.PossibleDesignerContacts = contactRepository.All;
				return View();
			}
        }

        //
        // GET: /Enquiries/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(enquiryRepository.Find(id));
        }

        //
        // POST: /Enquiries/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            enquiryRepository.Delete(id);
            enquiryRepository.Save();

            return RedirectToAction("Index");
        }

        public ActionResult Save(IEnumerable<HttpPostedFileBase> quotation, string enquiryId)
        {
            var fileName = "";
            System.Guid saveName = System.Guid.Empty ;
            foreach (var file in quotation)
            {
                 fileName = Path.GetFileName(file.FileName);
                 saveName  = Guid.NewGuid();
                 var destinationPath = Path.Combine(Server.MapPath("~/App_Data"), saveName.ToString());
                file.SaveAs(destinationPath);
                
              
            }

            return Json(new { status = fileName, savedName=saveName }, "text/plain");

        }

        public ActionResult Remove(string[] filenames)
        {

            foreach(var fullname in filenames)
            {
                var fileName = Path.GetFileName(fullname);
                var deletePath = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                if(System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
            }

            return Content("");
        }
        public ActionResult AdditionalDetails(int Id)
        {
            
            ViewBag.EnquiryId= Id;
            TempData["EnquiryId"] = Id;
            return View();
        }
        [HttpPost]
        public ActionResult AdditionalDetails(FormCollection form)
        {
            EnquiryDetails details = new EnquiryDetails();
           details.Amount = Convert.ToDouble(form.GetValue("amount").AttemptedValue);
           details.FileName = form.GetValue("uploadpath").AttemptedValue.ToString();
           details.UniqueName = form.GetValue("savedName").AttemptedValue.ToString();
           details.EnquiryID = Convert.ToInt32(TempData["EnquiryId"]);
           enquiryDetailsRepository.InsertOrUpdate(details);
           enquiryDetailsRepository.Save();
           return RedirectToAction("Details", new { id = details.EnquiryID });
        }
        public ActionResult Download(int Id)
        {
            var enquiryDetails = enquiryDetailsRepository.Find(Id);
            string FileName = enquiryDetails.FileName;
            var file = Server.MapPath("~/App_Data/" + enquiryDetails.UniqueName.ToString()); 
            return File(file, "application/octet-stream", FileName); 

        }


        public ActionResult ConfirmProject(int id)
        {
            var enquiry = enquiryRepository.Find(id);
            enquiry.StatusId = (int)Status.Confirmed;
            enquiryRepository.InsertOrUpdate(enquiry);
            enquiryRepository.Save();
            return View();

        }

      

    }

   
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessFlow.Models;
using Telerik.Web.Mvc;
using BusinessFlow.ViewModels;

namespace BusinessFlow.Controllers
{   
    public class ProductionController : Controller
    {
		private readonly IContactRepository contactRepository;
		private readonly IAddressRepository addressRepository;
        private readonly IStatusRepository statusRepository;
		private readonly IEnquiryDetailsRepository enquirydetailsRepository;
		private readonly IEnquiryRepository enquiryRepository;
        private readonly IProjectRepository projectRepository;
        private readonly ITeamRepository teamRepository;

	    public ProductionController(IContactRepository contactRepository, IAddressRepository addressRepository, IStatusRepository statusRepository, IEnquiryDetailsRepository enquirydetailsRepository, IEnquiryRepository enquiryRepository,IProjectRepository projectRepository,ITeamRepository teamRepository)
        {
			this.contactRepository = contactRepository;
			this.addressRepository = addressRepository;
			this.statusRepository = statusRepository;
			this.enquirydetailsRepository = enquirydetailsRepository;
			this.enquiryRepository = enquiryRepository;
            this.projectRepository = projectRepository;
            this.teamRepository = teamRepository;
        }



        public ViewResult ProductionDashboard()
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
            List<EnquiryDetails> enquiryDetails = enquirydetailsRepository.All.ToList();
            List<Project> projects = projectRepository.All.ToList();

            var data = from e in enquiries
                       join p in projects on e.EnquiryID equals  p.EnquiryID
                       join ed  in enquiryDetails on e.EnquiryID equals ed.EnquiryID
                      
                       select new AccountsViewModel// Use anonymous type to avoid JSON serialization exceptions due to circular object references. Also serialize only the required properties (for performance)
                       {
                           EnquiryId = e.EnquiryID,
                           ContactName = e.Contact.ContactName,
                           Requirement = e.Requirement,
                           SiteAddress = e.SiteAddress.address,
                           TentativeDeliveryDate = e.TentativeDeliveryDate,
                           DesignerEmail = e.DesignerContact.ContactEmail,
                           Attachment = ed.FileName == null ? "No Document Found" : ed.FileName,
                           Status = e.Status.StatusName

                       };

            return View(new GridModel(data));
        }

        [GridAction]

        public ActionResult SelectExistingProjects()
        {
            List<Enquiry> enquiries = enquiryRepository.All.ToList();
            List<EnquiryDetails> enquiryDetails = enquirydetailsRepository.All.ToList();
            List<Project> projects = projectRepository.All.ToList();

            var data = from e in enquiries
                       join p in projects on e.EnquiryID equals  p.EnquiryID
                       join ed  in enquiryDetails on e.EnquiryID equals ed.EnquiryID
                       select new AccountsViewModel// Use anonymous type to avoid JSON serialization exceptions due to circular object references. Also serialize only the required properties (for performance)
                       {
                           EnquiryId = e.EnquiryID,
                           ContactName = e.Contact.ContactName,
                           Requirement = e.Requirement,
                           SiteAddress = e.SiteAddress.address,
                           TentativeDeliveryDate = e.TentativeDeliveryDate,
                           DesignerEmail = e.DesignerContact.ContactEmail,
                           Attachment = ed.FileName == null ? "No Document Found" : ed.FileName,
                           Status = e.Status.StatusName

                       };

            return View(new GridModel(data));
        }

        //
        // GET: /Production/

        public ViewResult Index()
        {
            return View(enquiryRepository.AllIncluding(enquiry => enquiry.Contact, enquiry => enquiry.DesignerContact, enquiry => enquiry.Status));
        }

        //
        // GET: /Production/Details/5

        public ViewResult Details(int id)
        {
            
            return View(enquiryRepository.Find(id));
        }

        //
        // GET: /Production/Create

        public ActionResult Create()
        {
			ViewBag.PossibleContacts = contactRepository.All;
			ViewBag.PossibleAddresses = addressRepository.All;
			ViewBag.PossibleDesignerContacts = contactRepository.All;
			ViewBag.PossibleStatus = statusRepository.All;
			ViewBag.PossibleEnquiryDetails = enquirydetailsRepository.All;
            return View();
        } 

        //
        // POST: /Production/Create

        [HttpPost]
        public ActionResult Create(Enquiry enquiry)
        {
            if (ModelState.IsValid) {
                enquiryRepository.InsertOrUpdate(enquiry);
                enquiryRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleContacts = contactRepository.All;
				ViewBag.PossibleAddresses = addressRepository.All;
				ViewBag.PossibleDesignerContacts = contactRepository.All;
				ViewBag.PossibleStatus = statusRepository.All;
				ViewBag.PossibleEnquiryDetails = enquirydetailsRepository.All;
				return View();
			}
        }
        
        //
        // GET: /Production/Edit/5
 
        public ActionResult Edit(int id)
        {
			ViewBag.PossibleContacts = contactRepository.All;
			ViewBag.PossibleAddresses = addressRepository.All;
			ViewBag.PossibleDesignerContacts = contactRepository.All;
			ViewBag.PossibleStatus = statusRepository.All;
			ViewBag.PossibleEnquiryDetails = enquirydetailsRepository.All;
             return View(enquiryRepository.Find(id));
        }

        //
        // POST: /Production/Edit/5

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
				ViewBag.PossibleStatus = statusRepository.All;
				ViewBag.PossibleEnquiryDetails = enquirydetailsRepository.All;
				return View();
			}
        }

        //
        // GET: /Production/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(enquiryRepository.Find(id));
        }

        //
        // POST: /Production/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            enquiryRepository.Delete(id);
            enquiryRepository.Save();

            return RedirectToAction("Index");
        }


        public ActionResult Download()
        {
            int enquiryId = Convert.ToInt32(Request.QueryString["EnquiryId"]);
            var enquiryDetails = enquirydetailsRepository.FindByEnquiryID(enquiryId);
            string FileName = enquiryDetails.FileName;
            var file = Server.MapPath("~/App_Data/" + enquiryDetails.UniqueName.ToString());
            return File(file, "application/octet-stream", FileName);

        }

        public ActionResult Assign()
        {
            Enquiry enq = enquiryRepository.Find(Convert.ToInt32(Request.QueryString["EnquiryId"]));
            Project prj = projectRepository.FindByEnquiryId(enq.EnquiryID);
            ViewData["Enquiry"] = enq.EnquiryID;
            ViewData["Project"] = prj.ProjectID;
            ViewBag.Teams = teamRepository.All.GroupBy(m => m.TeamName).Select(m => m.FirstOrDefault());
            return View();
        }
        [HttpPost]
        public ActionResult Assign(FormCollection col)
        {
            int enquiryId = Convert.ToInt32(col.GetValue("Enquiry").AttemptedValue);
            int projectId = Convert.ToInt32(col.GetValue("Project").AttemptedValue);
            int TeamType = Convert.ToInt32(col.GetValue("TeamType").AttemptedValue);
            string TeamTypeName = (TeamType==1)?"Non Kenwood Team":"Kenwood Team";
            string TeamName = String.Empty;
            if(TeamType==1)
            {
                TeamName = col.GetValue("Non_Kenwood").AttemptedValue.ToString();

            }
            else{

                 TeamName = teamRepository.Find(Convert.ToInt32(col.GetValue("Team").AttemptedValue)).TeamName;
            }
            TeamProject team = new TeamProject();
            team.ProjectID = projectId;
            team.TeamID = Convert.ToInt32(col.GetValue("Team").AttemptedValue);
            BusinessFlowContext context = new BusinessFlowContext();
            context.TeamProjects.Add(team);
            context.SaveChanges();

            return RedirectToAction("TeamAssigned",team);
        }

        public ActionResult TeamAssigned(TeamProject team)
        {
            
            return View(team);

        }
    }
}


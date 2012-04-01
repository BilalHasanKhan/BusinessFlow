using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessFlow.Models;
using Telerik.Web.Mvc;
using BusinessFlow.ViewModels;
using System.Web.Security;

namespace BusinessFlow.Controllers
{   
    public class TeamLeaderController : Controller
    {
		private readonly IEmployeeRepository employeeRepository;
        private readonly IEnquiryRepository enquiryRepository;
        private readonly IEnquiryDetailsRepository enquiryDetailsRepository;
		private readonly IProjectRepository projectRepository;
		private readonly ITaskRepository taskRepository;
        private readonly ITeamRepository teamRepository;
        private readonly IContactRepository contactRepository;
        private readonly ITeamProjectRepository temprojectRepository;
        private readonly IEmployeeTaskRepository empTaskRepository;

        public TeamLeaderController(IEmployeeRepository employeeRepository, IProjectRepository projectRepository, ITaskRepository taskRepository, 
            ITeamRepository teamRepository, IContactRepository contactRepository, IEnquiryDetailsRepository enquiryDetailsRepository, 
            IEnquiryRepository enquiryRepository, ITeamProjectRepository teamprojectRepository, IEmployeeTaskRepository empTaskRepository)
        {
			this.employeeRepository = employeeRepository;
			this.projectRepository = projectRepository;
			this.taskRepository = taskRepository;
            this.teamRepository = teamRepository;
            this.contactRepository = contactRepository;
            this.enquiryRepository = enquiryRepository;
            this.enquiryDetailsRepository = enquiryDetailsRepository;
            this.temprojectRepository = teamprojectRepository;
            this.empTaskRepository = empTaskRepository;
        }


        public ActionResult TeamLeaderDashBoard()
        {
            CodeFirstMembershipProvider membershipProvider = new CodeFirstMembershipProvider();
           MembershipUser user = membershipProvider.GetUser(HttpContext.User.Identity.Name,true);
           string userEmail = user.Email.ToString();
           int ContactID = contactRepository.FindByEmail(userEmail).ContactID;
           int CurrentUserTeamID = employeeRepository.TeamID(ContactID) ;
           Session["CurrentUserTeam"] = CurrentUserTeamID;
           return View();
        }

        public ActionResult MyTeam()
        {

            return View();
        }

        [GridAction]
        public ActionResult SelectTeamMembers()
        {
            
            List<Employee> employees = employeeRepository.All.ToList();
            List<Team> teams = teamRepository.All.ToList();
            List<Contact> contacts = contactRepository.All.ToList();
            int currentuserteam = Convert.ToInt32(Session["CurrentUserTeam"]);
            var data = from e in employees join 
                       t in teams on e.TeamID equals t.TeamID
                       join c in contacts on e.ContactID equals c.ContactID                       
                       where t.TeamID==currentuserteam
                       select new EmployeeTeamViewModel// Use anonymous type to avoid JSON serialization exceptions due to circular object references. Also serialize only the required properties (for performance)
                       {
                           EmployeeID = e.EmployeeID,
                           EmployeeName = c.ContactName,
                           EmployeeEmail = c.ContactEmail,
                           ContactNumber = c.MobileNumber,
                           isTaskAssigned = (e.IsTaskAssigned)? "Yes":"No",
                           AssignmentCount = e.AssignmentCount

                       };

            return View(new GridModel(data));
        }

        public ActionResult TeamProjects()
        {
            return View();
        }

        [GridAction]
        public ActionResult SelectProjects()
        {

            List<Team> teams = teamRepository.All.ToList();
            List<Enquiry> enquiries = enquiryRepository.All.ToList();
            List<EnquiryDetails> enquiryDetails = enquiryDetailsRepository.All.ToList();
            List<Project> projects = projectRepository.All.ToList();
            List<TeamProject> teamprojects = temprojectRepository.All.ToList();
            int currentuserteam = Convert.ToInt32(Session["CurrentUserTeam"]);
            var data = from e in enquiries
                       join ed in enquiryDetails on e.EnquiryID equals ed.EnquiryID
                       join p in projects on e.EnquiryID equals p.EnquiryID
                       join tp in teamprojects on p.ProjectID equals tp.ProjectID
                       join t in teams on tp.TeamID equals t.TeamID
                       where t.TeamID == currentuserteam
                       select new TeamProjectsViewModel// Use anonymous type to avoid JSON serialization exceptions due to circular object references. Also serialize only the required properties (for performance)
                       {
                           ProjectID = p.ProjectID,
                           ConfirmDate = p.ConfirmDate,
                           Requirement = e.Requirement,
                           DesignerName = e.DesignerContact.ContactName,
                           DeliveryDate = e.TentativeDeliveryDate,
                           Team = t.TeamName,
                           Status = e.Status.StatusName,
                           FileName = ed.FileName

                       };

            return View(new GridModel(data));
        }

      #region AddTaskForProjectAction
        public ActionResult AddTaskForProject()
        {
            
            return View();
         
        }

        [GridAction]
        public ActionResult SelectTasks()
        {
            return View(new GridModel(taskRepository.All));

        }

        [GridAction]
        public ActionResult InsertTask(Task task)
        {
            taskRepository.InsertOrUpdate(task);
            taskRepository.Save();
            return View(new GridModel(taskRepository.All));
        }

        [GridAction]
        public ActionResult SaveTask(int id,Task task)
        {
            Task _task = taskRepository.Find(id);
            _task.ProjectID = task.ProjectID;
            _task.TaskName = task.TaskName;
            _task.TaskStartDate = task.TaskStartDate;
            _task.TaskEndDate = task.TaskEndDate;
            taskRepository.InsertOrUpdate(_task);
            taskRepository.Save();
            return View(new GridModel(taskRepository.All));

        }

        [GridAction]
        public ActionResult DeleteTask(int id)
        {

            taskRepository.Delete(id);
            taskRepository.Save();
            return View(new GridModel(taskRepository.All));

        }

    #endregion

        public ActionResult AssignTaskToEmployee()
        {
            int EmployeeID = Convert.ToInt32(Request.QueryString["EmployeeID"]);
            Session["Assignee"] = EmployeeID;
            return View();
        }

        [GridAction]
        public ActionResult _CheckBoxesAjax()
        {
            return View(new GridModel(GetTasks()));
        }
        public ActionResult DisplayCheckedTasks(int[] checkedRecords)
        {
            checkedRecords = checkedRecords ?? new int[] { };
            List<TaskViewModel> data = GetTasks().Where(o => checkedRecords.Contains(o.TaskID)).ToList();

            foreach( var tasks in data)
            {
            EmployeeTask empTask = new EmployeeTask();
            empTask.EmployeeID = Convert.ToInt32(Session["Assignee"]);
            empTask.TaskID = tasks.TaskID;
            empTaskRepository.InsertOrUpdate(empTask);
            empTaskRepository.Save();

            }

            //Update Employee record for task assignment
            Employee emp = employeeRepository.Find(Convert.ToInt32(Session["Assignee"]));
            emp.IsTaskAssigned = true;
            emp.AssignmentCount = emp.AssignmentCount+data.Count;
            employeeRepository.InsertOrUpdate(emp);
            employeeRepository.Save();

            //update Task record
            foreach (var task in data)
            {
                Task _task = taskRepository.Find(task.TaskID);
                _task.IsAssigned = true;
                taskRepository.InsertOrUpdate(_task);
                taskRepository.Save();

            }

            return PartialView("CheckedTasks", data );
        }

        private List<TaskViewModel> GetTasks()
        {

           
            List<Team> teams = teamRepository.All.ToList();
            List<Project> projects = projectRepository.All.ToList();
            List<TeamProject> teamprojects = temprojectRepository.All.ToList();
            List<Task> tasks = taskRepository.All.ToList();

            int currentuserteam = Convert.ToInt32(Session["CurrentUserTeam"]);
            var data = from p in projects 
                       join tp in teamprojects on p.ProjectID equals tp.ProjectID
                       join t in teams on tp.TeamID equals t.TeamID
                       join ta in tasks on p.ProjectID equals ta.ProjectID
                       where t.TeamID == currentuserteam && ta.IsAssigned==false
                       select new TaskViewModel// Use anonymous type to avoid JSON serialization exceptions due to circular object references. Also serialize only the required properties (for performance)
                       {
                          TaskID = ta.TaskID,
                          ProjectID = p.ProjectID,
                          EmployeeID = Convert.ToInt32(Session["Assignee"]),
                          TaskName = ta.TaskName,
                          TaskStartDate = ta.TaskStartDate,
                          TaskEndDate = ta.TaskEndDate
                          
                       };

            return data.ToList<TaskViewModel>();
           
        }
                

        public ActionResult Download()
        {
            int Id = Convert.ToInt32(Request.QueryString["ProjectID"]);
            Project proj = projectRepository.Find(Id);
            var enquiryDetails = enquiryDetailsRepository.FindByEnquiryID(proj.EnquiryID);
            string FileName = enquiryDetails.FileName;
            var file = Server.MapPath("~/App_Data/" + enquiryDetails.UniqueName.ToString());
            return File(file, "application/octet-stream", FileName);

        }
    }
}


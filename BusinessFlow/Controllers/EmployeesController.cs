using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessFlow.Models;
using System.Web.Security;
using BusinessFlow.ViewModels;
using Telerik.Web.Mvc;

namespace BusinessFlow.Controllers
{
    public class EmployeesController : Controller
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

     
        public EmployeesController(IEmployeeRepository employeeRepository, IProjectRepository projectRepository, ITaskRepository taskRepository, 
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
        //
        // GET: /Employee/

        public ActionResult MyTasks()
        {
            CodeFirstMembershipProvider membershipProvider = new CodeFirstMembershipProvider();
            MembershipUser user = membershipProvider.GetUser(HttpContext.User.Identity.Name, true);
            string userEmail = user.Email.ToString();
            int ContactID = contactRepository.FindByEmail(userEmail).ContactID;
            int CurrentUserTeamID = employeeRepository.TeamID(ContactID);
            Employee loggedEmployee = employeeRepository.FindByContactID(ContactID);
            Session["CurrentUserTeam"] = CurrentUserTeamID;
            Session["LoggedInEmployee"] = loggedEmployee;
            return View();
        }

        [GridAction]
        public ActionResult MyDetails()
        {

            List<Employee> employees = employeeRepository.All.ToList();
            List<Team> teams = teamRepository.All.ToList();
            List<Contact> contacts = contactRepository.All.ToList();
            int currentuserteam = Convert.ToInt32(Session["CurrentUserTeam"]);
            Employee loggedInEmployee = Session["LoggedInEmployee"] as Employee;
            var data = from e in employees
                       join
                           t in teams on e.TeamID equals t.TeamID
                       join c in contacts on e.ContactID equals c.ContactID
                       where t.TeamID == currentuserteam && e.EmployeeID==loggedInEmployee.EmployeeID
                       select new EmployeeTeamViewModel// Use anonymous type to avoid JSON serialization exceptions due to circular object references. Also serialize only the required properties (for performance)
                       {
                           EmployeeID = e.EmployeeID,
                           EmployeeName = c.ContactName,
                           EmployeeEmail = c.ContactEmail,
                           ContactNumber = c.MobileNumber,
                           TeamName = t.TeamName,
                           isTeamLeader = User.IsInRole(Constants.TeamLeader)? "Yes" :"No",
                           isTaskAssigned = (e.IsTaskAssigned) ? "Yes" : "No",
                           AssignmentCount = e.AssignmentCount,
                           TeamLeaderName = (from _e in employees join
                                              _t in teams on _e.TeamID equals _t.TeamID 
                                               join _c in contacts on _e.ContactID equals _c.ContactID 
                                               where _t.TeamID==currentuserteam && e.EmployeeRole==Constants.TeamLeader
                                               select new {_c.ContactName}.ContactName).Single().ToString()
                                             

                       };

            return View(new GridModel(data));

        }

        [GridAction]
        public ActionResult MyTaskDetails()
        {
            return View(new GridModel(GetEmployeeTask()));
        }

        [GridAction]
        public ActionResult UpdateTask(EmployeeTaskViewModel taskModel)
        {
            EmployeeTask task = empTaskRepository.FindByTaskId(taskModel.TaskID);
            task.Efforts = taskModel.HoursSpent;
            empTaskRepository.InsertOrUpdate(task);
            empTaskRepository.Save();
            return View(new GridModel(GetEmployeeTask()));

        }

        public List<EmployeeTaskViewModel> GetEmployeeTask()
        {
            List<EmployeeTask> employeeTasks = empTaskRepository.All.ToList();
            List<Task> tasks = taskRepository.All.ToList();
            Employee loggedInEmployee = Session["LoggedInEmployee"] as Employee;

            var data = from e in employeeTasks
                       join
                       t in tasks on e.TaskID equals t.TaskID
                       where e.EmployeeID == loggedInEmployee.EmployeeID
                       select new EmployeeTaskViewModel
                       {
                           TaskID = t.TaskID,
                           ProjectID = t.ProjectID,
                           TaskName = t.TaskName,
                           TaskDetails = t.TaskDescription,
                           TaskStartDate = t.TaskStartDate,
                           TaskEndDate = t.TaskEndDate,
                           TotalHours = Convert.ToInt32(Math.Ceiling((t.TaskEndDate - t.TaskStartDate).TotalHours)),
                           HoursSpent = e.Efforts

                       };

            return data.ToList();

        }

    }
}

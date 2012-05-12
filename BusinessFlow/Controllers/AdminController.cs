using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessFlow.Models;
using System.Data.SqlClient;
using System.Web.Security;

namespace BusinessFlow.Controllers
{
    public class AdminController : Controller
    {
        private readonly ITeamRepository teamRepository;
        private readonly IEmployeeRepository employeeRepository;

        public AdminController(ITeamRepository teamRepository, IEmployeeRepository employeeRepository)
        {
            this.teamRepository = teamRepository;
            this.employeeRepository = employeeRepository;
        }

        //Show Tasks

        public ActionResult AdminTasks()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult AddRole()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddRole(Role role)
        {
            CodeFirstRoleProvider roleProvider = new CodeFirstRoleProvider();
            roleProvider.CreateRole(role.RoleName);
            return View("AdminTasks");

        }

        [AllowAnonymous]
        public ActionResult AddEmployee()
        {
            CodeFirstRoleProvider roleProvider = new CodeFirstRoleProvider();
            //SqlParameter appIdParam = new SqlParameter("@ApplicationId","/");
            List<string> roles = roleProvider.GetAllRoles().ToList(); //context.Database.SqlQuery<string>("dbo.aspnet_Roles_GetAllRoles @ApplicationId", appIdParam).ToList<string>();
            ViewBag.Roles = roles;
            ViewBag.Teams = teamRepository.All;
            return View();
        }

        [HttpPost][AllowAnonymous]
        public ActionResult AddEmployee(Employee employee,FormCollection col)
        {
            Employee emp = new Employee();
            emp.TeamID = Convert.ToInt32(col["EmployeeTeam"]);
            emp.EmployeeRole = col["roles"];
            emp.EmployeeContact = employee.EmployeeContact;
            employeeRepository.InsertOrUpdate(emp);
            employeeRepository.Save();
            string[] roles= {emp.EmployeeRole};
            string[] username = {employee.EmployeeContact.ContactEmail};
            MembershipCreateStatus createStatus;
            CodeFirstMembershipProvider provider = new CodeFirstMembershipProvider();
            provider.CreateAccount(username[0], "KStart123", employee.EmployeeContact.ContactEmail,out createStatus);
            CodeFirstRoleProvider roleProvider = new CodeFirstRoleProvider();
            roleProvider.AddUsersToRoles(username, roles);

            return View("AdminTasks");

        }
        //
        // GET: /Employees/

        public ViewResult Index()
        {
            return View(employeeRepository.AllIncluding());
        }

        //
        // GET: /Employees/Details/5

        public ViewResult Details(int id)
        {
            return View(employeeRepository.Find(id));
        }

        //
        // GET: /Employees/Create

        public ActionResult Create()
        {
            ViewBag.PossibleTeams = teamRepository.All;
            return View();
        }

        //
        // POST: /Employees/Create

        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employeeRepository.InsertOrUpdate(employee);
                employeeRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PossibleTeams = teamRepository.All;
                return View();
            }
        }

        //
        // GET: /Employees/Edit/5

        public ActionResult Edit(int id)
        {
            ViewBag.PossibleTeams = teamRepository.All;
            return View(employeeRepository.Find(id));
        }

        //
        // POST: /Employees/Edit/5

        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employeeRepository.InsertOrUpdate(employee);
                employeeRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PossibleTeams = teamRepository.All;
                return View();
            }
        }

        //
        // GET: /Employees/Delete/5

        public ActionResult Delete(int id)
        {
            return View(employeeRepository.Find(id));
        }

        //
        // POST: /Employees/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            employeeRepository.Delete(id);
            employeeRepository.Save();

            return RedirectToAction("Index");
        }
    }
}


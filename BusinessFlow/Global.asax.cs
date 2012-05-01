using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BusinessFlow.Models;
using BusinessFlow.Filters;
using Ninject;

namespace BusinessFlow
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           // filters.Add(new LogonAuthorize());
            //filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            RegisterDependicies();
            //System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<BusinessFlow.Models.BusinessFlowContext>());
            System.Data.Entity.Database.SetInitializer<BusinessFlowContext>(new DbInitializer());
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            
        }

        private void RegisterDependicies()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ITeamRepository>().To<TeamRepository>();
            kernel.Bind<IEmployeeRepository>().To<EmployeeRepository>();
            kernel.Bind<IAddressRepository>().To<AddressRepository>();
            kernel.Bind<IClientRegisterRepository>().To<ClientRegisterRepository>();
            kernel.Bind<IContactRepository>().To<ContactRepository>();
            kernel.Bind<IEmployeeTaskRepository>().To<EmployeeTaskRepository>();
            kernel.Bind<IEnquiryDetailsRepository>().To<EnquiryDetailsRepository>();
            kernel.Bind<IEnquiryRepository>().To<EnquiryRepository>();
            kernel.Bind<IProjectRepository>().To<ProjectRepository>();
            kernel.Bind<IRoleRepository>().To<RoleRepository>();
            kernel.Bind<IStatusRepository>().To<StatusRepository>();
            kernel.Bind<ITaskRepository>().To<TaskRepository>();
            kernel.Bind<ITeamProjectRepository>().To<TeamProjectRepository>();
            DependencyResolver.SetResolver(new NinjectResolver(kernel));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Devtalk.EF.CodeFirst;


namespace BusinessFlow.Models
{
    public class BusinessFlowContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<BusinessFlow.Models.BusinessFlowContext>());
        public BusinessFlowContext() : base("BusinessFlow") { }
        public DbSet<BusinessFlow.Models.Enquiry> Enquiries { get; set; }

        public DbSet<BusinessFlow.Models.Address> Addresses { get; set; }

        public DbSet<BusinessFlow.Models.Contact> Contacts { get; set; }

        public DbSet<BusinessFlow.Models.StatusType> StatusType { get; set; }

        public DbSet<BusinessFlow.Models.StatusMaster> StatusMaster { get; set; }

        public DbSet<BusinessFlow.Models.EnquiryDetails> EnquiryDetails { get; set; }

        public DbSet<BusinessFlow.Models.ClientRegister> ClientRegister { get; set; }

        public DbSet<BusinessFlow.Models.Project> Projects { get; set; }

        public DbSet<BusinessFlow.Models.Team> Teams { get; set; }

        public DbSet<BusinessFlow.Models.Task> Tasks { get; set; }

        public DbSet<BusinessFlow.Models.Employee> Employees { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Application> Application { get; set; }

        public DbSet<TeamProject> TeamProjects { get; set; }

        public DbSet<EmployeeTask> EmployeeTasks { get; set; }

     

       // public DbSet<BusinessFlow.Models.Project> Project { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Enquiry>().HasRequired(c => c.Contact).WithMany().HasForeignKey(c => c.ContactID).WillCascadeOnDelete(false);
            modelBuilder.Entity<Enquiry>().HasRequired(d => d.DesignerContact).WithMany(e=>e.Enquiry).HasForeignKey(d => d.DesignerContactID).WillCascadeOnDelete(false);
            modelBuilder.Entity<ClientRegister>().HasRequired(d => d.DeliveryAddID).WithMany().HasForeignKey(k => k.DeliveryAddressID).WillCascadeOnDelete(false);
            modelBuilder.Entity<ClientRegister>().HasRequired(d => d.BillingAddID).WithMany(e => e.ClientRegister).HasForeignKey(k => k.BillingAddressID).WillCascadeOnDelete(false);
            modelBuilder.Entity<ClientRegister>().HasRequired(d => d.Enquiry).WithMany().HasForeignKey(k => k.EnquiryID).WillCascadeOnDelete(false);
            //modelBuilder.Entity<Team>().HasRequired(t => t.Project).WithMany(t => t.Teams).HasForeignKey(t => t.ProjectID).WillCascadeOnDelete(false);
           
          //  modelBuilder.Entity<EnquiryDetails>().HasRequired(c => c.Enquiry).WithMany().HasForeignKey(e => e.EnquiryID).WillCascadeOnDelete(false);
          


        }

        

       

        

   
    }

    public class DbInitializer : DbContext
    {
        public DbInitializer()
        {
            Database.SetInitializer(new DontDropDbJustCreateTablesIfModelChanged<BusinessFlowContext>());
        }
    }
    //    protected void Seed(BusinessFlowContext context)
    //    {
    //       // base.Seed(context);

           


    //        var statusType = new List<StatusType>
    //        {
    //             new StatusType { Type="Before 10K"},
    //             new StatusType {Type="After 10K"}
    //        };
    //        statusType.ForEach(s => context.StatusType.Add(s));

    //        var statusMaster = new List<StatusMaster>
    //        {
    //            new StatusMaster { StatusName="Await Appointment", StatusTypeId=1},
    //            new StatusMaster { StatusName="Site Visited", StatusTypeId=1},
    //            new StatusMaster { StatusName="Quote 1 Sent", StatusTypeId=1},
    //            new StatusMaster { StatusName="Quote 2 Sent", StatusTypeId=1},
    //            new StatusMaster { StatusName="Almost Confirmed", StatusTypeId=1},
    //            new StatusMaster { StatusName="Quote 3 Sent", StatusTypeId=1},
    //            new StatusMaster { StatusName="Quote 4 Sent", StatusTypeId=1},
    //            new StatusMaster { StatusName="Await Discussion", StatusTypeId=1},
    //             new StatusMaster { StatusName="Drawings being prepared", StatusTypeId=2},
    //              new StatusMaster { StatusName="Drawings sent, await approval", StatusTypeId=2},
    //               new StatusMaster { StatusName="Revised drawings being prepared", StatusTypeId=2},
    //                new StatusMaster { StatusName="Await approval on revised drawings", StatusTypeId=2},
    //                 new StatusMaster { StatusName="SignOff meeting to be fixed", StatusTypeId=2},
    //                  new StatusMaster { StatusName="SignOff Fixed", StatusTypeId=2}
    //        };

    //        //statusMaster.ForEach(s => context.StatusMaster.Add(s));
    //    }
    //}
}
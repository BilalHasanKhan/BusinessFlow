using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BusinessFlow.Models
{
    public class ProjectRepository : IProjectRepository
    {
        BusinessFlowContext context = new BusinessFlowContext();

        public IQueryable<Project> All
        {
            get { return context.Projects; }
        }

        public IQueryable<Project> AllIncluding(params Expression<Func<Project, object>>[] includeProperties)
        {
            IQueryable<Project> query = context.Projects;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Project Find(int id)
        {
            return context.Projects.Find(id);
        }

        public void InsertOrUpdate(Project projects)
        {
            if (projects.ProjectID == default(int))
            {
                // New entity
                context.Projects.Add(projects);
            }
            else
            {
                // Existing entity
                context.Entry(projects).State = EntityState.Modified;
            }
        }
        public Project FindProjectId(int id)
        {
            return context.Projects.SingleOrDefault(p => p.ProjectID == id);
        }

        public Project FindByEnquiryId(int enquiryId)
        {
            return context.Projects.SingleOrDefault(p => p.EnquiryID == enquiryId);
        }


        public void Delete(int id)
        {
            var project = context.Projects.Find(id);
            context.Projects.Remove(project);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }

    public interface IProjectRepository
    {
        IQueryable<Project> All { get; }
        IQueryable<Project> AllIncluding(params Expression<Func<Project, object>>[] includeProperties);
        Project Find(int id);
        Project FindProjectId(int id);
        Project FindByEnquiryId(int enquiryId);
        void InsertOrUpdate(Project project);
        void Delete(int id);
        void Save();
    }
}
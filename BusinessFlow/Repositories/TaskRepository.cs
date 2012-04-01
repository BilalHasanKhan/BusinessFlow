using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BusinessFlow.Models
{ 
    public class TaskRepository : ITaskRepository
    {
        BusinessFlowContext context = new BusinessFlowContext();

        public IQueryable<Task> All
        {
            get { return context.Tasks; }
        }

        public IQueryable<Task> AllIncluding(params Expression<Func<Task, object>>[] includeProperties)
        {
            IQueryable<Task> query = context.Tasks;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Task Find(int id)
        {
            return context.Tasks.Find(id);
        }

        public List<Task> FindByProject(int projectId)
        {
            return context.Tasks.Where(t => t.ProjectID == projectId).ToList();
        }

        public void InsertOrUpdate(Task task)
        {
            if (task.TaskID == default(int)) {
                // New entity
                context.Tasks.Add(task);
            } else {
                // Existing entity
                context.Entry(task).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var task = context.Tasks.Find(id);
            context.Tasks.Remove(task);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }

    public interface ITaskRepository
    {
        IQueryable<Task> All { get; }
        IQueryable<Task> AllIncluding(params Expression<Func<Task, object>>[] includeProperties);
        Task Find(int id);
        List<Task> FindByProject(int projectId);
        void InsertOrUpdate(Task task);
        void Delete(int id);
        void Save();
    }
}
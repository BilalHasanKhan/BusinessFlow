using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BusinessFlow.Models
{
    public class EmployeeTaskRepository : IEmployeeTaskRepository
    {
        BusinessFlowContext context = new BusinessFlowContext();

        public IQueryable<EmployeeTask> All
        {
            get { return context.EmployeeTasks; }
        }

        public IQueryable<EmployeeTask> AllIncluding(params Expression<Func<EmployeeTask, object>>[] includeProperties)
        {
            IQueryable<EmployeeTask> query = context.EmployeeTasks;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public EmployeeTask Find(int id)
        {
            return context.EmployeeTasks.Find(id);
        }

        public List<EmployeeTask> FindByProject(int projectId)
        {
            return context.EmployeeTasks.Where(t => t.Task.ProjectID == projectId).ToList();
        }

        public EmployeeTask FindByTaskId(int taskId)
        {

            return context.EmployeeTasks.SingleOrDefault(t => t.TaskID==taskId);
        }

        public void InsertOrUpdate(EmployeeTask task)
        {
            if (task.EmployeeTaskID == default(int))
            {
                // New entity
                context.EmployeeTasks.Add(task);
            }
            else
            {
                // Existing entity
                context.Entry(task).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var task = context.EmployeeTasks.Find(id);
            context.EmployeeTasks.Remove(task);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }

    public interface IEmployeeTaskRepository
    {
        IQueryable<EmployeeTask> All { get; }
        IQueryable<EmployeeTask> AllIncluding(params Expression<Func<EmployeeTask, object>>[] includeProperties);
        EmployeeTask Find(int id);
        EmployeeTask FindByTaskId(int taskId);
        List<EmployeeTask> FindByProject(int projectId);
        void InsertOrUpdate(EmployeeTask task);
        void Delete(int id);
        void Save();
    }
}
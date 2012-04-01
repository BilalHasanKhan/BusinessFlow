using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BusinessFlow.Models
{
    public class StatusRepository : IStatusRepository
    {
        BusinessFlowContext context = new BusinessFlowContext();

        public IQueryable<StatusMaster> All
        {
            get { return context.StatusMaster; }
        }

        public IQueryable<StatusMaster> AllIncluding(params Expression<Func<StatusMaster, object>>[] includeProperties)
        {
            IQueryable<StatusMaster> query = context.StatusMaster;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public StatusMaster Find(int id)
        {
            return context.StatusMaster.Find(id);
        }
        public StatusMaster FindByName(string status)
        {
            return context.StatusMaster.SingleOrDefault(s => s.StatusName.Equals(status));

        }
        public void InsertOrUpdate(StatusMaster status)
        {
            if (status.StatusId == default(int))
            {
                // New entity
                context.StatusMaster.Add(status);
            }
            else
            {
                // Existing entity
                context.Entry(status).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var contact = context.StatusMaster.Find(id);
            context.StatusMaster.Remove(contact);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }

    public interface IStatusRepository
    {
        IQueryable<StatusMaster> All { get; }
        IQueryable<StatusMaster> AllIncluding(params Expression<Func<StatusMaster, object>>[] includeProperties);
        StatusMaster Find(int id);
        StatusMaster FindByName(string status);
        void InsertOrUpdate(StatusMaster contact);
        void Delete(int id);
        void Save();
    }
}
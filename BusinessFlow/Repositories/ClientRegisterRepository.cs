using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BusinessFlow.Models
{ 
    public class ClientRegisterRepository : IClientRegisterRepository
    {
        BusinessFlowContext context = new BusinessFlowContext();

        public IQueryable<ClientRegister> All
        {
            get { return context.ClientRegister; }
        }

        public IQueryable<ClientRegister> AllIncluding(params Expression<Func<ClientRegister, object>>[] includeProperties)
        {
            IQueryable<ClientRegister> query = context.ClientRegister;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public ClientRegister Find(int id)
        {
            return context.ClientRegister.Find(id);
        }

        public ClientRegister FindEnquiryId(int id)
        {
            return context.ClientRegister.SingleOrDefault(e => e.EnquiryID == id);
        }

        public void InsertOrUpdate(ClientRegister clientregister)
        {
            if (clientregister.ClientID == default(int)) {
                // New entity
                context.ClientRegister.Add(clientregister);
            } else {
                // Existing entity
                context.Entry(clientregister).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var clientregister = context.ClientRegister.Find(id);
            context.ClientRegister.Remove(clientregister);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }

    public interface IClientRegisterRepository
    {
        IQueryable<ClientRegister> All { get; }
        IQueryable<ClientRegister> AllIncluding(params Expression<Func<ClientRegister, object>>[] includeProperties);
        ClientRegister Find(int id);
        ClientRegister FindEnquiryId(int id);
        void InsertOrUpdate(ClientRegister clientregister);
        void Delete(int id);
        void Save();
    }
}
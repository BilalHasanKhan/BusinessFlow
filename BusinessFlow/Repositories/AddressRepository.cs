using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BusinessFlow.Models
{ 
    public class AddressRepository : IAddressRepository
    {
        BusinessFlowContext context = new BusinessFlowContext();

        public IQueryable<Address> All
        {
            get { return context.Addresses; }
        }

        public IQueryable<Address> AllIncluding(params Expression<Func<Address, object>>[] includeProperties)
        {
            IQueryable<Address> query = context.Addresses;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Address Find(int id)
        {
            return context.Addresses.Find(id);
        }

        public void InsertOrUpdate(Address address)
        {
            if (address.AddressID == default(int)) {
                // New entity
                context.Addresses.Add(address);
            } else {
                // Existing entity
                context.Entry(address).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var address = context.Addresses.Find(id);
            context.Addresses.Remove(address);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }

    public interface IAddressRepository
    {
        IQueryable<Address> All { get; }
        IQueryable<Address> AllIncluding(params Expression<Func<Address, object>>[] includeProperties);
        Address Find(int id);
        void InsertOrUpdate(Address address);
        void Delete(int id);
        void Save();
    }
}
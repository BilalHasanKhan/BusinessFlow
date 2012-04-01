using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BusinessFlow.Models
{ 
    public class EnquiryRepository : IEnquiryRepository
    {
        BusinessFlowContext context = new BusinessFlowContext();

        public IQueryable<Enquiry> All
        {
            get { return context.Enquiries; }
        }

        public IQueryable<Enquiry> AllIncluding(params Expression<Func<Enquiry, object>>[] includeProperties)
        {
            IQueryable<Enquiry> query = context.Enquiries;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Enquiry Find(int id)
        {
            return context.Enquiries.Find(id);
        }

        public void InsertOrUpdate(Enquiry enquiry)
        {
            if (enquiry.EnquiryID == default(int)) {
                // New entity
                context.Enquiries.Add(enquiry);
            } else {
                // Existing entity
                context.Entry(enquiry).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var enquiry = context.Enquiries.Find(id);
            context.Enquiries.Remove(enquiry);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }

    public interface IEnquiryRepository
    {
        IQueryable<Enquiry> All { get; }
        IQueryable<Enquiry> AllIncluding(params Expression<Func<Enquiry, object>>[] includeProperties);
        Enquiry Find(int id);
        void InsertOrUpdate(Enquiry enquiry);
        void Delete(int id);
        void Save();
    }
}
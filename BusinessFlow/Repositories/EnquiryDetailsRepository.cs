using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BusinessFlow.Models
{
    public class EnquiryDetailsRepository : IEnquiryDetailsRepository
    {
        BusinessFlowContext context = new BusinessFlowContext();

        public IQueryable<EnquiryDetails> All
        {
            get { return context.EnquiryDetails; }
        }

        public IQueryable<EnquiryDetails> AllIncluding(params Expression<Func<EnquiryDetails, object>>[] includeProperties)
        {
            IQueryable<EnquiryDetails> query = context.EnquiryDetails;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public EnquiryDetails Find(int id)
        {
            return context.EnquiryDetails.Find(id);
        }

        public EnquiryDetails FindByEnquiryID(int enquiryId)
        {

            return context.EnquiryDetails.SingleOrDefault(e => e.EnquiryID == enquiryId);
        }


        public void InsertOrUpdate(EnquiryDetails enquirydetails)
        {
            if (enquirydetails.EnquiryDetailsID== default(int))
            {
                // New entity
                context.EnquiryDetails.Add(enquirydetails);
            }
            else
            {
                // Existing entity
                context.Entry(enquirydetails).State = EntityState.Modified;
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

    public interface IEnquiryDetailsRepository
    {
        IQueryable<EnquiryDetails> All { get; }
        IQueryable<EnquiryDetails> AllIncluding(params Expression<Func<EnquiryDetails, object>>[] includeProperties);
        EnquiryDetails Find(int id);
        
        void InsertOrUpdate(EnquiryDetails enquiry);
        void Delete(int id);
        void Save();
        EnquiryDetails FindByEnquiryID(int enquiryId);
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BusinessFlow.Models
{
    public class TeamProjectRepository : ITeamProjectRepository
    {
        BusinessFlowContext context = new BusinessFlowContext();

        public IQueryable<TeamProject> All
        {
            get { return context.TeamProjects; }
        }

        public IQueryable<TeamProject> AllIncluding(params Expression<Func<TeamProject, object>>[] includeProperties)
        {
            IQueryable<TeamProject> query = context.TeamProjects;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public TeamProject Find(int id)
        {
            return context.TeamProjects.Find(id);
        }
        
        public void InsertOrUpdate(TeamProject team)
        {
            if (team.TeamID == default(int))
            {
                // New entity
                context.TeamProjects.Add(team);
            }
            else
            {
                // Existing entity
                context.Entry(team).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var team = context.Teams.Find(id);
            context.Teams.Remove(team);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }

    public interface ITeamProjectRepository
    {
        IQueryable<TeamProject> All { get; }
        IQueryable<TeamProject> AllIncluding(params Expression<Func<TeamProject, object>>[] includeProperties);
        TeamProject Find(int id);
        void InsertOrUpdate(TeamProject teamproject);
        void Delete(int id);
        void Save();
    }
}
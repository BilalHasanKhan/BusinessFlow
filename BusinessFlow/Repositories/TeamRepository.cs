using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BusinessFlow.Models
{
    public class TeamRepository : ITeamRepository
    {
        BusinessFlowContext context = new BusinessFlowContext();

        public IQueryable<Team> All
        {
            get { return context.Teams; }
        }

        public IQueryable<Team> AllIncluding(params Expression<Func<Team, object>>[] includeProperties)
        {
            IQueryable<Team> query = context.Teams;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Team Find(int id)
        {
            return context.Teams.Find(id);
        }
        public Team FindByName(string teamName)
        {
            return context.Teams.SingleOrDefault(s => s.TeamName.Equals(teamName));

        }
        public void InsertOrUpdate(Team team)
        {
            if (team.TeamID == default(int))
            {
                // New entity
                context.Teams.Add(team);
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

    public interface ITeamRepository
    {
        IQueryable<Team> All { get; }
        IQueryable<Team> AllIncluding(params Expression<Func<Team, object>>[] includeProperties);
        Team Find(int id);
        Team FindByName(string teamName);
        void InsertOrUpdate(Team team);
        void Delete(int id);
        void Save();
    }
}
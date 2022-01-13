using System.Collections.Generic;
using System.Linq;
using GreenOnion.Server.DataLayer.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace GreenOnion.Server.DataLayer.DataAccess
{
    public class ProjectMemberDataAccess
    {

        private readonly GreenOnionContext _context;

        public ProjectMemberDataAccess(GreenOnionContext context)
        {
            this._context = context;
        }

        // SELECT
        // retrieves & returns all projectMem from db related to the particular project by its id
        // used to get project members
        public IEnumerable<ProjectMember> SelectAllByProjectId(string id)
        {
            return _context.Project_member
                .Where(memb => memb.projectId == id)
                .Select(memb => memb);
        }

        // SELECT
        // retrieves & returns all projectMem object from db related to the particular user by its id
        public List<ProjectMember> SelectAllByUserId(string id)
        {
            return _context.Project_member
                .Where(memb => memb.userId == id)
                .Select(memb => memb).ToList();
        }

        // INSERT
        // adds new project_member associated with project and member into the db
        public void Insert(ProjectMember projectMember)
        {
            _context.Project_member.Add(projectMember);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        // DELETE
        // deletes project_member rows associated with project id from the db
        public void DeleteColumn(string projectId)
        {
            var projectMembers = _context.Project_member
                .Where(proj_m => proj_m.projectId == projectId)
                .Select(proj_m => proj_m).ToList();

            foreach (var projMem in projectMembers)
            {
                _context.Project_member.Remove(projMem);
            }

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
    }
}

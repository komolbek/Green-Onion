using System.Collections.Generic;
using System.Linq;
using GreenOnion.Server.DataLayer.DomainModels;

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
        // retrieves & returns all users(members) from db related to the particular project by its id
        public IEnumerable<ProjectMember> SelectAllByProjectId(string id)
        {
            return _context.Project_member
                .Where(memb => memb.projectId == id)
                .Select(memb => memb);
        }
    }
}

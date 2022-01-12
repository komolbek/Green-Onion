using System.Collections.Generic;
using System.Linq;
using GreenOnion.Server.DataLayer.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenOnion.Server.DataLayer.DataAccess
{
    public class ProjectDataAccess
    {
        private readonly GreenOnionContext _context;

        public ProjectDataAccess(GreenOnionContext context)
        {
            this._context = context;
        }

        // SELECT
        // retrieves & returns all projects related to particular company by its id
        public List<Project> SelectAllByCompanyId(string id)
        {
            return _context.Project
                .Where(proj => proj.companyId == id)
                .Select(proj => proj).ToList();
        }
    }
}


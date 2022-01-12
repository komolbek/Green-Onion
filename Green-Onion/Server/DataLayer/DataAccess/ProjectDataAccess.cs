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

        // INSERT
        // adds project to db
        public Project Insert(Project project)
        {
            _context.Project.Add(project);

            try
            {
                _context.SaveChanges();

                return Select(project.projectId);
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        // SELECT
        // retrieves & returns all projects related to particular company by its id
        public List<Project> SelectAllByCompanyId(string id)
        {
            return _context.Project
                .Where(proj => proj.companyId == id)
                .Select(proj => proj).ToList();
        }

        // SELECT
        // retrieves & returns all projects from db
        public IEnumerable<Project> SelectAll()
        {
            return _context.Project.ToList();
        }

        // SELECT
        // retrieves project by his id
        public Project Select(string id)
        {
            return _context.Project.FirstOrDefault(proj => proj.projectId == id);
        }

        // UPDATE
        // updates project information
        public ActionResult<Project> Update(string id, Project project)
        {
            _context.Entry(project).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();

                return Select(project.projectId);
            }
            catch (DbUpdateException)
            {
                if (!ProjectExists(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool ProjectExists(string id)
        {
            return _context.Project.Any(e => e.projectId == id);
        }
    }
}


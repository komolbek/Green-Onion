using GreenOnion.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using GreenOnion.Server.DataLayer.DataAccess;
using System.Linq;
using System.Collections.Generic;

namespace GreenOnion.Server.Controllers
{
    [Route("api/CompaniesController")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {

        private readonly CompanyDbContext _context;

        public CompaniesController(CompanyDbContext context)
        {
            this._context = context;
        }

        // Create company
        // POST: api/Company
        [HttpPost]
        public async Task<ActionResult<Company>> CreateCompany(Company company)
        {
            _context.companies.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompanyById), new { id = company.CompanyID }, company);
        }

        // Add project into the context
        // PUT: api/Company
        [HttpPut("{id}")]
        public async Task<ActionResult<Company>> AddProject(string companyID, Project project)
        {
            Company copmpany = await _context.companies.FindAsync(companyID);
            copmpany.Projects.Add(project);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(companyID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Get projects of company by its id
        // GET: api/Company
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Project>>> GetProjects(string companyID)
        {
            Company company = await _context.companies.FindAsync(companyID);

            return company.Projects;
        }

        // Delete company by id
        // DELETE: api/Company
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCompany(string companyID)
        {
            Company company = await _context.companies.FindAsync(companyID);
            _context.companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Remove project from company
        // UPDATE: api/Company
        [HttpPut("{companyId/projectId}")]
        public async Task<ActionResult<Company>> RemoveProject(string copmanyId, string projectId)
        {
            // FIXME: implemeted deleting Project entity from DB as well. Now it's just removing it from list I guess.

            Company company = await _context.companies.FindAsync(copmanyId);
            Project project = company.Projects.Find(proj => proj.ProjectID == projectId);

            company.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return company;
        }

        // Change company data by its id
        // DELETE: api/Company
        [HttpDelete("{id}")]
        public async Task<ActionResult> ChangeCompany(string companyID, Company newCompany)
        {
            if (companyID != newCompany.CompanyID)
            {
                return BadRequest();
            }

            _context.Entry(newCompany).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(companyID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Get company by id
        // GET: api/Company
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompanyById(string companyID) => await _context.companies.FindAsync(companyID);


        private bool CompanyExists(string id)
        {
            return _context.companies.Any(e => e.CompanyID == id);
        }
    }
}

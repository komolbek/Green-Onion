using System.Collections.Generic;
using System.Linq;
using GreenOnion.Server.DataLayer.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace GreenOnion.Server.DataLayer.DataAccess
{
    public class CompanyEmployeeDataAccess
    {

        private readonly GreenOnionContext _context;

        public CompanyEmployeeDataAccess(GreenOnionContext context)
        {
            this._context = context;
        }

        // SELECT
        // retrieves & returns all users(employees) from db related to the particular company by its id
        public IEnumerable<CompanyEmployee> SelectAllByCompanyId(string id)
        {
            return _context.Company_employee
                .Where(empl => empl.companyId == id)
                .Select(empl => empl);
        }

        // SELECT
        // retrieves & returns all companies from db related to the particular user by his id
        public List<string> SelectAllByUserId(string id)
        {
            
            return (List<string>)_context.Company_employee
                .Where(comp => comp.userId == id)
                .Select(comp => new { comp.companyId });
        }

        // INSERT
        // adds new company_employee associated with company and user into the db
        public void Insert(CompanyEmployee companyEmployee)
        {
            _context.Company_employee.Add(companyEmployee);

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

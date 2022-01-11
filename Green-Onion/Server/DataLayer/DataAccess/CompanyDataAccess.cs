using System.Collections.Generic;
using System.Linq;
using GreenOnion.Server.DataLayer.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenOnion.Server.DataLayer.DataAccess
{
    public class CompanyDataAccess
    {
        private readonly GreenOnionContext _context;

        public CompanyDataAccess(GreenOnionContext context)
        {
            this._context = context;
        }

        // INSERT
        // adds company to db
        public void Insert(Company company)
        {
            _context.Company.Add(company);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        // SELECT
        // retrieves user by his id
        public Company Select(string id)
        {
            return _context.Company.FirstOrDefault(company => company.companyId == id);
        }

        // SELECT
        // retrieves & returns all companues from db
        public IEnumerable<Company> SelectAll()
        {
            return _context.Company.ToList();
        }

        // UPDATE
        // updates user information
        public ActionResult<Company> Update(string id, Company company)
        {
            _context.Entry(company).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();

                return Select(company.companyId);
            }
            catch (DbUpdateException)
            {
                if (!UserExists(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.userId == id);
        }
    }
}

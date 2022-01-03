using System;
using GreenOnion.DomainModels;

namespace GreenOnion.DataMappers
{
    public class CompanyDataMapper
    {
        public CompanyDataMapper()
        {
        }

        public bool Insert(Company company)
        {
            return true;
        }

        public Company Select(string companyID)
        {
            return new Company();
        }

        public bool Update(Company company)
        {
            return true;
        }

        public bool Delete(string companyID)
        {
            return true;
        }
    }
}

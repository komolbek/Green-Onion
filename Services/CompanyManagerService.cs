using System;
using GreenOnion.DomainModels;
using GreenOnion.DataMappers;
using System.Collections.Generic;

namespace GreenOnion.Services
{
    public class CompanyManagerService
    {

        private CompanyDataMapper companyDataMapper;

        public CompanyManagerService()
        {
            this.companyDataMapper = new CompanyDataMapper()
        }

        public bool CreateCompany()
        {
            return true;
        }

        public bool AddProject()
        {
            return true
        }

        public List<Project> GetProjects()
        {
            return new List<Project>();
        }

        public bool DeleteCompany()
        {
            return true;
        }

        public bool ChangeCompany()
        {
            return true;
        }

        public Company GetCompany()
        {
            return new Company();
        }
    }
}

using System;
using GreenOnion.DomainModels;
using GreenOnion.DataMappers;
using System.Collections.Generic;

namespace GreenOnion.Services
{
    public class CompanyManagerService
    {

        private CompanyDataMapper companyDataMapper;
        private ProjectManagerService projectManagerService;

        public CompanyManagerService()
        {
            this.companyDataMapper = new CompanyDataMapper();
            this.projectManagerService = new ProjectManagerService();
        }

        public bool CreateCompany(string name, string aboutInfo)
        {
            Company company = new Company();
            company.Name = name;
            company.AboutInfo = aboutInfo;

            return this.companyDataMapper.Insert(company);
        }

        public bool AddProject(string companyID, string projectID)
        {
            Project project = this.projectManagerService.GetProject(projectID);
            Company company = this.companyDataMapper.Select(companyID);

            company.Projects.Add(project);

            return this.companyDataMapper.Update(company);
        }

        public List<Project> GetProjects(string companyID) => companyDataMapper.Select(companyID).Projects;

        public bool DeleteCompany(string companyID) => this.companyDataMapper.Delete(companyID);

        public bool ChangeCompany(string companyID, string name, string aboutInfo)
        {
            Company company = this.companyDataMapper.Select(companyID);
            company.Name = name;
            company.AboutInfo = aboutInfo;

            return this.companyDataMapper.Update(company);
        }

        public Company GetCompany(string companyID) => this.companyDataMapper.Select(companyID);
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenOnion.Server.DataLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenOnion.Server.DataLayer.DataMappers;
using GreenOnion.Server.DataLayer.DomainModels;
using GreenOnion.Server.Enums;
using GreenOnion.Server.DataLayer.DataAccess;

namespace Green_Onion.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {

        private readonly CompanyDataAccess _companyData;
        private readonly CompanyEmployeeDataAccess _companyEmployeeData;
        private readonly UserDataAccess _userData;
        private readonly ProjectDataAccess _projectData;

        public CompanyController(
            CompanyDataAccess companyData,
            CompanyEmployeeDataAccess companyEmployeeData,
            UserDataAccess userData,
            ProjectDataAccess projectData)
        {
            _companyData = companyData;
            _companyEmployeeData = companyEmployeeData;
            _userData = userData;
            _projectData = projectData;
        }


        // GET: api/Company
        [HttpGet]
        public IEnumerable<Company> GetCompanies()
        {
            return _companyData.SelectAll();
        }

        // GET: api/Company/getById/5
        [Route("getById/{id}")]
        [HttpGet]
        public ActionResult<CompanyDto> GetCompanyById(string id)
        {
            var company = _companyData.Select(id);
            var projects = GetCompanyProjects(id);
            var employees = GetCompanyEmployees(id);
            var creator = UserDataMapper.MapEntityToDto(_userData.Select(_companyData.Select(id).userId));

            if (company == null)
            {
                return NotFound();
            }

            return CompanyDataMapper.MapEntityToDto(company, projects, employees, creator);
        }

        private List<ProjectDto> GetCompanyProjects(string id)
        {
            var projectEntities = _projectData.SelectAllByCompanyId(id);

            var projectDtos = new List<ProjectDto>();

            foreach (var projEntity in projectEntities)
            {
                projectDtos.Add(ProjectDataMapper.MapEntityToDto(projEntity));
            }

            return projectDtos;
        }

        private List<UserDto> GetCompanyEmployees(string id)
        {
            var companyEmployeeEntities = _companyEmployeeData.SelectAllByCompanyId(id);

            var userDtos = new List<UserDto>(); // employees

            foreach (var companyEmployeeEntity in companyEmployeeEntities)
            {
                userDtos.Add(UserDataMapper.MapEntityToDto(_userData.Select(companyEmployeeEntity.userId)));
            }

            return userDtos;
        }

        // Changes company data
        // PUT: api/Company/changeById/5
        [Route("changeById/{id}")]
        [HttpPut]
        public ActionResult<CompanyDto> ChangeCompany(string id, CompanyDto companyDto)
        {         
            if (id != companyDto.companyId && companyDto.creator.userId is null)
            {
                return BadRequest();
            }

            // saving Dto data to use when mapping back
            var projects = companyDto.projects;
            var employees = companyDto.employees;
            var creator = companyDto.creator;

            Company companyEntity = CompanyDataMapper.MapDtoToEntity(companyDto);

            try
            {
                return CompanyDataMapper
                    .MapEntityToDto(_companyData.Update(id, companyEntity), projects, employees, creator);

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        // POST: api/Company
        [HttpPost]
        public ActionResult<CompanyDto> PostCompany(Company company)
        {
            CompanyEmployee companyEmployee = new CompanyEmployee()
            {
                companyId = company.companyId,
                userId = company.userId
            };

            try
            {
                var companyEntity = _companyData.Insert(company);

                _companyEmployeeData.Insert(companyEmployee);

                var creator = UserDataMapper.MapEntityToDto(_userData.Select(companyEntity.userId));

                return CompanyDataMapper.MapEntityToDto(company, creator);
            }
            catch (DbUpdateException)
            {
                throw;                
            }            
        }

        // Add employee into the company
        // PUT: api/Company
        [Route("addEmployee/userId/{userId}")]
        [HttpPut]
        public ActionResult<CompanyDto> AddEmployee(string userId, CompanyDto company)
        {
            CompanyEmployee companyEmployee = new CompanyEmployee()
            {
                companyId = company.companyId,
                userId = userId
            };

            _companyEmployeeData.Insert(companyEmployee);

            var userEntity = _userData.Select(userId);

            if (userEntity is not null)
            {
                company.employees.Add(UserDataMapper.MapEntityToDto(userEntity));
            } else
            {
                return NotFound();
            }
            

            return company;
        }               

        // Get current user associated companies by his userId.
        // Returns mainly a company names because companies will be used for list.
        // When selected specific company, companyDto with all data will be returned.
        // GET: api/Company
        [Route("getCompaniesByUserId/{userId}")]
        [HttpGet]
        public ActionResult<IEnumerable<CompanyDto>> GetCompaniesByUserId(string userId)
        {
            var companyEmployeeEntities = _companyEmployeeData.SelectAllByUserId(userId);

            var companies = new List<CompanyDto>();

            foreach (var companyEmployeeEntity in companyEmployeeEntities)
            {
                companies.Add(CompanyDataMapper.MapEntityToDto(
                    _companyData.Select(companyEmployeeEntity.companyId),
                    UserDataMapper.MapEntityToDto(_userData.Select(companyEmployeeEntity.userId))));
            }

            return companies;
        }
    }
}

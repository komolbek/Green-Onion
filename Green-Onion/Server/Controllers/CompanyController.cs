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
        private readonly PredictionService predictionService;
        private readonly CompanyEmployeeDataAccess _companyEmployeeData;
        private readonly UserDataAccess _userData;

        public CompanyController(
            CompanyDataAccess companyData,
            CompanyEmployeeDataAccess companyEmployeeData,
            UserDataAccess userData)
        {
            _companyData = companyData;
            //this.predictionService = new PredictionService();
            _companyEmployeeData = companyEmployeeData;
            _userData = userData;
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
        public ActionResult<Company> GetCompanyById(string id)
        {
            var company = _companyData.Select(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // Changes company data
        // PUT: api/Company/changeById/5
        [Route("changeById/{id}")]
        [HttpPut]
        public ActionResult<CompanyDto> ChangeCompany(string id, CompanyDto companyDto)
        {         
            if (id != companyDto.companyId)
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

            company.employees.Add(UserDataMapper.MapEntityToDto(userEntity));

            return company;

        }       

        // Removes project from company. Deletes project from DB. Call this api to delete project.
        // PUT: api/Company
        [Route("removeProject/{projectId}/fromCompany/{companyId}")]
        [HttpPut]
        

        // Get current user associated companies by his userId.
        // Returns mainly a company names because companies will be used for list.
        // When selected specific company, companyDto with all data will be returned.
        // GET: api/Company
        [Route("getCompaniesByUserId/{userId}")]
        [HttpGet]
        public ActionResult<IEnumerable<Company>> GetCompaniesByUserId(string userId)
        {
            var companyEntityIds = _companyEmployeeData.SelectAllByUserId(userId);
            var companies = new List<Company>();

            foreach (var companyEnitityId in companyEntityIds)
            {
                companies.Add(_companyData.Select(companyEnitityId));
            }

            return companies;
        }
    }
}

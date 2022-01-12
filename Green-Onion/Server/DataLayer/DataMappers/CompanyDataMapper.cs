using GreenOnion.Server.DataLayer.DomainModels;
using GreenOnion.Server.DataLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GreenOnion.Server.DataLayer.DataMappers
{
    // Class used for mapping Company related objects from Request, DTO model to Entity and vice versa.
    public class CompanyDataMapper
   {

        public CompanyDataMapper()
        {
        }

        public static Company MapDtoToEntity(CompanyDto companyDto)
        {
            Company company = new Company();
            company.companyId = companyDto.companyId;
            company.name = companyDto.name;
            company.userId = companyDto.creator.userId;
            company.aboutInfo = companyDto.aboutInfo;

            return company;
        }

        public static CompanyDto MapEntityToDto(Company companyEntity)
        {
            CompanyDto companyDto = new CompanyDto()
            {
                companyId = companyEntity.companyId,
                name = companyEntity.name,
                aboutInfo = companyEntity.aboutInfo
            };

            return companyDto;
        }

        public static CompanyDto MapEntityToDto(
            ActionResult<Company> companyEntity,
            List<ProjectDto> projects,
            List<UserDto> employees,
            UserDto creator)
        {
            CompanyDto companyDto = new CompanyDto()
            {
                companyId = companyEntity.Value.companyId,
                name = companyEntity.Value.name,
                creator = creator,
                aboutInfo = companyEntity.Value.aboutInfo,
                projects = projects,
                employees = employees
            };
            
            return companyDto;
        }

        public static CompanyDto MapEntityToDto(Company companyEntity, UserDto creator)
        {
            CompanyDto companyDto = new CompanyDto()
            {
                companyId = companyEntity.companyId,
                name = companyEntity.name,
                creator = creator,
                aboutInfo = null,
                projects = null,
                employees = null
            };

            return companyDto;
        }
    }
}

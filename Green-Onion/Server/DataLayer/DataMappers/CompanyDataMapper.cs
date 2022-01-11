using GreenOnion.Server.DataLayer.DomainModels;
using GreenOnion.Server.DataLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using GreenOnion.Server.DataLayer.DataAccess;

namespace GreenOnion.Server.DataLayer.DataMappers
{
    // Class used for mapping Company related objects from Request, DTO model to Entity and vice versa.
    public class CompanyDataMapper
    {

        private readonly UserDataAccess _userDataAccess;

        public CompanyDataMapper(UserDataAccess userDataAccess)
        {
            _userDataAccess = userDataAccess;
        }

        public Company MapDtoToEntity(CompanyDto companyDto)
        {
            Company company = new Company()
            {
                companyId = companyDto.companyId,
                name = companyDto.name,
                userId = companyDto.creator.userId,
                aboutInfo = companyDto.aboutInfo
            };

            return company;
        }

        public CompanyDto MapEntityToDto(ActionResult<Company> companyEntity)
        {
            CompanyDto companyDto = new CompanyDto()
            {
                companyId = companyEntity.Value.companyId,
                name = companyEntity.Value.name,
                creator = UserDataMapper.MapEntityToDto(_userDataAccess.Select(companyEntity.Value.userId)),
                aboutInfo = companyEntity.Value.aboutInfo
            };
            
            return new CompanyDto();
        }
    }
}

using System.Collections.Generic;
using GreenOnion.Server.DataLayer.DomainModels;
using GreenOnion.Server.DataLayer.DTOs;

namespace GreenOnion.Server.DataLayer.DataMappers
{
    public class ProjectDataMapper
    {
        public ProjectDataMapper()
        {
        }

        // Mapped for list view
        public static ProjectDto MapEntityToDto(Project projectEntity)
        {
            ProjectDto projectDto = new ProjectDto()
            {
                projectId = projectEntity.projectId,
                name = projectEntity.name,
                startedDate = projectEntity.startedDate,
                closedDate = projectEntity.closedDate,
                dueDate = projectEntity.dueDate
            };

            return projectDto;
        }

        public static ProjectDto MapEntityToDto(Project projectEntity, UserDto userDto, CompanyDto companyDto, List<UserDto> members)
        {
            ProjectDto projectDto = new ProjectDto()
            {
                projectId = projectEntity.projectId,
                name = projectEntity.name,
                startedDate = projectEntity.startedDate,
                closedDate = projectEntity.closedDate,
                dueDate = projectEntity.dueDate,
                creator = userDto,
                company = companyDto,
                members = members
            };

            return projectDto;
        }
    }
}

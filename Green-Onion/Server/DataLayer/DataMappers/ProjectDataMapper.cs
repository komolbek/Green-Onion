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

        // Mapper used when new project with empty data is created
        public static ProjectDto MapEntityToDto(
            Project projectEntity,
            UserDto creator = null,
            CompanyDto companyDto = null,
            List<UserDto> members = null)
        {
            ProjectDto projectDto = new ProjectDto()
            {
                projectId = projectEntity.projectId,
                name = projectEntity.name,
                creator = creator,
                company = companyDto,
                startedDate = projectEntity.startedDate,
                members = members
            };

            return projectDto;
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

        public static ProjectDto MapEntityToDto(
            Microsoft.AspNetCore.Mvc.ActionResult<Project> projectEntity,
            UserDto userDto,
            CompanyDto companyDto,
            List<UserDto> members,
            List<TicketDto> tickets)
        {
            ProjectDto projectDto = new ProjectDto()
            {
                projectId = projectEntity.Value.projectId,
                name = projectEntity.Value.name,
                startedDate = projectEntity.Value.startedDate,
                closedDate = projectEntity.Value.closedDate,
                dueDate = projectEntity.Value.dueDate,
                creator = userDto,
                company = companyDto,
                members = members,
                tickets = tickets
            };

            return projectDto;
        }

        public static Project MapDtoToEntity(ProjectDto projectDto)
        {
            return new Project()
            {
                projectId = projectDto.projectId,
                companyId = projectDto.company.companyId,
                userId = projectDto.creator.userId,
                name = projectDto.name,
                startedDate = projectDto.startedDate,
                closedDate = projectDto.closedDate,
                dueDate = projectDto.dueDate
            };
        }
    }
}

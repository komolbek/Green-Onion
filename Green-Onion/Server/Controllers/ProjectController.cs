using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenOnion.Server.DataLayer.DataAccess;
using GreenOnion.Server.DataLayer.DomainModels;
using GreenOnion.Server.DataLayer.DTOs;
using GreenOnion.Server;
using GreenOnion.Server.DataLayer.DataMappers;

namespace Green_Onion.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly PredictionService _predictionService;
        private readonly ProjectDataAccess _projectData;
        private readonly ProjectMemberDataAccess _projectMemberData;
        private readonly UserDataAccess _userData;
        private readonly CompanyDataAccess _companyData;
        private readonly TicketDataAccess _ticketData;

        public ProjectController(
            ProjectDataAccess projectData,
            PredictionService predictionService,
            ProjectMemberDataAccess projectMemberData,
            UserDataAccess userData,
            CompanyDataAccess companyData,
            TicketDataAccess ticketData)
        {
            _projectData = projectData;
            _predictionService = predictionService;
            _projectMemberData = projectMemberData;
            _userData = userData;
            _companyData = companyData;
            _ticketData = ticketData;
        }

        // GET: api/Project
        [HttpGet]
        public IEnumerable<Project> GetProjects()
        {
            return _projectData.SelectAll();
        }

        // GET: api/Project/getById/5
        [Route("getById/{id}")]
        [HttpGet]
        public ActionResult<ProjectDto> GetProject(string id)
        {
            var projectEntity = _projectData.Select(id);
            var company = CompanyDataMapper.MapEntityToDto(_companyData.Select(_projectData.Select(id).companyId));
            var tickets = GetProjectTickets(id);
            var members = GetProjectMembers(id);
            var creator = UserDataMapper.MapEntityToDto(_userData.Select(_projectData.Select(id).userId));

            if (company == null)
            {
                return NotFound();
            }

            return ProjectDataMapper.MapEntityToDto(projectEntity, creator, company, members, tickets);
        }

        private List<UserDto> GetProjectMembers(string id)
        {
            var projectMemberEntities = _projectMemberData.SelectAllByProjectId(id);

            var userDtos = new List<UserDto>(); // employees

            foreach (var projectMemberEntity in projectMemberEntities)
            {
                userDtos.Add(UserDataMapper.MapEntityToDto(_userData.Select(projectMemberEntity.userId)));
            }

            return userDtos;
        }

        private List<TicketDto> GetProjectTickets(string id)
        {
            var ticketEntities = _ticketData.SelectAllByProjectId(id);

            var ticketDtos = new List<TicketDto>();

            foreach (var tickEntity in ticketEntities)
            {
                ticketDtos.Add(TicketDataMapper.MapEntityToDto(tickEntity));
            }

            return ticketDtos;
        }

        // Change project's data only
        // PUT: api/Project/changeById/5
        [Route("changeById/{id}")]
        [HttpPut]
        public ActionResult<ProjectDto> PutProject(string id, ProjectDto projectDto)
        {
            if(id != projectDto.projectId && projectDto.creator.userId is null)
            {
                return BadRequest();
            }

            // saving Dto data to use when mapping back
            var tickets = projectDto.tickets;
            var members = projectDto.members;
            var creator = projectDto.creator;
            var company = projectDto.company;

            Project projectEntity = ProjectDataMapper.MapDtoToEntity(projectDto);

            try
            {
                return ProjectDataMapper.MapEntityToDto(_projectData.Update(id, projectEntity), creator, company, members, tickets);

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        // POST: api/Project
        [HttpPost]
        public ActionResult<ProjectDto> PostProject(Project project)
        {
            ProjectMember projectMember = new ProjectMember()
            {
                projectId = project.projectId,
                userId = project.userId
            };

            try
            {
                var creator = UserDataMapper.MapEntityToDto(_userData.Select(project.userId));
                var company = CompanyDataMapper.MapEntityToDto(_companyData.Select(project.companyId));
                var members = new List<UserDto>() { creator };

                if (creator is not null && company is not null)
                {
                    var projectEntity = _projectData.Insert(project);

                    _projectMemberData.Insert(projectMember);
                    return ProjectDataMapper.MapEntityToDto(project, creator, company, members);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        // Used if need to remove project from Company
        // DELETE: api/Project/deleteById/5
        [Route("deleteById/{id}")]
        [HttpDelete]
        public string DeleteProject(string id)
        {
            // delete project and member relation data
            _projectMemberData.DeleteColumn(id);
            // delete project's tickets plus their assignees
            _ticketData.DeleteByColumnProjId(id);
            _projectData.Delete(id);

            if (_projectData.Select(id) is null)
            {
                return "deleted sucessfully";
            } else
            {
                return "could not delete";
            }

            
        }

        // GET: api/Project
        //[Route("projectDuration/{projectId}/byTicketComplexity")]
        //[HttpGet]
        //public ActionResult<string> GetDurationByTicketComplexity(string projectId)
        //{
        //    return predictionService.CalculateDurationByTicketComplexity(projectId, _context);
        //}

        //        // GET: api/Project
        //        [Route("projectDuration/projectId/{projectId}/byCompanyData/companyId/{companyId}")]
        //        [HttpGet]
        //        public async Task<ActionResult<string>> GetDurationByHistoricalData(string projectId, string companyId)
        //        {
        //            return await predictionService.CalculateDurationByHistoricalData(projectId, companyId, _context);
        //        }

        // Get projects by company id
        // GET: api/projects
        [Route("getByCompanyId/{companyId}")]
        [HttpGet]
        public ActionResult<List<ProjectDto>> GetProjectsByCompanyId(string companyId)
        {
            var projectEntities = _projectData.SelectAllByCompanyId(companyId);

            return GetProjectsBy(projectEntities);
        }

        // Get projects by user id
        // GET: api/projects
        [Route("getByUserId/{userId}")]
        [HttpGet]
        public ActionResult<List<ProjectDto>> GetProjectsByUserId(string userId)
        {
            List<ProjectMember> projectMembers = _projectMemberData.SelectAllByUserId(userId);
            List<Project> projectEntities = new List<Project>();
            
            foreach (var projectMember in projectMembers)
            {
                projectEntities.Add(_projectData.Select(projectMember.projectId));
            }

            return GetProjectsBy(projectEntities);
        }

        // reusable
        private List<ProjectDto> GetProjectsBy(List<Project> projectEntities) 
        {
            List<ProjectDto> projectDtos= new List<ProjectDto>();

            foreach (var project in projectEntities)
            {
                // data for projectDto
                var company = CompanyDataMapper.MapEntityToDto(_companyData.Select(_projectData.Select(project.projectId).companyId));
                var tickets = GetProjectTickets(project.projectId);
                var members = GetProjectMembers(project.projectId);
                var creator = UserDataMapper.MapEntityToDto(_userData.Select(_projectData.Select(project.projectId).userId));

                projectDtos.Add(ProjectDataMapper.MapEntityToDto(project, creator, company, members, tickets));
            }

            return projectDtos;
        }

        // Add memeber to the project. Gets User & Project from DB by IDs, adds User to Project and saves Project records in the DB
        // PUT: api/projects
        [Route("addMember/{userId}/toProject/{projectId}")]
        [HttpPut]
        public ActionResult<ProjectDto> AddMember(string userId, string projectId)
        {
            if (projectId is null | userId is null)
            {
                return BadRequest();
            }

            ProjectMember projectMember = new()
            {
                projectId = projectId,
                userId = userId
            };

            _projectMemberData.Insert(projectMember);

            User userEntity = _userData.Select(userId);
            UserDto userDto = UserDataMapper.MapEntityToDto(userEntity);

            return GetProject(projectId);
        }

        // Get project members. Members who have assigned Ticket from selected project
        // GET: api/projects
        [Route("getMembersByProjectId/{projectId}")]
        [HttpGet]
        public ActionResult<List<UserDto>> GetMembers(string projectId)
        {          
            return GetMembers(projectId);
        }

        // Get project tickets
        // GET: api/projects
        [Route("getTicketsByProjectId/{projectId}")]
        [HttpGet("{projectId}")]
        public ActionResult<List<Ticket>> GetTickets(string projectId)
        {
            return GetTickets(projectId);
        }


        // Moves ticket in project list by changing ticket status in selected project & updates DB.
        // PUT: api/project
        [Route("moveTicket/{ticketId}")]
        [HttpPut]
        public ActionResult<Dictionary<string, List<TicketDto>>> MoveTicket(string ticketId, Ticket newTicket, string oldTicketStatus)
        {

            ActionResult<Ticket> ticketEntity = _ticketData.Update(ticketId, newTicket);
            List<TicketDto> ticketDtos = GetProjectTickets(ticketEntity.Value.projectId);

            return FilterProjectTicketsByStatus(ticketDtos);
        }

        // Returns filtered Tickets by status.
        private Dictionary<string, List<TicketDto>> FilterProjectTicketsByStatus(List<TicketDto> ticketDtos)
        {
            // Filtered tickets by lists like To do, Doing & Done.
            Dictionary<string, List<TicketDto>> filteredTickets= new Dictionary<string, List<TicketDto>>();

            foreach (TicketDto ticketDto in ticketDtos)
            {
                if (ticketDto.status == "todo")
                {
                    filteredTickets["todo"].Add(ticketDto);
                }
                else if (ticketDto.status == "doing")
                {
                    filteredTickets["doing"].Add(ticketDto);
                }
                else
                {
                    filteredTickets["done"].Add(ticketDto);
                }
            }

            return filteredTickets;
        }
    }
}

//GET: api / Prediction
//[Route("makepPrediction/projectId/{projectId}/companyId/{companyId}")]
//[HttpGet]
//public async Task<ActionResult<PredictionDto>> MakePrediction(string projectId, string companyId)
//{
//    PredictionDto prediction = new PredictionDto();

//    Company company = await _context.companies.FindAsync(companyId);

//    // project is used to make prediction for it by its tickets complexy
//    Project project = await _context.projects.FindAsync(projectId);
//    prediction.DurationByTicketComplexity = predictionService.CalculateDurationByTicketComplexity(projectId, _context).ToString();

//    prediction.NumOfMembers = predictionService.CalculateRequiredNum(PredictionType.Members.ToString(), company.Projects);
//    prediction.NumOfTickets = predictionService.CalculateRequiredNum(PredictionType.Tickets.ToString(), company.Projects);

//    prediction.DurationByHistoricalData = predictionService.CalculateDurationByHistoricalData(projectId, companyId, _context).ToString();

//    return prediction;
//}

/// Add project into the company
//        // PUT: api/Company
//        [Route("addProject/projectId/inCompany/{companyId}")]
//[HttpPut]
//public async Task<ActionResult<Company>> AddProject(string companyId, string projectId)
//{
//    Company company = await _context.companies.FindAsync(companyId);
//    Project project = await _context.projects.FindAsync(projectId);

//    if (company is not null && project is not null)
//    {
//        company.Projects.Add(project);

//        _context.Entry(company).State = EntityState.Modified;

//        await _context.SaveChangesAsync();

//        return company;
//    }
//    else
//    {
//        return null;
//    }
//}

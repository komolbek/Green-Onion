//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using GreenOnion.Server;
//using GreenOnion.Server.DataLayer.DomainModels;
//using System;

//namespace Green_Onion.Server.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ProjectController : ControllerBase
//    {
//        private readonly GreenOnionContext _context;
//        private readonly PredictionService predictionService;

//        public ProjectController(GreenOnionContext context)
//        {
//            _context = context;
//            this.predictionService = new PredictionService();
//        }

//        // GET: api/Project
//        [Route("all")]
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Project>>> Getprojects()
//        {
//            return await _context.projects.ToListAsync();
//        }

//        // GET: api/Project/5
//        [Route("getById/{id}")]
//        [HttpGet]
//        public async Task<ActionResult<Project>> GetProject(string id)
//        {
//            var project = await _context.projects.FindAsync(id);

//            if (project == null)
//            {
//                return NotFound();
//            }

//            return project;
//        }

//        // PUT: api/Project/5
//        [Route("changeById/{id}")]
//        [HttpPut]
//        public async Task<IActionResult> PutProject(string id, Project project)
//        {
//            if (id != project.projectId)
//            {
//                return BadRequest();
//            }

//            _context.Entry(project).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!ProjectExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // POST: api/Project
//        [HttpPost]
//        public async Task<ActionResult<Project>> PostProject(Project project)
//        {
//            _context.projects.Add(project);
//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateException)
//            {
//                if (ProjectExists(project.projectId))
//                {
//                    return Conflict();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return CreatedAtAction("GetProject", new { id = project.projectId }, project);
//        }

//        // DELETE: api/Project/5
//        [Route("deleteById/{id}")]
//        [HttpDelete]
//        public async Task<IActionResult> DeleteProject(string id)
//        {
//            var project = await _context.projects.FindAsync(id);
//            if (project == null)
//            {
//                return NotFound();
//            }

//            _context.projects.Remove(project);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool ProjectExists(string id)
//        {
//            return _context.projects.Any(e => e.projectId == id);
//        }

//        // GET: api/Project
//        [Route("projectDuration/{projectId}/byTicketComplexity")]
//        [HttpGet]
//        public async Task<ActionResult<string>> GetDurationByTicketComplexity(string projectId)
//        {
//            return await predictionService.CalculateDurationByTicketComplexity(projectId, _context);
//        }

//        // GET: api/Project
//        [Route("projectDuration/projectId/{projectId}/byCompanyData/companyId/{companyId}")]
//        [HttpGet]
//        public async Task<ActionResult<string>> GetDurationByHistoricalData(string projectId, string companyId)
//        {
//            return await predictionService.CalculateDurationByHistoricalData(projectId, companyId, _context);
//        }

//        // Get projects by company id
//        // GET: api/projects
//        [Route("getProjectByCompanyId/{companyId}")]
//        [HttpGet]
//        public async Task<ActionResult<List<Project>>> GetProjectsByCompanyId(string companyID)
//        {
//            Company company = await _context.companies.FindAsync(companyID);

//            return company.Projects;
//        }

//        // Get projects by user id
//        // GET: api/projects
//        [Route("getProjectsByUserId/{userId}")]
//        [HttpGet]
//        public async Task<ActionResult<List<Project>>> GetProjectsByUserId(string userId)
//        {
//            List<Project> projects = new List<Project>();

//            User user = await _context.users.FindAsync(userId);

//            await _context.projects.ForEachAsync(delegate (Project project)
//            {
//                if (project.Members.Contains(user))
//                {
//                    projects.Add(project);
//                }
//            });

//            return projects;
//        }

//        // Add memeber to the project. Gets User & Project from DB by IDs, adds User to Project and saves Project records in the DB
//        // PUT: api/projects
//        [Route("addMember/{userId}/toProject/{projectId}")]
//        [HttpPut]
//        public async Task<ActionResult<Project>> AddMember(string projId, string userId)
//        {
//            Project project = await _context.projects.FindAsync(projId);
//            User user = await _context.users.FindAsync(userId);


//            if (project is not null && user is not null)
//            {
//                project.Members.Add(user);

//                _context.Entry(project).State = EntityState.Modified;

//                await _context.SaveChangesAsync();

//                return project;
//            }
//            else
//            {
//                return null;
//            }

//            return project;
//        }

//        // Get project members. Members who have assigned Ticket from selected project
//        // GET: api/projects
//        [Route("getMembersByProjectId/{projectId}")]
//        [HttpGet]
//        public async Task<ActionResult<List<User>>> GetMembers(string projectId)
//        {
//            Project project = await _context.projects.FindAsync(projectId);

//            return project.Members;
//        }

//        // Get project tickets
//        // GET: api/projects
//        [Route("getTicetsByProjectId/{projectId}")]
//        [HttpGet("{projectId}")]
//        public async Task<ActionResult<List<Ticket>>> GetTickets(string projectId)
//        {
//            Project project = await _context.projects.FindAsync(projectId);

//            return project.Tickets;
//        }

//        // Gets Ticket & Project from DB by IDs, adds Ticket to Project and saves Project records in the DB
//        // PUT: api/projects
//        [Route("addTicketByProjectId/{projectId}")]
//        [HttpPut]
//        public async Task<ActionResult<Project>> AddTicket(string projId, Ticket ticket)
//        {
//            Project project = await _context.projects.FindAsync(projId);
//            project.Tickets.Add(ticket);

//            _context.tickets.Add(ticket);

//            await _context.SaveChangesAsync();

//            return project;
//        }

//        // Removes ticket from project. Also deletes ticket from DB. Call this api to delete ticket.
//        // UPDATE: api/projects
//        [Route("removeTicket/{ticketId}/inProject/{projectId}")]
//        [HttpPut]
//        public async Task<ActionResult<Project>> RemoveTicket(string projectId, string ticketId)
//        {
//            Project project = await _context.projects.FindAsync(projectId);
//            Ticket ticket = project.Tickets.Find(tick => tick.TicketId == ticketId);

//            project.Tickets.Remove(ticket);

//            _context.tickets.Remove(ticket);

//            await _context.SaveChangesAsync();

//            return project;
//        }

//        // Moves ticket in project list by changign ticket status in selected project & updates DB.
//        // PUT: api/project
//        [Route("moveTicket/{ticketId}/inProject/{projectId}")]
//        [HttpPut("{projectId}/{ticketId}")]
//        public async Task<ActionResult<Dictionary<string, List<Ticket>>>> MoveTicket(string projectId, string ticketId, string newTicketStatus, string oldTicketStatus)
//        {
//            Project project = await _context.projects.FindAsync(projectId);
//            Ticket ticket = project.Tickets.Find(_tickId => _tickId.TicketId == ticketId);

//            ticket.status = newTicketStatus;
//            await _context.SaveChangesAsync();

//            return FilterProjectTicketsByStatus(project);
//        }

//        // Returns filtered Tickets by status.
//        private Dictionary<string, List<Ticket>> FilterProjectTicketsByStatus(Project project)
//        {
//            Dictionary<string, List<Ticket>> filteredTicketsByProjectLists = new Dictionary<string, List<Ticket>>();

//            foreach (Ticket ticket in project.Tickets)
//            {
//                if (ticket.status == "todo")
//                {
//                    filteredTicketsByProjectLists["todo"].Add(ticket);
//                }
//                else if (ticket.status == "doing")
//                {
//                    filteredTicketsByProjectLists["doing"].Add(ticket);
//                }
//                else
//                {
//                    filteredTicketsByProjectLists["done"].Add(ticket);
//                }
//            }

//            return filteredTicketsByProjectLists;
//        }
//    }
//}

// GET: api/Prediction
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

// MEANS JUST DELETE PROJECT

//public ActionResult<Company> RemoveProject(string copmanyId, string projectId)
//{
//    Company company = _context.companies.Find(copmanyId);
//    Project project = company.Projects.Find(proj => proj.ProjectId == projectId);

//    RemoveProjectTicketsFromAssignees(project);
//    RemoveTicketFromProject(project);

//    company.Projects.Remove(project);

//    _context.projects.Remove(project);
//    _context.SaveChanges();

//    return company;
//}

//private void RemoveProjectTicketsFromAssignees(Project project)
//{
//    project.Members.ForEach(delegate (User member)
//    {
//        member.Tickets.ForEach(delegate (Ticket ticket)
//        {
//            if (ticket.projectId == project.projectId)
//            {
//                _ = member.Tickets.Remove(ticket);
//            }
//        });
//    });
//}

//private void RemoveTicketFromProject(Project project)
//{
//    project.Tickets.ForEach(delegate (Ticket ticket)
//    {
//        _context.tickets.Remove(ticket);
//    });
//}
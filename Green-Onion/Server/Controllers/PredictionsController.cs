using System.Collections.Generic;
using GreenOnion.DomainModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GreenOnion.Server.Enums;
using GreenOnion.Server.DataLayer.DataAccess;

namespace GreenOnion.Server.Controllers
{
    [Route("api/PredictionsController")]
    [ApiController]
    public class PredictionsController : ControllerBase
    {
        private string ticketsString = "tickets"; // const
        private string membersString = "members"; // const

        private readonly ProjectDbContext _projectContext;
        private readonly CompanyDbContext _companyContext;

        public PredictionsController(ProjectDbContext projectContext, CompanyDbContext companyContext)
        {
            this._projectContext = projectContext;
            this._companyContext = companyContext;
        }                    

        // Sums the number of tickets of each project. Devides the sum to the number of projects.
        // Returns an avarage as approximate number of required tickets for project.        
        private int CalculateRequiredNum(string predictingItem, List<Project> projects)
        {
            int predictionNum = 0;

            // predicting entity is either Ticket or Member depending on parameter passage.
            int sumOfPredictingEntity = 0;

            int projectCount = 0;

            projects.ForEach(delegate (Project project)
            {
                projectCount++;

                if (predictingItem == this.ticketsString)
                {
                    sumOfPredictingEntity += project.Tickets.Count;
                }
                else
                {
                    sumOfPredictingEntity += project.Members.Count;
                }

            });

            predictionNum = (sumOfPredictingEntity + projectCount * 2) / projectCount;

            return predictionNum;
        }

        // Todo: add documentation
        // GET: api/Prediction
        [HttpGet]
        [Route("{projectId}/{companyId}")]
        public async Task<ActionResult<PredictionDto>> MakePrediction(string projectId, string companyId)
        {
            PredictionDto prediction = new PredictionDto();

            Company company = await _companyContext.companies.FindAsync(companyId);

            // project is used to make prediction for it by its tickets complexy
            Project project = await _projectContext.projects.FindAsync(projectId);
            prediction.DurationByTicketComplexity = CalculateDurationByTicketComplexity(projectId).ToString();

            prediction.NumOfMembers = this.CalculateRequiredNum(this.membersString, company.Projects);
            prediction.NumOfTickets = this.CalculateRequiredNum(this.ticketsString, company.Projects);

            prediction.DurationByHistoricalData = CalculateDurationByHistoricalData(projectId, companyId).ToString();
            
            return prediction;
        }

        // Todo: add documentation
        // GET: api/Prediction
        [HttpGet]
        [Route("{projectId}")]
        public async Task<ActionResult<string>> GetDurationByTicketComplexity(string projectId)
        {
            return await this.CalculateDurationByTicketComplexity(projectId);
        }

        // Todo: add documentation
        public async Task<ActionResult<string>> CalculateDurationByTicketComplexity(string projectId)
        {
            int daysSum = 0;

            Project project = await _projectContext.projects.FindAsync(projectId);

            project.Tickets.ForEach(delegate (Ticket ticket)
            {
                if (ticket.Complexity == TicketComplexity.Easy.ToString())
                {
                    daysSum += 2;
                }
                else if (ticket.Complexity == TicketComplexity.Hard.ToString())
                {
                    daysSum += 4;
                }
                else
                {
                    daysSum += 7;
                }
            });

            return $"Predicted project duration by opened tickets complexity is {daysSum} days";
        }

        // Todo: add documentation
        // GET: api/Prediction
        [HttpGet]
        [Route("{projectId}/{companyId}")]
        public async Task<ActionResult<string>> GetDurationByHistoricalData(string projectId, string companyId)
        {
            return await this.CalculateDurationByHistoricalData(projectId, companyId);
        }

        // Todo: add documentation
        public async Task<ActionResult<string>> CalculateDurationByHistoricalData(string projectId, string companyId)
        {
            Company company = await this._companyContext.companies.FindAsync(companyId);

            int totalDays = 0;
            int predictedDays = 0;
            int totalProjects = company.Projects.Count;

            company.Projects.ForEach(delegate (Project project)
            {
                int projectDuration = (int)(project.ClosedDate - project.StartedDate).TotalDays + 2;
                totalDays += projectDuration;
            });

            predictedDays = totalDays / totalProjects;

            return $"Predicted project duration by company historical data is {predictedDays} days";
        }
    }
}

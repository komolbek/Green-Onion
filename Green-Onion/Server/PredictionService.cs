using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GreenOnion.Server.DataLayer.DomainModels;
using GreenOnion.Server.Enums;
using System.Collections.Generic;

namespace GreenOnion.Server
{
    public class PredictionService
    {
        public PredictionService()
        {
        }

        // Sums the number of tickets of each project. Devides the sum to the number of projects.
        // Returns an avarage as approximate number of required tickets for project.        
        public int CalculateRequiredNum(string predictingItem, List<Project> projects)
        {
            int predictionNum = 0;

            // predicting entity is either Ticket or Member depending on parameter passage.
            int sumOfPredictingEntity = 0;

            int projectCount = 0;

            projects.ForEach(delegate (Project project)
            {
                projectCount++;

                if (predictingItem == PredictionType.Tickets.ToString())
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

        public async Task<ActionResult<string>> CalculateDurationByHistoricalData(string projectId, string companyId, GreenOnionContext context)
        {
            Company company = await context.companies.FindAsync(companyId);

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

        public async Task<ActionResult<string>> CalculateDurationByTicketComplexity(string projectId, GreenOnionContext context)
        {
            int daysSum = 0;

            Project project = await context.projects.FindAsync(projectId);

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
    }
}

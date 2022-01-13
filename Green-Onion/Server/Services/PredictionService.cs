using Microsoft.AspNetCore.Mvc;
using GreenOnion.Server.DataLayer.DomainModels;
using GreenOnion.Server.Enums;
using System.Collections.Generic;
using GreenOnion.Server.DataLayer.DataAccess;
using System;

namespace GreenOnion.Server.Servers
{
    public class PredictionService
    {
        // TODO: reuse methods below.
        private readonly ProjectDataAccess _projectData;
        private readonly TicketDataAccess _ticketData;
        private readonly UserDataAccess _userData;
        private readonly ProjectMemberDataAccess _projectMemberData;

        public static string dateFormat = "dd/MM/yyyy";

        public PredictionService(
            TicketDataAccess ticketData,
            ProjectDataAccess projectData,
            UserDataAccess userData,
            ProjectMemberDataAccess projectMemberData)
        {
            _projectData = projectData;
            _ticketData = ticketData;
            _userData = userData;
            _projectMemberData = projectMemberData;
        }

        // Sums the number of tickets of each project. Devides the sum to the number of projects.
        // Returns an avarage as approximate number of required tickets for project.
        // <PredictingItem> param means the type of prediction: By ticket complexity or members
        public int CalculateRequiredNum(string predictingItem, string projectId)
        {
            int predictionNum = 0;

            // predicting entity is either Ticket or Member depending on parameter passage.
            int sumOfPredictingEntity = 0;

            int projectCount = 0;

            Project projectEntity = _projectData.Select(projectId);

            // projects of company
            List<Project> projectEntites = _projectData.SelectAllByCompanyId(projectEntity.companyId);
                  
            projectEntites.ForEach(delegate (Project project)
            {
                projectCount++;

                if (predictingItem == PredictionType.Tickets.ToString())
                {
                    // entity means sum of tickets
                    sumOfPredictingEntity += _ticketData.SelectAllByProjectId(projectId).Count;
                }
                else
                {
                    // entity means sum of member
                    sumOfPredictingEntity += GetProjectMembers(projectId).Count;
                }

            });

            predictionNum = (sumOfPredictingEntity + projectCount * 2) / projectCount;

            return predictionNum;
        }

        private List<User> GetProjectMembers(string id)
        {
            var projectMemberEntities = _projectMemberData.SelectAllByProjectId(id);

            var userEntites = new List<User>(); // employees

            foreach (var projectMemberEntity in projectMemberEntities)
            {
                userEntites.Add(_userData.Select(projectMemberEntity.userId));
            }

            return userEntites;
        }

        // By historical data

        public string CalculateDurationByHistoricalData(string projectId)
        {
            return $"Predicted project duration by company historical data is {DurationByHistoricalData(projectId)} days";
        }

        public string CalculateDueByHistoricalData(string projectId)
        {
            int days = DurationByHistoricalData(projectId);
            Project project = _projectData.Select(projectId);

            if (project is not null && project.startedDate is not null)
            {
                DateTime startedDate = DateTime.ParseExact(project.startedDate, PredictionService.dateFormat,
                                       System.Globalization.CultureInfo.InvariantCulture);

                return startedDate.AddDays(days).ToString("dd/MM/yyyy");
            } else
            {
                return null;
            }
            
        }       

        private int DurationByHistoricalData(string projectId)
        {

            Project project = _projectData.Select(projectId);
            List<Project> projectEntities = _projectData.SelectAllByCompanyId(project.companyId);

            int totalDays = 0;
            int predictedDays = 0;
            int totalProjects = projectEntities.Count;

            foreach (Project proj in projectEntities)
            {
                if (proj.closedDate is null)
                {
                    continue;
                }

                DateTime createdDate = DateTime.ParseExact(project.startedDate, PredictionService.dateFormat,
                                       System.Globalization.CultureInfo.InvariantCulture);

                DateTime closedDate = DateTime.ParseExact(project.closedDate, PredictionService.dateFormat,
                                       System.Globalization.CultureInfo.InvariantCulture);
                                
                int projectDuration = (int)(closedDate - createdDate).TotalDays + 2;
                totalDays += projectDuration;
            }

            predictedDays = totalDays / totalProjects;

            return predictedDays;
        }

        // By ticket complexity

        public string CalculateDurationByTicketComplexity(string projectId)
        {
            return $"Predicted project duration by opened tickets complexity is {DurationByTicketComplexity(projectId)} days";
        }

        public string CalculateDueByTicketComplexity(string projectId)
        {
            int days = DurationByTicketComplexity(projectId);
            Project project = _projectData.Select(projectId);

            if (project is not null && project.startedDate is not null)
            {
                DateTime startedDate = DateTime.ParseExact(project.startedDate, PredictionService.dateFormat,
                                       System.Globalization.CultureInfo.InvariantCulture);

                return startedDate.AddDays(days).ToString("dd/MM/yyyy");
            }
            else
            {
                return null;
            }

        }

        public int DurationByTicketComplexity(string projectId)
        {
            int daysSum = 0;

            Project project =  _projectData.Select(projectId);
            List<Ticket> ticketEntities = _ticketData.SelectAllByProjectId(projectId);

            ticketEntities.ForEach(delegate (Ticket ticket)
            {
                if (ticket.complexity == TicketComplexity.Easy.ToString())
                {
                    daysSum += 2;
                }
                else if (ticket.complexity == TicketComplexity.Hard.ToString())
                {
                    daysSum += 4;
                }
                else
                {
                    daysSum += 7;
                }
            });

            return daysSum;
        }
    }
}

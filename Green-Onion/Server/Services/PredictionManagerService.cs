using System;
using GreenOnion.DomainModels;
using System.Collections.Generic;
using GreenOnion.Enums;

namespace GreenOnion.Services
{
    public class PredictionManagerService
    {
        private Prediction prediction;
        private List<Project> projects;
        private string ticketsString = "tickets"; // const
        private string membersString = "members"; // const

        public PredictionManagerService(List<Project> projects)
        {
            this.projects = projects;
        }

        // Sums the number of tickets of each project. Devides the sum to the number of projects.
        // And returns an avarage as approximate number of required tickets for project.
        private int calculateRequiredNum(string predictingItem)
        {
            int predictionNum= 0;

            // predicting entity is either Ticket or Member depending on parameter passage.
            int sumOfPredictingEntity = 0;

            int projectCount = 0;

            this.projects.ForEach(delegate (Project project)
            {
                projectCount++;

                if (predictingItem == this.ticketsString)
                {
                    sumOfPredictingEntity += project.Tickets.Count;
                } else
                {
                    sumOfPredictingEntity += project.Members.Count;
                }
                
            });

            predictionNum = (sumOfPredictingEntity + projectCount * 2) / projectCount;

            return predictionNum;
        }

        public Prediction MakePrediction(Project predictionProject)
        {
            Prediction prediction = new Prediction();
            prediction.NumOfMembers = this.calculateRequiredNum(this.membersString);
            prediction.NumOfTickets = this.calculateRequiredNum(this.ticketsString);
            prediction.DurationByHistoricalData = this.calculateDurationByHistoricalData();
            prediction.DurationByTicketComplexity = this.calculateDurationByTicketComplexity(predictionProject);

            return new Prediction();
        }

        // 
        public string calculateDurationByTicketComplexity(Project project)
        {
            int daysSum = 0;

            project.Tickets.ForEach(delegate (Ticket ticket)
            {
                if (ticket.Complexity == TicketComplexity.Easy.ToString())
                {
                    daysSum += 2;
                } else if (ticket.Complexity == TicketComplexity.Hard.ToString())
                {
                    daysSum += 4;
                } else
                {
                    daysSum += 7;
                }
            });

            return $"Predicted project duration by opened tickets complexity is {daysSum} days";
        }

        public string calculateDurationByHistoricalData()
        {
            int totalDays = 0;
            int predictedDays = 0;
            int totalProjects = this.projects.Count;

            projects.ForEach(delegate (Project project)
            {
                int projectDuration = (int)(project.ClosedDate - project.StartedDate).TotalDays + 2;
                totalDays += projectDuration;
            });

            predictedDays = totalDays / totalProjects;

            return $"Predicted project duration by company historical data is {predictedDays} days";
        }
    }
}

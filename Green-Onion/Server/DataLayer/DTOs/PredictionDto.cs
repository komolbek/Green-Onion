using System;

namespace GreenOnion.Server.DataLayer.DTOs
{
    public class PredictionDto
    {
        private int requiredNumOfMembers;
        private int requiredNumOfTickets;

        // TicketComplexity enum is used here.
        // Adding Easy - 2 days, Medium - 4 days & Hard - 6 days while prediction.
        private string projectDurationByTicketComplexity;

        // "historical data" means the projects of the company & the tickets of the projects.
        private string projectDurationByHistoricalData;

        // defines predicted due date of the project using current tickets complexity
        // TicketComplexity enum is used here.
        // Adding Easy - 2 days, Medium - 4 days & Hard - 6 days while prediction.
        private string projectDueByTicketComplexity;

        // defines predicted due date of the project using historical data
        // "historical data" means the projects of the company & the tickets of the projects.
        private string projectDueByHistoricalData;

        public int NumOfMembers { get => requiredNumOfMembers; set => requiredNumOfMembers = value; }
        public int NumOfTickets { get => requiredNumOfTickets; set => requiredNumOfTickets = value; }
        public string DurationByTicketComplexity { get => projectDurationByTicketComplexity; set => projectDurationByTicketComplexity = value; }
        public string DurationByHistoricalData { get => projectDurationByHistoricalData; set => projectDurationByHistoricalData = value; }
        public string ProjectDueByTicketComplexity { get => projectDueByTicketComplexity; set => projectDueByTicketComplexity = value; }
        public string ProjectDueByHistoricalData { get => projectDueByHistoricalData; set => projectDueByHistoricalData = value; }

        public PredictionDto()
        {
        }
    }
}

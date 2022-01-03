using System;
namespace GreenOnion.DomainModels
{
    public class Prediction
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
        private DateTime projectDueByTicketComplexity;

        // defines predicted due date of the project using historical data
        // "historical data" means the projects of the company & the tickets of the projects.
        private DateTime projectDueByHistoricalData;

        public int RequiredNumOfMembers { get => requiredNumOfMembers; set => requiredNumOfMembers = value; }
        public int RequiredNumOfTickets { get => requiredNumOfTickets; set => requiredNumOfTickets = value; }
        public string ProjectDurationByTicketComplexity { get => projectDurationByTicketComplexity; set => projectDurationByTicketComplexity = value; }
        public string ProjectDurationByHistoricalData { get => projectDurationByHistoricalData; set => projectDurationByHistoricalData = value; }
        public DateTime ProjectDueByTicketComplexity { get => projectDueByTicketComplexity; set => projectDueByTicketComplexity = value; }
        public DateTime ProjectDueByHistoricalData { get => projectDueByHistoricalData; set => projectDueByHistoricalData = value; }

        public Prediction()
        {
        }
    }
}

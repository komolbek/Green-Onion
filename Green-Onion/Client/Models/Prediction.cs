using System;

namespace GreenOnion.Client.Models
{
    public class Prediction
    {
        public Prediction()
        {
        }

        public int requiredNumOfMembers { get; set; }
        public int requiredNumOfTickets { get; set; }
        public string projectDurationByTicketComplexity { get; set; }
        public string projectDurationByHistoricalData { get; set; }
        public DateTime projectDueByTicketComplexity { get; set; }
        public DateTime projectDueByHistoricalData { get; set; }
    }
}

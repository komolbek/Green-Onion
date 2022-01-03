using System;
using GreenOnion.DomainModels;
using System.Collections.Generic;

namespace GreenOnion.Services
{
    public class PredictionManagerService
    {
        private Prediction prediction;
        private List<Project> projects;

        public PredictionManagerService()
        {
        }

        public int calculateRequiredNumOfTickets()
        {
            return 0;
        }

        public int calculateRequiredNumOfMembers()
        {
            return 0;
        }

        public Prediction MakePrediction()
        {
            return new Prediction();
        }

        // 
        public string calculateProjectDurationByTicketComplexity()
        {
            return "";
        }

        public string calculateProjectDurationByHistoricalData()
        {
            return "";
        }

        private string makeStringFromDate()
        {
            return "";
        }
    }
}

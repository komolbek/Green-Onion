using System;

namespace GreenOnion.Server.DataLayer.DTOs
{
    public class ProjectRange
    {
        public ProjectRange()
        {
        }

        private DateTime startDate;
        private DateTime endDate;

        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
    }
}

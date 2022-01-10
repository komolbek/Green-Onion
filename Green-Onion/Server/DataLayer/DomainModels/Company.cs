using System.Collections.Generic;

namespace GreenOnion.Server.DataLayer.DomainModels
{
    public class Company
    {
        public Company()
        {
        }

        private string companyID;
        private User creatorId;
        private string name;
        private string aboutInfo;
        private List<Project> projects;

        public string CompanyID { get => companyID; set => companyID = value; }
        public User CreatorID { get => creatorId; set => creatorId = value; }
        public string Name { get => name; set => name = value; }
        public string AboutInfo { get => aboutInfo; set => aboutInfo = value; }
        public List<Project> Projects { get => projects; set => projects = value; }
    }
}

using System;
namespace GreenOnion.DomainModels
{
    public class Company
    {
        public Company()
        {
        }

        private string companyID;
        private string name;
        private string aboutInfo;
        private Project[] projects;

        public string CompanyID { get => companyID; set => companyID = value; }
        public string Name { get => name; set => name = value; }
        public string AboutInfo { get => aboutInfo; set => aboutInfo = value; }
        public Project[] Projects { get => projects; set => projects = value; }
    }
}

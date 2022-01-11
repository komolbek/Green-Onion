using System.Collections.Generic;

namespace GreenOnion.Client.Models
{
    public class Company
    {
        public Company()
        {
        }

        public string companyId { get; set; }
        public User creatorId { get; set; }
        public string name { get; set; }
        public string aboutInfo { get; set; }
        public List<Project> projects { get; set; }
    }
}

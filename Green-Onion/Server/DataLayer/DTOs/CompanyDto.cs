using System.Collections.Generic;

namespace GreenOnion.Server.DataLayer.DTOs
{
    public class CompanyDto
    {
        public CompanyDto()
        {
        }

        public string companyId { get; set; }

        public UserDto creator { get; set; }

        public string name { get; set; }

        public string aboutInfo { get; set; }

        public List<ProjectDto> projects { get; set; }

        public List<UserDto> employees { get; set; }
    }
}

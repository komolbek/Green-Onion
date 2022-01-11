using System;
namespace GreenOnion.Server.DataLayer.DTOs
{
    public class CompanyDto
    {
        public CompanyDto()
        {
        }

        [Key]
        [Required]
        private string companyId;

        [Required]
        [ForeignKey("User")]
        private string userId;

        [Required]
        private string name;

        private string aboutInfo;

        // has many projects
        private List<Project> projects;

        // has many employees
        private List<User> employees;

        public string CompanyId { get => companyId; set => companyId = value; }
        public string UserId { get => userId; set => userId = value; }
        public string Name { get => name; set => name = value; }
        public string AboutInfo { get => aboutInfo; set => aboutInfo = value; }
        public List<Project> Projects { get => projects; set => projects = value; }
        public List<User> Employees { get => employees; set => employees = value; }
    }
}

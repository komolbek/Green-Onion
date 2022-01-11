using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenOnion.Server.DataLayer.DomainModels
{
    public class Project
    {
        public Project()
        {

        }

        [Key]
        [Required]
        public string projectId { get; set; }

        // Indicates the company in which the project is created. 1 to N.
        [Required]
        [ForeignKey("Company"), Column(Order = 2)]
        public string companyId { get; set; }

        // Indicates the user who created the project. Thus that user can delete, change the project & add members. 1 to M
        [Required]
        [ForeignKey("User"), Column(Order = 3)]
        public string userId { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string startedDate { get; set; }

        public string closedDate { get; set; }
        public string dueDate { get; set; }
    }
}

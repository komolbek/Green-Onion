using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenOnion.Server.DataLayer.DomainModels
{
    public class ProjectMember
    {
        public ProjectMember()
        {
        }

        // indicates project where user added as member.
        [Required]
        [ForeignKey("Project"), Column(Order = 0)]
        public string projectId { get; set; }


        // indicates user who were added into the project as member.
        // Users who are not created a project cant delete or change it.
        // P.S. User who creates project automatically becomes member.
        [Required]
        [ForeignKey("User"), Column(Order = 1)]
        public string userId { get; set; }
    }
}

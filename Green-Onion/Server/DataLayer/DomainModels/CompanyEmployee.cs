using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenOnion.Server.DataLayer.DomainModels
{
    public class CompanyEmployee
    {
        public CompanyEmployee()
        {
        }

        // indicates company where user added as employee.
        [Required]
        [ForeignKey("Company"), Column(Order = 0)]
        public string companyId { get; set; }

        // indicates user who were added into the company as employee.
        // Users who are not created a company cant delete or change it.
        // P.S. User who creates company automatically becomes employee.
        [Required]
        [ForeignKey("User"), Column(Order = 1)]
        public string userId { get; set; }
    }
}

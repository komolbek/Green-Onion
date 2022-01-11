using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenOnion.Server.DataLayer.DomainModels
{
    public class Company
    {
        public Company()
        {
        }

        [Key]
        [Required]
        public string companyId { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        [ForeignKey("User"), Column(Order = 2)]
        public string userId { get; set; }

        public string aboutInfo { get; set; }
    }
}

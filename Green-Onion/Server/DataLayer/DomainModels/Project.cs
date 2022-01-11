using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenOnion.Server.DataLayer.DomainModels
{
    public class Project
    {
        public Project()
        {

        }

        // Primary keys
        [Key]
        [Required]
        private string projectId;

        // Foreign keys
        [Required]
        [ForeignKey("Company"), Column(Order = 0)]
        private string companyId;

        [Required]
        [ForeignKey("User"), Column(Order = 1)]
        private string userId;              

        // Has many
        private List<Ticket> tickets;

        // Has many
        [Required]
        private List<User> members;

        [Required]
        private string name;

        [Required]
        private DateTime startedDate;

        private DateTime closedDate;
        private DateTime dueDate;        

        public string ProjectId { get => projectId; set => projectId = value; }
        public string CompanyId { get => companyId; set => companyId = value; }
        public string UserId { get => userId; set => userId = value; }
        public string Name { get => name; set => name = value; }
        public List<Ticket> Tickets { get => tickets; set => tickets = value; }
        public DateTime StartedDate { get => startedDate; set => startedDate = value; }
        public DateTime ClosedDate { get => closedDate; set => closedDate = value; }
        public DateTime DueDate { get => dueDate; set => dueDate = value; }
        public List<User> Members { get => members; set => members = value; }
    }
}

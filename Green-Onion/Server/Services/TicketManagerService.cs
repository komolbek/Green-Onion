using System;
using GreenOnion.DataMappers;
using GreenOnion.DomainModels;
using GreenOnion.Enums;

namespace GreenOnion.Services
{
    public class TicketManagerService
    {
        private TicketDataMapper ticketDataMapper;
        
        public TicketManagerService()
        {
            this.ticketDataMapper = new TicketDataMapper();
        }
                
        public bool CreateTicket(string name, string description, string creatorID, string projectID, string complexity, DateTime? due = null)
        {
            Ticket ticket = new Ticket();
            ticket.Name = name;
            ticket.Description = description;
            ticket.ProjectID = projectID;
            ticket.CreatorID = creatorID;
            ticket.StartedDate = DateTime.Today;
            ticket.DueDate = due;
            ticket.ClosedDate = null;

            // all tickets are created in backlog list by default
            ticket.Status = TicketStatus.Backlog.ToString();

            // while creating Ticket if user doesnt set ticket complexity then Easy is used by default
            if (complexity is null)
            {
                ticket.Complexity = TicketComplexity.Easy.ToString();
            }
            else
            {
                ticket.Complexity = complexity;
            }
            

            return this.ticketDataMapper.Insert(ticket);
        }

        public bool ChangeTicket(string ticketID, string name, string description, string status, string complexity, DateTime due, DateTime closedDate)
        {

            Ticket ticket = this.ticketDataMapper.Select(ticketID);
            ticket.Name = name;
            ticket.Description = description;
            ticket.Status = status;
            ticket.DueDate = due;
            ticket.ClosedDate = closedDate;

            return this.ticketDataMapper.Update(ticket);
        }

        public bool DeleteTicket(string ticketID) =>this.ticketDataMapper.Delete(ticketID);

        public Ticket GetTicket(string ticketID) => this.ticketDataMapper.Select(ticketID);

        public User GetAssignee(string ticketID) => this.ticketDataMapper.SelectAssignee(ticketID);

        // TODO: maybe combine this functionality with ChangeTicket()?
        public bool AssignMember(string ticketID, string assigneeID)
        {
            Ticket ticket = this.ticketDataMapper.Select(ticketID);
            ticket.AssigneeID = assigneeID;

            return this.ticketDataMapper.UpdateAssignee(ticket);
        }

        public bool RemoveMember(string ticketID, string assigneeID)
        {
            Ticket ticket = this.ticketDataMapper.Select(ticketID);
            ticket.AssigneeID = null;

            return this.ticketDataMapper.RemoveAssignee(ticket);
        }
    }
}

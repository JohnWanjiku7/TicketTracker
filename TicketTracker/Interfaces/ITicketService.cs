using TicketTracker.Data;

namespace TicketTracker.Interfaces
{
    public interface ITicketService
    {
        Task<List<Ticket>> GetTicketsAsync();
        Task<Ticket> GetTicketsByIdAsync(int id);
        Task<Ticket> CreateTicket(Ticket ticket);
        Task UpdateTicketAsync(Ticket updateTicket, ApplicationUser editor, int ticketId);
        Task ResolveTicketAsync(int ticketId, ApplicationUser resolver);
    }
}

using TicketTracker.Data;

namespace TicketTracker.Interfaces
{
    public interface ITicketRepository
    {
        Task<Ticket> GetTicketByIdAsync(int ticketId);
        Task<List<Ticket>> GetTicketsAsync();
        Task AddTicketAsync(Ticket ticket);
        void DeleteTicket(Ticket ticket);
        Task UpdateTicketAsync(Ticket ticket);
    }
}

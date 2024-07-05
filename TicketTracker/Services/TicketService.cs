using TicketTracker.Data;
using TicketTracker.Interfaces;

namespace TicketTracker.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Ticket> CreateTicket(Ticket createRequest)

        {
            var ticket = Ticket.Create(
                createRequest.Type,
                createRequest.Summary,
                createRequest.Description,
                createRequest.Severity,
                createRequest.Priority,
                createRequest.CreatedBy);
            _ticketRepository.AddTicketAsync(ticket);
            return ticket;
        }

        public async Task UpdateTicketAsync(Ticket updateTicket,
           ApplicationUser editor,
           int ticketId)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);
            if (ticket == null)
            {
                throw new KeyNotFoundException("Ticket not found");
            }

            ticket.Update(updateTicket.Summary, updateTicket.Description, updateTicket.Severity, updateTicket.Priority, editor);
            await _ticketRepository.UpdateTicketAsync(ticket);
        }


        public async Task ResolveTicketAsync(int ticketId, ApplicationUser resolver)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);
            if (ticket == null)
            {
                throw new KeyNotFoundException("Ticket not found");
            }

            ticket.ResolveTicket(resolver);
            await _ticketRepository.UpdateTicketAsync(ticket);
        }


        public async Task<List<Ticket>> GetTicketsAsync()
        {

            return await _ticketRepository.GetTicketsAsync();
        }
        public async Task<Ticket> GetTicketsByIdAsync(int id)
        {

            return await _ticketRepository.GetTicketByIdAsync(id);
        }

    }

}

using Microsoft.EntityFrameworkCore;
using System;
using TicketTracker.Data;
using TicketTracker.Interfaces;

namespace TicketTracker.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            return await _context.Tickets
                .FirstOrDefaultAsync(m => m.TicketId == ticketId);
        }
        public async Task<List<Ticket>> GetTicketsAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task AddTicketAsync(Ticket ticket)
        {
            _context.Add(ticket);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateTicketAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public void DeleteTicket(Ticket ticket)
        {
            // Data deletion logic
            throw new NotImplementedException();
        }
    }
}

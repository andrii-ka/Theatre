using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Theatre.TicketOffice.Data;
using Theatre.TicketOffice.Models;

namespace Theatre.TicketOffice.Services
{
    public interface IBookingsService
    {
        Task BookTickets(int showTimeId, int ticketsToBook, string userId);
    }

    public class BookingsService : IBookingsService
    {
        private ApplicationDbContext _context;

        public BookingsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task BookTickets(int showTimeId, int ticketsToBook, string userId)
        {
            var showTime = await _context.ShowTimes.Include(s => s.Bookings).SingleOrDefaultAsync(s => s.ID == showTimeId);
            if (showTime == null)
            {
                throw new Exception("Show time not found");
            }

            var ticketsAvailable = showTime.TicketsTotal - showTime.Bookings.Sum(b => b.TicketsBooked);
            if (ticketsToBook > ticketsAvailable)
            {
                throw new Exception($"Cannot book {ticketsToBook} tickets. Not enough tickets left.");
            }

            var booking = new Booking { ShowTimeID = showTimeId, TicketsBooked = ticketsToBook, UserID = userId };
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
        }
    }
}

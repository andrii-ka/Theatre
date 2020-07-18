using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.TicketOffice.Models
{
    public class Booking
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public int ShowTimeID { get; set; }
        public int TicketsBooked { get; set; }
        public DateTime BookedAtUtc { get; set; }

        public ShowTime ShowTime { get; set; }
    }
}

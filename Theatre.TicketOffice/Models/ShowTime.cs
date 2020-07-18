using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.TicketOffice.Models
{
    public class ShowTime
    {
        public int ID { get; set; }
        public int ShowID { get; set; }
        public DateTime StartDateUtc { get; set; }
        public int TicketsTotal { get; set; }

        public Show Show { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}

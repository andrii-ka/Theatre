using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.TicketOffice.Models
{
    public class Show
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<ShowTime> ShowTimes { get; set; }
    }
}

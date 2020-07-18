using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.TicketOffice.ViewModels
{
    public class AddShowTimesParams
    {
        [Required]
        public int ShowID { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public int TicketsTotal { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
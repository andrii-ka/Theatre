using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.TicketOffice.ViewModels
{
    public class ShowTimeViewModel
    {
        public int ID { get; set; }
        [Required]
        public int ShowID { get; set; }
        public string ShowName { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public int TicketsTotal { get; set; }

        public string DayOfWeek => StartDate.DayOfWeek.ToString();
    }
}

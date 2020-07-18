using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.TicketOffice.ViewModels
{
    public class ShowViewModel
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}

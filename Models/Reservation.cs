using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2FlightReservations.Models
{
    internal class Reservation
    {
        public string FlightCode { get; set; }
        public string Airline { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public int Cost { get; set; }
        public string Name { get; set; }
        public string Citizenship { get; set; }

    }
}

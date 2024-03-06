using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2FlightReservations.Models
{
    public class Reservation
    {
        public string ReservationCode { get; set; }
        public string FlightCode { get; set; }
        public string Airline { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public double Cost { get; set; }
        public string Name { get; set; }
        public string Citizenship { get; set; }

    }
}
